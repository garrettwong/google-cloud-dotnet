﻿// Copyright 2017 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Cloud.Spanner.V1;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Cloud.Spanner.Data
{
    /// <summary>
    /// Represents a SQL transaction to be made in a Spanner database.
    /// A transaction in Cloud Spanner is a set of reads and writes that execute
    /// atomically at a single logical point in time across columns, rows, and
    /// tables in a database.
    /// </summary>
    public sealed class SpannerTransaction : DbTransaction, ISpannerTransaction
    {
        private readonly List<Mutation> _mutations = new List<Mutation>();
        private DisposeBehavior _disposeBehavior = DisposeBehavior.ReleaseToPool;
        private bool _disposed = false;

        /// <summary>
        /// When executing multiple DML commands in a single transaction, each is given a specific sequence number
        /// to indicate the difference between "apply this DML command twice" and "I'm replaying a request due to a transient failure".
        /// The first request uses a sequence number of 1 to make it clear that it's been set deliberately.
        /// </summary>
        private int _lastDmlSequenceNumber = 0;

        /// <inheritdoc />
        public override IsolationLevel IsolationLevel => IsolationLevel.Serializable;

        /// <summary>
        /// Indicates the <see cref="TransactionMode"/> for the transaction.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Cloud Spanner supports two transaction modes:
        /// <list type="bullet">
        /// <item><description>
        /// Locking read-write transactions are the only transaction type that support writing
        /// data into Cloud Spanner. These transactions rely on pessimistic locking and, if
        /// necessary, two-phase commit. Locking read-write transactions may abort, requiring
        /// the application to retry.
        /// </description></item>
        /// <item><description>
        /// Read-only transactions provide guaranteed consistency across several reads,
        /// but do not allow writes. Read-only transactions can be configured to read at
        /// timestamps in the past. Read-only transactions do not need to be committed and
        /// do not take locks.
        /// </description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public TransactionMode Mode { get; }

        private readonly PooledSession _session;

        private int _commitTimeout;

        // Note: We use seconds here to follow the convention set by DbCommand.CommandTimeout.
        /// <summary>
        /// Gets or sets the wait time before terminating the attempt to <see cref="Commit()"/> 
        /// or <see cref="Rollback"/> and generating an error.
        /// Defaults to the timeout from the connection string. A value of '0' normally indicates that no timeout should be used (it waits an infinite amount of time).
        /// However, if you specify AllowImmediateTimeouts=true in the connection string, '0' will cause a timeout
        /// that expires immediately. This is normally used only for testing purposes.
        /// </summary>
        public int CommitTimeout
        {
            get => _commitTimeout;
            set => _commitTimeout = GaxPreconditions.CheckArgumentRange(value, nameof(value), 0, int.MaxValue);
        }

        /// <summary>
        /// Tells Cloud Spanner how to choose a timestamp at which to read the data for read-only
        /// transactions.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The types of timestamp bounds are:
        /// <list type="bullet">
        ///   <item><description>Strong (the default): read the latest data.</description></item>
        ///   <item><description>Bounded staleness: read a version of the data that's no staler than a bound.</description></item>
        ///   <item><description>Exact staleness: read the version of the data at an exact timestamp.</description></item>
        /// </list>
        /// </para>
        /// </remarks>
        public TimestampBound TimestampBound { get; }

        /// <inheritdoc />
        protected override DbConnection DbConnection => SpannerConnection;

        /// <summary>
        /// <see cref="SpannerConnection"/> associated with this transaction.
        /// </summary>
        internal SpannerConnection SpannerConnection { get; }

        internal bool HasMutations
        {
            get
            {
                lock (_mutations)
                {
                    return _mutations.Any();
                }
            }
        }

        internal SpannerTransaction(
            SpannerConnection connection,
            TransactionMode mode,
            PooledSession session,
            TimestampBound timestampBound)
        {
            SpannerConnection = GaxPreconditions.CheckNotNull(connection, nameof(connection));
            CommitTimeout = SpannerConnection.Builder.Timeout;
            Mode = mode;
            _session = GaxPreconditions.CheckNotNull(session, nameof(session));
            TimestampBound = timestampBound;
        }

        /// <summary>
        /// Returns true if this transaction is being used by multiple <see cref="SpannerConnection"/> objects.
        /// <see cref="SpannerCommand.GetReaderPartitionsAsync"/> will automatically mark the transaction as shared
        /// because it is expected that you will be distributing the read among several tasks or processes.
        /// </summary>
        public bool Shared { get; internal set; }

        /// <summary>
        /// Specifies how resources are treated when <see cref="Dispose"/> is called.
        /// The default behavior of <see cref="DisposeBehavior.ReleaseToPool"/> will cause transactional resources
        /// to be sent back into a shared pool for re-use.
        /// Shared transactions may only set this value to either <see cref="DisposeBehavior.CloseResources"/> to close
        /// resources or <see cref="DisposeBehavior.Detach"/> to detach from the resources.
        /// A shared transaction must have one process choose <see cref="DisposeBehavior.CloseResources"/>
        /// to avoid leaks of transactional resources.
        /// </summary>
        public DisposeBehavior DisposeBehavior
        {
            get => _disposeBehavior;
            set => _disposeBehavior = GaxPreconditions.CheckEnumValue(value, nameof(DisposeBehavior));
        }

        /// <summary>
        /// Creates a new <see cref="SpannerBatchCommand"/> to execute batched DML statements within this transaction.
        /// You can add commands to the batch by using <see cref="SpannerBatchCommand.Add(SpannerCommand)"/>,
        /// <see cref="SpannerBatchCommand.Add(SpannerCommandTextBuilder, SpannerParameterCollection)"/>
        /// and <see cref="SpannerBatchCommand.Add(string, SpannerParameterCollection)"/>.
        /// </summary>
        public SpannerBatchCommand CreateBatchDmlCommand() => new SpannerBatchCommand(this);

        internal Task<IEnumerable<ByteString>> GetPartitionTokensAsync(
            ExecuteSqlRequest request,
            long? partitionSizeBytes,
            long? maxPartitions,
            CancellationToken cancellationToken,
            int timeoutSeconds)
        {
            GaxPreconditions.CheckNotNull(request, nameof(request));
            GaxPreconditions.CheckState(Mode == TransactionMode.ReadOnly, "You can only call GetPartitions on a read-only transaction.");

            // Calling this method marks the used transaction as "shared" - but does not set
            // DisposeBehavior to any value. This will cause an exception during dispose that tells the developer
            // that they need to handle this condition by explicitly setting DisposeBehavior to some value.
            Shared = true;

            var partitionRequest = new PartitionQueryRequest
            {
                Sql = request.Sql,
                Params = request.Params,
                ParamTypes = { request.ParamTypes },
                PartitionOptions = partitionSizeBytes.HasValue || maxPartitions.HasValue ? new PartitionOptions() : null,
            };
            if (partitionSizeBytes.HasValue)
            {
                partitionRequest.PartitionOptions.PartitionSizeBytes = partitionSizeBytes.Value;
            }
            if (maxPartitions.HasValue)
            {
                partitionRequest.PartitionOptions.MaxPartitions = maxPartitions.Value;
            }
            return ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
                {
                    var callSettings = SpannerConnection.CreateCallSettings(settings => settings.PartitionQuerySettings, timeoutSeconds, cancellationToken);
                    var response = await _session.PartitionQueryAsync(partitionRequest, callSettings).ConfigureAwait(false);
                    return response.Partitions.Select(x => x.PartitionToken);
                },
                "SpannerTransaction.GetPartitionTokensAsync", SpannerConnection.Logger);
        }

        Task<int> ISpannerTransaction.ExecuteMutationsAsync(
            List<Mutation> mutations,
            CancellationToken cancellationToken,
            int timeoutSeconds)
        {
            CheckCompatibleMode(TransactionMode.ReadWrite);
            return ExecuteHelper.WithErrorTranslationAndProfiling(() =>
            {
                var taskCompletionSource = new TaskCompletionSource<int>();
                cancellationToken.ThrowIfCancellationRequested();
                lock (_mutations)
                {
                    _mutations.AddRange(mutations);
                }
                taskCompletionSource.SetResult(mutations.Count);
                return taskCompletionSource.Task;
            }, "SpannerTransaction.ExecuteMutations", SpannerConnection.Logger);
        }

        Task<ReliableStreamReader> ISpannerTransaction.ExecuteQueryAsync(
            ExecuteSqlRequest request,
            CancellationToken cancellationToken,
            int timeoutSeconds) // Ignored
        {
            GaxPreconditions.CheckNotNull(request, nameof(request));
            CheckCompatibleMode(TransactionMode.ReadOnly);
            // We're not making any Spanner requests here, so we don't need profiling or error translation.
            var callSettings = SpannerConnection.CreateCallSettings(settings => settings.ExecuteStreamingSqlSettings, cancellationToken);
            return Task.FromResult(_session.ExecuteSqlStreamReader(request, callSettings));
        }

        Task<long> ISpannerTransaction.ExecuteDmlAsync(ExecuteSqlRequest request, CancellationToken cancellationToken, int timeoutSeconds)
        {
            CheckCompatibleMode(TransactionMode.ReadWrite);
            GaxPreconditions.CheckNotNull(request, nameof(request));
            request.Seqno = Interlocked.Increment(ref _lastDmlSequenceNumber);
            return ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
            {
                // Note: ExecuteSql would work, but by using a streaming call we enable potential future scenarios
                // where the server returns interim resume tokens to avoid timeouts.
                var callSettings = SpannerConnection.CreateCallSettings(settings => settings.ExecuteStreamingSqlSettings, timeoutSeconds, cancellationToken);
                using (var reader = _session.ExecuteSqlStreamReader(request, callSettings))
                {
                    Value value = await reader.NextAsync(cancellationToken).ConfigureAwait(false);
                    if (value != null)
                    {
                        throw new SpannerException(ErrorCode.Internal, "DML returned results unexpectedly.");
                    }

                    var stats = reader.Stats;
                    if (stats == null)
                    {
                        throw new SpannerException(ErrorCode.Internal, "DML completed without statistics.");
                    }
                    switch (stats.RowCountCase)
                    {
                        case ResultSetStats.RowCountOneofCase.RowCountExact:
                            return stats.RowCountExact;
                        case ResultSetStats.RowCountOneofCase.RowCountLowerBound:
                            return stats.RowCountLowerBound;
                        default:
                            throw new SpannerException(ErrorCode.Internal, $"Unknown row count type: {stats.RowCountCase}");
                    }
                }
            }, "SpannerTransaction.ExecuteDml", SpannerConnection.Logger);
        }

        Task<IEnumerable<long>> ISpannerTransaction.ExecuteBatchDmlAsync(ExecuteBatchDmlRequest request, CancellationToken cancellationToken, int timeoutSeconds)
        {
            CheckCompatibleMode(TransactionMode.ReadWrite);
            GaxPreconditions.CheckNotNull(request, nameof(request));
            request.Seqno = Interlocked.Increment(ref _lastDmlSequenceNumber);
            return ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
            {
                var callSettings = SpannerConnection.CreateCallSettings(settings => settings.ExecuteBatchDmlSettings, timeoutSeconds, cancellationToken);
                ExecuteBatchDmlResponse response = await _session.ExecuteBatchDmlAsync(request, callSettings).ConfigureAwait(false);
                IEnumerable<long> result = response.ResultSets.Select(rs => rs.Stats.RowCountExact);
                if (response.Status.Code == (int) Rpc.Code.Ok)
                {
                    return result;
                }
                else
                {
                    throw new SpannerBatchNonQueryException(response.Status, result);
                }
            }, "SpannerTransaction.ExecuteBatchDml", SpannerConnection.Logger);
        }

        /// <inheritdoc />
        public override void Commit() => Commit(out _);

        // Note: it would be nice just to use Commit, but that's taken by the void method.
        /// <summary>
        /// Commits the database transaction synchronously, returning the database timestamp for the commit via <paramref name="timestamp"/>.
        /// </summary>
        /// <param name="timestamp">Returns the UTC timestamp when the data was written to the database.</param>
        public void Commit(out DateTime timestamp) => timestamp = Task.Run(() => CommitAsync(default)).ResultWithUnwrappedExceptions();

        /// <summary>
        /// Commits the database transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token used for this task.</param>
        /// <returns>Returns the UTC timestamp when the data was written to the database.</returns>
        public Task<DateTime> CommitAsync(CancellationToken cancellationToken = default)
        {
            GaxPreconditions.CheckState(Mode != TransactionMode.ReadOnly, "You cannot commit a readonly transaction.");
            var request = new CommitRequest { Mutations = { _mutations } };
            return ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
            {
                var callSettings = SpannerConnection.CreateCallSettings(settings => settings.CommitSettings, CommitTimeout, cancellationToken);
                var response = await _session.CommitAsync(request, callSettings).ConfigureAwait(false);
                if (response.CommitTimestamp == null)
                {
                    throw new SpannerException(ErrorCode.Internal, "Commit succeeded, but returned a response with no commit timestamp");
                }
                return response.CommitTimestamp.ToDateTime();
            },
            "SpannerTransaction.Commit", SpannerConnection.Logger);
        }

        /// <inheritdoc />
        public override void Rollback() => Task.Run(() => RollbackAsync(default)).WaitWithUnwrappedExceptions();

        /// <summary>
        /// Rolls back a transaction asynchronously.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token used for this task.</param>
        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            GaxPreconditions.CheckState(Mode != TransactionMode.ReadOnly, "You cannot roll back a readonly transaction.");
            var callSettings = SpannerConnection.CreateCallSettings(settings => settings.RollbackSettings, CommitTimeout, cancellationToken);
            return ExecuteHelper.WithErrorTranslationAndProfiling(
                () => _session.RollbackAsync(new RollbackRequest(), callSettings),
                "SpannerTransaction.Rollback", SpannerConnection.Logger);
        }

        /// <summary>
        /// Identifying information about this transaction.
        /// </summary>
        public TransactionId TransactionId => new TransactionId(
            SpannerConnection.ConnectionString,
            _session.SessionName.ToString(),
            _session.TransactionId.ToBase64(),
            TimestampBound);

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            switch (DisposeBehavior)
            {
                case DisposeBehavior.CloseResources:
                    _session.ReleaseToPool(forceDelete: true);
                    break;
                case DisposeBehavior.ReleaseToPool:
                    if (Shared)
                    {
                        // This guard will prevent accidental leaks by forcing the developer to think
                        // about how they want to manage the lifetime of the outer transactional resources.
                        throw new InvalidOperationException(
                            "When calling GetPartitionTokensAsync, you must indicate when transactional resources are released by setting DisposeBehavior=DisposeBehavior.CloseResources or DisposeBehavior.Detach");
                    }
                    _session.ReleaseToPool(forceDelete: false);
                    break;
                // Default for detach or unknown DisposeBehavior is to do nothing.
            }
        }

        private void CheckCompatibleMode(TransactionMode mode)
        {
            switch (mode)
            {
                case TransactionMode.ReadOnly:
                {
                    GaxPreconditions.CheckState(
                        Mode == TransactionMode.ReadOnly || Mode == TransactionMode.ReadWrite,
                        "You can only execute reads on a ReadWrite or ReadOnly Transaction!");
                }
                    break;
                case TransactionMode.ReadWrite:
                {
                    GaxPreconditions.CheckState(
                        Mode == TransactionMode.ReadWrite,
                        "You can only execute read/write commands on a ReadWrite Transaction!");
                }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }
    }
}
