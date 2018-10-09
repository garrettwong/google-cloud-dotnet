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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Google.Api.Gax;
using Google.Api.Gax.Grpc;
using Google.Cloud.Spanner.V1;
using Google.Cloud.Spanner.V1.Internal;
using Google.Cloud.Spanner.V1.Internal.Logging;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

#if !NETSTANDARD1_5
using Transaction = System.Transactions.Transaction;

#endif

namespace Google.Cloud.Spanner.Data
{
    /// <summary>
    /// Represents a connection to a single Spanner database.
    /// When opened, <see cref="SpannerConnection" /> will acquire and maintain a session
    /// with the target Spanner database.
    /// <see cref="SpannerCommand" /> instances using this <see cref="SpannerConnection" />
    /// will use this session to execute their operation. Concurrent read operations can
    /// share this session, but concurrent write operations may cause additional sessions
    /// to be opened to the database.
    /// Underlying sessions with the Spanner database are pooled and are closed after a
    /// configurable
    /// <see>
    /// <cref>SpannerOptions.PoolEvictionDelay</cref>
    /// </see>
    /// .
    /// </summary>
    public sealed class SpannerConnection : DbConnection
    {
        // Internally, a SpannerConnection acts as a local SessionPool.
        // When OpenAsync() is called, it creates a session with passthru transaction semantics and
        // allows other consumers to borrow that session.
        // Consumers may be SpannerTransactions or if the user has is not explicitly using transactions,
        // the consumer may be the SpannerCommand itself.
        // While SpannerTransaction has sole ownership of the session it obtains, SpannerCommand shares
        // any session it obtains with others.

        // Default transaction options: not valid to pass to Spanner to begin a transaction.
        private static readonly TransactionOptions s_defaultTransactionOptions = new TransactionOptions();
        // Read/write transaction options; no additional state, so can be reused.
        private static readonly TransactionOptions s_readWriteTransactionOptions = new TransactionOptions { ReadWrite = new TransactionOptions.Types.ReadWrite() };

        private readonly object _sync = new object();

        // This object is never mutated and never exposed to consumers.

        /// <summary>
        /// The current connection string builder. The object is never mutated and never exposed to consumers.
        /// The field itself may be changed to a new builder by setting the <see cref="ConnectionString"/>
        /// property, or within this class via the <see cref="TrySetNewConnectionInfo(SpannerConnectionStringBuilder)"/> method.
        /// This value may return null.
        /// </summary>
        private SpannerConnectionStringBuilder _connectionStringBuilder;

        private CancellationTokenSource _keepAliveCancellation;

        private Task _keepAliveTask;

        private int _sessionRefCount;
        private Session _sharedSession;

        private volatile Task<Session> _sharedSessionAllocator;
        private ConnectionState _state = ConnectionState.Closed;
        private readonly HashSet<string> _staleSessions = new HashSet<string>();

#if !NETSTANDARD1_5
        // State used for TransactionScope-based transactions.
        private TimestampBound _timestampBound;
        private VolatileResourceManager _volatileResourceManager;
        private TransactionId _transactionId;
#endif

        /// <summary>
        /// Provides options to customize how connections to Spanner are created
        /// and maintained.
        /// </summary>
        public static SpannerOptions SpannerOptions => SpannerOptions.Instance;

        /// <summary>
        /// Releases all pooled Cloud Spanner sessions.
        /// </summary>
        public static Task ClearPooledResourcesAsync() => SessionPool.Default.ReleaseAllAsync();

        /// <inheritdoc />
        public override string ConnectionString
        {
            get => _connectionStringBuilder?.ToString();
            set => TrySetNewConnectionInfo(
                new SpannerConnectionStringBuilder(value, _connectionStringBuilder?.CredentialOverride));
        }

        /// <summary>
        /// The <see cref="ChannelCredentials"/> credential used to communicate with Spanner, if explicitly
        /// set. Otherwise, this method returns null, usually indicating that default application credentials should be used.
        /// See Google Cloud documentation for more information.
        /// </summary>
        public ChannelCredentials GetCredentials() => _connectionStringBuilder?.GetCredentials();

        /// <inheritdoc />
        public override string Database => _connectionStringBuilder?.SpannerDatabase;

        /// <inheritdoc />
        public override string DataSource => _connectionStringBuilder?.DataSource;

        /// <summary>
        /// The Spanner project name.
        /// </summary>
        [Category("Data")]
        public string Project => _connectionStringBuilder?.Project;

        /// <inheritdoc />
        public override string ServerVersion => "0.0";

        /// <summary>
        /// The Spanner instance name
        /// </summary>
        [Category("Data")]
        public string SpannerInstance => _connectionStringBuilder?.SpannerInstance;

        /// <summary>
        /// This property is intended for internal use only.
        /// </summary>
        public Logger Logger { get; set; } = Logger.DefaultLogger;

        /// <inheritdoc />
        public override ConnectionState State => _state;

        internal bool IsClosed => (State & ConnectionState.Open) == 0;

        internal bool IsOpen => (State & ConnectionState.Open) == ConnectionState.Open;

        internal SpannerClient SpannerClient { get; private set; }

        /// <summary>
        /// Creates a SpannerConnection with no datasource or credential specified.
        /// </summary>
        public SpannerConnection() { }

        /// <summary>
        /// Creates a SpannerConnection with a datasource contained in connectionString
        /// and optional credential information supplied in connectionString or the credential
        /// argument.
        /// </summary>
        /// <param name="connectionString">
        /// A Spanner formatted connection string. This is usually of the form
        /// `Data Source=projects/{project}/instances/{instance}/databases/{database};[Host={hostname};][Port={portnumber}]`
        /// </param>
        /// <param name="credentials">An optional credential for operations to be performed on the Spanner database.  May be null.</param>
        public SpannerConnection(string connectionString, ChannelCredentials credentials = null)
            : this(new SpannerConnectionStringBuilder(connectionString, credentials)) { }

        /// <summary>
        /// Creates a SpannerConnection with a datasource contained in connectionString.
        /// </summary>
        /// <param name="connectionStringBuilder">
        /// A SpannerConnectionStringBuilder containing a formatted connection string.  Must not be null.
        /// </param>
        public SpannerConnection(SpannerConnectionStringBuilder connectionStringBuilder)
        {
            GaxPreconditions.CheckNotNull(connectionStringBuilder, nameof(connectionStringBuilder));
            TrySetNewConnectionInfo(connectionStringBuilder);
        }

        /// <summary>
        /// Begins a read-only transaction using the optionally provided <see cref="CancellationToken" />.
        /// Read transactions are preferred if possible because they do not impose locks internally.
        /// ReadOnly transactions run with strong consistency and return the latest copy of data.
        /// This method is thread safe.
        /// </summary>
        /// <param name="cancellationToken">An optional token for canceling the call. May be null.</param>
        /// <returns>The newly created <see cref="SpannerTransaction"/>.</returns>
        public Task<SpannerTransaction> BeginReadOnlyTransactionAsync(
            CancellationToken cancellationToken = default) => BeginReadOnlyTransactionAsync(
            TimestampBound.Strong, cancellationToken);

        /// <summary>
        /// Begins a read-only transaction using the optionally provided <see cref="CancellationToken" />
        /// and provided <see cref="TimestampBound" /> to control the read timestamp and/or staleness
        /// of data.
        /// Read transactions are preferred if possible because they do not impose locks internally.
        /// Stale read-only transactions can execute more quickly than strong or read-write transactions,.
        /// This method is thread safe.
        /// </summary>
        /// <param name="targetReadTimestamp">Specifies the timestamp or allowed staleness of data. Must not be null.</param>
        /// <param name="cancellationToken">An optional token for canceling the call.</param>
        /// <returns>The newly created <see cref="SpannerTransaction"/>.</returns>
        public Task<SpannerTransaction> BeginReadOnlyTransactionAsync(
            TimestampBound targetReadTimestamp,
            CancellationToken cancellationToken = default)
        {
            if (targetReadTimestamp.Mode == TimestampBoundMode.MinReadTimestamp
                || targetReadTimestamp.Mode == TimestampBoundMode.MaxStaleness)
            {
                throw new ArgumentException(
                    nameof(targetReadTimestamp),
                    $"{nameof(TimestampBoundMode.MinReadTimestamp)} and "
                    + $"{nameof(TimestampBoundMode.MaxStaleness)} can only be used in a single-use"
                    + " transaction as an argument to SpannerCommand.ExecuteReader().");
            }

            return BeginTransactionImplAsync(
                CreateReadOnlyTransactionOptions(targetReadTimestamp),
                TransactionMode.ReadOnly,
                cancellationToken,
                targetReadTimestamp);
        }

        /// <summary>
        /// Begins a read-only transaction.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Read-only transactions are preferred if possible because they do not impose locks internally.
        /// Read-only transactions run with strong consistency and return the latest copy of data.
        /// </para>
        /// <para>This method is thread safe.</para>
        /// </remarks>
        /// <returns>The newly created <see cref="SpannerTransaction"/>.</returns>
        public SpannerTransaction BeginReadOnlyTransaction() => BeginReadOnlyTransaction(TimestampBound.Strong);

        /// <summary>
        /// Begins a read-only transaction using the provided <see cref="TimestampBound"/> to control the read timestamp
        /// and/or staleness of data.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Read-only transactions are preferred if possible because they do not impose locks internally.
        /// Read-only transactions run with strong consistency and return the latest copy of data.
        /// </para>
        /// <para>This method is thread safe.</para>
        /// </remarks>
        /// <param name="targetReadTimestamp">Specifies the timestamp or allowed staleness of data. Must not be null.</param>
        /// <returns>The newly created <see cref="SpannerTransaction"/>.</returns>
        public SpannerTransaction BeginReadOnlyTransaction(TimestampBound targetReadTimestamp) =>
            BeginReadOnlyTransactionAsync(targetReadTimestamp).ResultWithUnwrappedExceptions();

        /// <summary>
        /// Begins a read-only transaction using the provided <see cref="TransactionId" /> to refer to an existing server-side transaction.
        /// </summary>
        /// <remarks>
        /// Read-only transactions are preferred if possible because they do not impose locks internally.
        /// Providing a transaction ID will connect to an already created transaction which is useful
        /// for batch reads. This method differs from <see cref="BeginReadOnlyTransaction()">the parameterless overload</see>
        /// and <see cref="BeginReadOnlyTransaction(TimestampBound)">the overload accepting a TimestampBound</see> as it
        /// uses an existing transaction rather than creating a new server-side transaction.
        /// </remarks>
        /// <param name="transactionId">Specifies the transaction ID of an existing read-only transaction.</param>
        /// <returns>A <see cref="SpannerTransaction"/> attached to the existing transaction represented by
        /// <paramref name="transactionId"/>.</returns>
        public SpannerTransaction BeginReadOnlyTransaction(TransactionId transactionId)
        {
            GaxPreconditions.CheckNotNull(transactionId, nameof(transactionId));
            return SpannerTransaction.FromTransactionId(this, transactionId);
        }

        private static TransactionOptions CreateReadOnlyTransactionOptions(TimestampBound targetReadTimestamp)
        {
            var innerOptions = new TransactionOptions.Types.ReadOnly();

            switch (targetReadTimestamp.Mode)
            {
                case TimestampBoundMode.Strong:
                    innerOptions.Strong = true;
                    break;
                case TimestampBoundMode.ReadTimestamp:
                    innerOptions.ReadTimestamp = Timestamp.FromDateTime(targetReadTimestamp.Timestamp);
                    break;
                case TimestampBoundMode.MinReadTimestamp:
                    innerOptions.MinReadTimestamp = Timestamp.FromDateTime(targetReadTimestamp.Timestamp);
                    break;
                case TimestampBoundMode.ExactStaleness:
                    innerOptions.ExactStaleness = Duration.FromTimeSpan(targetReadTimestamp.Staleness);
                    break;
                case TimestampBoundMode.MaxStaleness:
                    innerOptions.MaxStaleness = Duration.FromTimeSpan(targetReadTimestamp.Staleness);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return new TransactionOptions { ReadOnly = innerOptions };
        }

        /// <summary>
        /// Begins a new Spanner transaction synchronously. This method hides <see cref="DbConnection.BeginTransaction()"/>, but behaves
        /// the same way, just with a more specific return type.
        /// </summary>
        public new SpannerTransaction BeginTransaction() => (SpannerTransaction)base.BeginTransaction();

        /// <summary>
        /// Begins a new read/write transaction.
        /// This method is thread safe.
        /// </summary>
        /// <param name="cancellationToken">An optional token for canceling the call.</param>
        /// <returns>A new <see cref="SpannerTransaction" /></returns>
        public Task<SpannerTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default) =>
            BeginTransactionImplAsync(s_readWriteTransactionOptions, TransactionMode.ReadWrite, cancellationToken);

        /// <inheritdoc />
        public override void ChangeDatabase(string newDataSource)
        {
            if (IsOpen)
            {
                Close();
            }

            TrySetNewConnectionInfo(_connectionStringBuilder?.CloneWithNewDataSource(newDataSource));
        }

        /// <inheritdoc />
        public override void Close()
        {
            Session session;
            bool primarySessionInUse;
            bool releaseClient = true;

            var oldState = _state;
            lock (_sync)
            {
                if (IsClosed)
                {
                    return;
                }

                _keepAliveCancellation?.Cancel();
#if !NETSTANDARD1_5
                // In implicit transactions, the connection will be closed before the transaction is
                // completed. We expect the SpannerTransaction to have ownership of the session, so that
                // can be handled by disposal there, but we need to make sure the right SpannerClient is
                // used when releasing the session to the session pool, and that the SpannerClient is
                // released to the client pool too.
                if (_volatileResourceManager != null)
                {
                    // Capture current state as local variables so we know what we'll be disposing.
                    var connectionStringBuilder = _connectionStringBuilder;
                    var client = SpannerClient;

                    _volatileResourceManager.TransferClientOwnership(client, () => ReleaseClient(client, connectionStringBuilder));
                    releaseClient = false;
                    _volatileResourceManager = null;
                }
#endif
                primarySessionInUse = _sessionRefCount > 0;
                _state = ConnectionState.Closed;
                //we do not await the actual session close, we let that happen async.
                session = _sharedSession;
                _sharedSession = null;
            }
            if (session != null && !primarySessionInUse)
            {
                SessionPool.Default.ReleaseToPool(SpannerClient, session);
            }
            if (releaseClient)
            {
                ReleaseClient(SpannerClient, _connectionStringBuilder);
            }
            SpannerClient = null;
            if (oldState != _state)
            {
                OnStateChange(new StateChangeEventArgs(oldState, _state));
            }
        }

        private static void ReleaseClient(SpannerClient client, SpannerConnectionStringBuilder connectionStringBuilder)
        {
            if (client != null)
            {
                ClientPool.Default.ReleaseClient(client, connectionStringBuilder);
            }
        }

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to delete rows from a Spanner database table.
        /// This method is thread safe.
        /// </summary>
        /// <param name="databaseTable">The name of the table from which to delete rows. Must not be null.</param>
        /// <param name="primaryKeys">The set of columns that form the primary key of the table.</param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateDeleteCommand(
            string databaseTable,
            SpannerParameterCollection primaryKeys = null) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateDeleteTextBuilder(databaseTable), this, null,
            primaryKeys);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to insert rows into a Spanner database table.
        /// This method is thread safe.
        /// </summary>
        /// <param name="databaseTable">The name of the table to insert rows into. Must not be null.</param>
        /// <param name="insertedColumns">
        /// A collection of <see cref="SpannerParameter" />
        /// where each instance represents a column in the Spanner database table being set.
        /// May be null.
        /// </param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateInsertCommand(
            string databaseTable,
            SpannerParameterCollection insertedColumns = null) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateInsertTextBuilder(databaseTable), this, null,
            insertedColumns);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to insert or update rows into a Spanner database table.
        /// This method is thread safe.
        /// </summary>
        /// <param name="databaseTable">The name of the table to insert or updates rows. Must not be null.</param>
        /// <param name="insertUpdateColumns">
        /// A collection of <see cref="SpannerParameter" />
        /// where each instance represents a column in the Spanner database table being set.
        /// May be null
        /// </param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateInsertOrUpdateCommand(
            string databaseTable,
            SpannerParameterCollection insertUpdateColumns = null) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateInsertOrUpdateTextBuilder(databaseTable), this,
            null, insertUpdateColumns);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to select rows using a SQL query statement.
        /// This method is thread safe.
        /// </summary>
        /// <param name="sqlQueryStatement">
        /// A full SQL query statement that may optionally have
        /// replacement parameters. Must not be null.
        /// </param>
        /// <param name="selectParameters">
        /// Optionally supplied set of <see cref="SpannerParameter" />
        /// that correspond to the parameters used in the SQL query. May be null.
        /// </param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateSelectCommand(
            string sqlQueryStatement,
            SpannerParameterCollection selectParameters = null) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateSelectTextBuilder(sqlQueryStatement), this, null,
            selectParameters);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> from a <see cref="CommandPartition"/>.
        /// The newly created command will execute on a subset of data defined by the <see cref="CommandPartition.PartitionId"/>
        /// </summary>
        /// <param name="partition">
        /// Information that represents a command to execute against a subset of data.
        /// </param>
        /// <param name="transaction">The <see cref="SpannerTransaction"/> used when
        /// creating the <see cref="CommandPartition"/>.  See <see cref="SpannerConnection.BeginReadOnlyTransaction(TransactionId)"/>.</param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateCommandWithPartition(CommandPartition partition, SpannerTransaction transaction)
            => new SpannerCommand(this, transaction, partition);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to update rows in a Spanner database table.
        /// This method is thread safe.
        /// </summary>
        /// <param name="databaseTable">The name of the table to update rows. Must not be null.</param>
        /// <param name="updateColumns">
        /// A collection of <see cref="SpannerParameter" />
        /// where each instance represents a column in the Spanner database table being set.
        /// Primary keys of the rows to be updated must also be included.
        /// May be null.
        /// </param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateUpdateCommand(
            string databaseTable,
            SpannerParameterCollection updateColumns = null) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateUpdateTextBuilder(databaseTable), this, null,
            updateColumns);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to execute a DDL (CREATE/DROP TABLE, etc) statement.
        /// This method is thread safe.
        /// </summary>
        /// <param name="ddlStatement">The DDL statement (eg 'CREATE TABLE MYTABLE ...').  Must not be null.</param>
        /// <param name="extraDdlStatements">An optional set of additional DDL statements to execute after
        /// the first statement.  Extra Ddl statements cannot be used to create additional databases.</param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateDdlCommand(
            string ddlStatement, params string[] extraDdlStatements) => new SpannerCommand(
            SpannerCommandTextBuilder.CreateDdlTextBuilder(ddlStatement, extraDdlStatements), this);

        /// <summary>
        /// Creates a new <see cref="SpannerCommand" /> to execute a general DML (UPDATE, INSERT, DELETE) statement.
        /// This method is thread safe.
        /// </summary>
        /// <remarks>
        /// To insert, update, delete or "insert or update" a single row, the operation-specific methods
        /// (<see cref="CreateUpdateCommand(string, SpannerParameterCollection)"/> etc) are preferred as they are more efficient.
        /// This method is more appropriate for general-purpose DML which can perform modifications based on query results.
        /// </remarks>
        /// <param name="dmlStatement">The DML statement (eg 'DELETE FROM MYTABLE WHERE ...').  Must not be null.</param>
        /// <param name="dmlParameters">
        /// Optionally supplied set of <see cref="SpannerParameter" />
        /// that correspond to the parameters used in the SQL query. May be null.
        /// </param>
        /// <returns>A configured <see cref="SpannerCommand" /></returns>
        public SpannerCommand CreateDmlCommand(
            string dmlStatement, SpannerParameterCollection dmlParameters = null) =>
            new SpannerCommand(SpannerCommandTextBuilder.CreateDmlTextBuilder(dmlStatement), this, null, dmlParameters);

        /// <inheritdoc />
        public override void Open()
        {
            if (IsOpen)
            {
                return;
            }
#if NETSTANDARD1_5
            Func<Task> taskRunner = () => OpenAsyncImpl(CancellationToken.None);
#else
            // Important: capture the transaction on *this* thread.
            Transaction transaction = Transaction.Current;
            Func<Task> taskRunner = () => OpenAsyncImpl(transaction, CancellationToken.None);
#endif

            if (!Task.Run(taskRunner).Wait(TimeSpan.FromSeconds(SpannerOptions.Instance.Timeout)))
            {
                throw new SpannerException(ErrorCode.DeadlineExceeded, "Timed out opening connection");
            }
        }

        /// <inheritdoc />
        public override Task OpenAsync(CancellationToken cancellationToken) =>
#if NETSTANDARD1_5
            OpenAsyncImpl(cancellationToken);
#else
            OpenAsyncImpl(Transaction.Current, cancellationToken);
#endif

        private Task OpenAsyncImpl(
#if !NETSTANDARD1_5
            Transaction currentTransaction,
#endif
            CancellationToken cancellationToken)
        {
            return ExecuteHelper.WithErrorTranslationAndProfiling(
                async () =>
                {
                    if (string.IsNullOrEmpty(_connectionStringBuilder?.SpannerDatabase))
                    {
                        Logger.Info(() => "No database was defined. Therefore OpenAsync did not establish a session.");
                        _state = ConnectionState.Open;
                        return;
                    }
                    if (IsOpen)
                    {
                        return;
                    }

                    lock (_sync)
                    {
                        if (IsOpen)
                        {
                            return;
                        }

                        if (_state == ConnectionState.Connecting)
                        {
                            throw new InvalidOperationException("The SpannerConnection is already being opened.");
                        }

                        _state = ConnectionState.Connecting;
                    }
                    OnStateChange(new StateChangeEventArgs(ConnectionState.Closed, ConnectionState.Connecting));
                    SpannerClient localClient = null;
                    try
                    {
                        localClient = await ClientPool.Default
                            .AcquireClientAsync(_connectionStringBuilder)
                            .ConfigureAwait(false);
                        _sharedSession = await SessionPool.Default.CreateSessionFromPoolAsync(
                                localClient, _connectionStringBuilder.Project,
                                _connectionStringBuilder.SpannerInstance,
                                _connectionStringBuilder.SpannerDatabase,
                                s_defaultTransactionOptions,
                                cancellationToken)
                            .ConfigureAwait(false);
                        _sessionRefCount = 0;
                        _keepAliveCancellation = new CancellationTokenSource();
                        _keepAliveTask = Task.Run(
                            () => KeepAlive(_keepAliveCancellation.Token),
                            _keepAliveCancellation.Token);
                    }
                    finally
                    {
                        _state = _sharedSession != null ? ConnectionState.Open : ConnectionState.Broken;
                        if (IsOpen)
                        {
                            SpannerClient = localClient;
                        }
                        else
                        {
                            ReleaseClient(localClient, _connectionStringBuilder);
                        }
#if !NETSTANDARD1_5
                        if (IsOpen && currentTransaction != null)
                        {
                            EnlistTransaction(currentTransaction);
                        }
#endif
                        OnStateChange(new StateChangeEventArgs(ConnectionState.Connecting, _state));
                    }
                }, "SpannerConnection.OpenAsync", Logger);
        }

        /// <inheritdoc />
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            if (isolationLevel != IsolationLevel.Unspecified
                && isolationLevel != IsolationLevel.Serializable)
            {
                throw new NotSupportedException(
                    $"Cloud Spanner only supports isolation levels {IsolationLevel.Serializable} and {IsolationLevel.Unspecified}.");
            }
            return BeginTransactionAsync().ResultWithUnwrappedExceptions();
        }

        /// <inheritdoc />
        protected override DbCommand CreateDbCommand() => new SpannerCommand() { Connection = this };

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (IsOpen)
            {
                Close();
            }
        }

        internal ISpannerTransaction GetDefaultTransaction()
        {
#if !NETSTANDARD1_5
            if (_volatileResourceManager != null)
            {
                return _volatileResourceManager;
            }
#endif
            return new EphemeralTransaction(this, Logger);
        }

        /// <summary>
        /// The current connection string builder; this may be null. Callers must not mutate the returned builder.
        /// </summary>
        internal SpannerConnectionStringBuilder SpannerConnectionStringBuilder => _connectionStringBuilder;

        internal void ReleaseSession(Session session, SpannerClient client)
        {
            Session sessionToRelease = null;
            lock (_sync)
            {
                if (ReferenceEquals(session, _sharedSession))
                {
                    Interlocked.Decrement(ref _sessionRefCount);
                    if (!IsOpen && _sessionRefCount == 0)
                    {
                        //delayed session release due to a synchronous close
                        sessionToRelease = _sharedSession;
                        _sharedSession = null;
                    }
                }
                else if (_sharedSession == null && IsOpen)
                {
                    //someone stole the shared session, lets put it back for reserved use.
                    _sharedSession = session;
                    //we'll also ensure the refcnt is zero, but this should already be true.
                    _sessionRefCount = 0;
                }
                else if (!_staleSessions.Contains(session.Name))
                {
                    if (SessionPool.Default.IsSessionExpired(session))
                    {
                        // _staleSessions ensures we only release bad sessions once because
                        // we are clobbering its ref count that it would otherwise use for this purpose.
                        _staleSessions.Add(session.Name);
                    }
                    sessionToRelease = session;
                }
            }
            if (sessionToRelease != null)
            {
                SessionPool.Default.ReleaseToPool(client, sessionToRelease);
            }
        }

        private Task<Session> AllocateSession(TransactionOptions options, CancellationToken cancellationToken)
        {
            return ExecuteHelper.WithErrorTranslationAndProfiling(
                async () =>
                {
                    AssertOpen("execute command");
                    Task<Session> result;
                    // If the shared session gets used for anything but a write, we need
                    // to clear out the transaction state, because this will happen implicitly
                    // by spanner when a read is done without the active transaction.
                    // For other cases, the transaction state is always cleared as soon as its
                    // placed back into the session pool.
                    var sharedSessionReadOnlyUse = false;

                    lock (_sync)
                    {
                        //first we ensure _sharedSession hasn't been invalidated/expired.
                        if (SessionPool.Default.IsSessionExpired(_sharedSession))
                        {
                            _sharedSession = null;
                            _sessionRefCount = 0; //any existing references will fail out and release them.
                        }

                        //we will use _sharedSession if
                        //a) options == s_defaultTransactionOptions && _sharedSession != null
                        //    (we increment refcnt) and return _sharedSession
                        //b) options == s_defaultTransactionOptions && _sharedSession == null
                        //    we create a new shared session and return it.
                        //c) options != s_defaultTransactionOptions && _sharedSession != null && refcnt == 0
                        //    we steal _sharedSession to return it, set _sharedSession = null
                        //d) options != s_defaultTransactionOptions && (_sharedSession == null || refcnt > 0)
                        //    we grab a new session from the pool.
                        bool isSharedReadonlyTx = Equals(options, s_defaultTransactionOptions);
                        if (isSharedReadonlyTx && _sharedSession != null)
                        {
                            result = Task.FromResult(_sharedSession);
                            Interlocked.Increment(ref _sessionRefCount);
                        }
                        else if (isSharedReadonlyTx && _sharedSession == null)
                        {
                            sharedSessionReadOnlyUse = true;
                            // If we enter this code path, it means a transaction has stolen our shared session.
                            // This is ok, we'll just create another. But need to be very careful about concurrency
                            // as compared to OpenAsync (which is documented as not threadsafe).
                            // To make this threadsafe, we store the creation task as a member and let other callers
                            // hook onto the first creation task.
                            if (_sharedSessionAllocator == null)
                            {
                                _sharedSessionAllocator = SessionPool.Default.CreateSessionFromPoolAsync(
                                    SpannerClient, _connectionStringBuilder.Project, _connectionStringBuilder.SpannerInstance,
                                    _connectionStringBuilder.SpannerDatabase, options, CancellationToken.None);
                                result = Task.Run(
                                    async () =>
                                    {
                                        var newSession = await _sharedSessionAllocator.ConfigureAwait(false);
                                        Interlocked.Increment(ref _sessionRefCount);
                                        // we need to make sure the refcnt is >0 before setting _sharedSession.
                                        // or else the session could again be stolen from us by another transaction.
                                        _sharedSession = newSession;
                                        return _sharedSession;
                                    }, CancellationToken.None);
                            }
                            else
                            {
                                result = Task.Run(
                                    async () =>
                                    {
                                        await _sharedSessionAllocator.ConfigureAwait(false);
                                        Interlocked.Increment(ref _sessionRefCount);
                                        return _sharedSession;
                                    }, CancellationToken.None);
                            }
                        }
                        else if (!isSharedReadonlyTx && _sharedSession != null && _sessionRefCount == 0)
                        {
                            // In this case, someone has called OpenAsync() followed by BeginTransactionAsync().
                            // While we'd prefer them to just call BeginTransaction (so we can allocate a session
                            // with the appropriate transaction semantics straight from the pool), this is still allowed
                            // and we shouldn't create *two* sessions here for the case where they only ever use
                            // this connection for a single transaction.
                            // So, we'll steal the shared precreated session and re-allocate it to the transaction.
                            // If the user later does reads outside of a transaction, it will force create a new session.
                            result = Task.FromResult(_sharedSession);
                            _sessionRefCount = 0;
                            _sharedSession = null;
                            _sharedSessionAllocator = null;
                        }
                        else
                        {
                            //In this case, its a transaction and the shared session is also in use.
                            //so, we'll just create a new session (from the pool).
                            result = SessionPool.Default.CreateSessionFromPoolAsync(
                                SpannerClient, _connectionStringBuilder.Project, _connectionStringBuilder.SpannerInstance,
                                _connectionStringBuilder.SpannerDatabase, options, cancellationToken);
                        }
                    }

                    var session = await result.ConfigureAwait(false);
                    if (sharedSessionReadOnlyUse)
                    {
                        await TransactionPool.RemoveSessionAsync(session).ConfigureAwait(false);
                    }

                    return session;
                }, "SpannerConnection.AllocateSession", Logger);
        }

        private void AssertClosed(string message)
        {
            if (!IsClosed)
            {
                throw new InvalidOperationException("The connection must be closed. Failed to " + message);
            }
        }

        private void AssertOpen(string message)
        {
            if (!IsOpen)
            {
                throw new InvalidOperationException("The connection must be open. Failed to " + message);
            }
        }

        internal Task<SingleUseTransaction> BeginSingleUseTransactionAsync(
            TimestampBound targetReadTimestamp,
            CancellationToken cancellationToken)
        {
            var options = CreateReadOnlyTransactionOptions(targetReadTimestamp);
            return ExecuteHelper.WithErrorTranslationAndProfiling(
                async () =>
                {
                    using (var sessionHolder = await SessionHolder.Allocate(
                            this,
                            options, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        await TransactionPool.RemoveSessionAsync(sessionHolder.Session).ConfigureAwait(false);
                        return new SingleUseTransaction(this, sessionHolder.TakeOwnership(), options);
                    }
                }, "SpannerConnection.BeginSingleUseTransaction", Logger);
        }

        internal Task<PartitionedUpdateTransaction> BeginPartitionedUpdateTransactionAsync(CancellationToken cancellationToken) =>
            ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
            {
                // Note that this bypasses the transaction pool. I *believe* that's the desirable behaviour here.
                var options = new TransactionOptions { PartitionedDml = new TransactionOptions.Types.PartitionedDml() };
                using (var sessionHolder = await SessionHolder.Allocate(
                        this,
                        options, cancellationToken)
                    .ConfigureAwait(false))
                {
                    var transaction = await SpannerClient
                        .BeginTransactionAsync(sessionHolder.Session.SessionName, options)
                        .ConfigureAwait(false);
                    return new PartitionedUpdateTransaction(this, sessionHolder.TakeOwnership(), transaction);
                }
            }, "SpannerConnection.BeginPartitionedUpdateTransaction", Logger);

        /// <summary>
        /// Helper method for common code to execute DML via a ReliableStreamReader.
        /// </summary>
        internal Task<long> ExecuteDmlAsync(Session session, ExecuteSqlRequest request, CancellationToken cancellationToken, int timeoutSeconds, string callerType) =>
            ExecuteHelper.WithErrorTranslationAndProfiling(async () =>
            {
                var callSettings = SpannerClient.Settings.ExecuteSqlSettings
                    .WithExpiration(SpannerClient.Settings.ConvertTimeoutToExpiration(timeoutSeconds));

                request.Session = session.Name;
                ResultSet resultSet = await SpannerClient.ExecuteSqlAsync(request, callSettings).ConfigureAwait(false);
                var stats = resultSet.Stats;
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
            }, $"{callerType}.ExecuteDml", Logger);

        private Task<SpannerTransaction> BeginTransactionImplAsync(
            TransactionOptions transactionOptions,
            TransactionMode transactionMode,
            CancellationToken cancellationToken,
            TimestampBound targetReadTimestamp = null)
        {
            return ExecuteHelper.WithErrorTranslationAndProfiling(
                async () =>
                {
                    using (var sessionHolder = await SessionHolder.Allocate(this, transactionOptions, cancellationToken)
                        .ConfigureAwait(false))
                    {
                        var transaction = await TransactionPool
                            .BeginPooledTransactionAsync(SpannerClient, sessionHolder.Session, transactionOptions)
                            .ConfigureAwait(false);
                        return new SpannerTransaction(
                            this, transactionMode, sessionHolder.TakeOwnership(),
                            transaction, targetReadTimestamp);
                    }
                }, "SpannerConnection.BeginTransaction", Logger);
        }

        private Task KeepAlive(CancellationToken cancellationToken)
        {
            var request = new ExecuteSqlRequest
            {
                Sql = "SELECT 1"
            };

            var task = Task.Delay(SpannerOptions.KeepAliveInterval, cancellationToken);
            var loopTask = task.ContinueWith(
                async t =>
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        //ping and reschedule.
                        var sharedSession = _sharedSession;
                        if (sharedSession != null)
                        {
                            try
                            {
                                request.SessionAsSessionName = sharedSession.SessionName;
                                await SpannerClient.ExecuteSqlAsync(request).WithSessionChecking(() => sharedSession)
                                    .ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                Logger.Warn(() => $"Exception attempting to keep session alive: {e}");
                            }
                        }

                        _keepAliveTask = Task.Run(
                            () => KeepAlive(_keepAliveCancellation.Token),
                            _keepAliveCancellation.Token);
                    }
                }, cancellationToken);

            return loopTask;
        }

        private void TrySetNewConnectionInfo(SpannerConnectionStringBuilder newBuilder)
        {
            AssertClosed("change connection information.");
            // We will never allow our internal connectionstringbuilder to be touched from the outside, so its cloned.
            _connectionStringBuilder = newBuilder.Clone();
        }

        /// <summary>
        /// SessionHolder is a helper class to ensure that sessions do not leak and are properly recycled when
        /// an error occurs.
        /// </summary>
        internal sealed class SessionHolder : IDisposable
        {
            private readonly SpannerConnection _connection;
            private Session _session;

            public Session Session => _session;

            private SessionHolder(SpannerConnection connection, Session session)
            {
                _connection = connection;
                _session = session;
            }

            public void Dispose()
            {
                var session = Interlocked.Exchange(ref _session, null);
                if (session != null)
                {
                    // TODO: Check there's always a client at this point.
                    _connection.ReleaseSession(session, _connection.SpannerClient);
                }
            }

            public static Task<SessionHolder> Allocate(SpannerConnection owner, CancellationToken cancellationToken) =>
                Allocate(owner, s_defaultTransactionOptions, cancellationToken);

            public static async Task<SessionHolder> Allocate(SpannerConnection owner, TransactionOptions options, CancellationToken cancellationToken)
            {
                var session = await owner.AllocateSession(options, cancellationToken).ConfigureAwait(false);
                return new SessionHolder(owner, session);
            }

            public Session TakeOwnership() => Interlocked.Exchange(ref _session, null);
        }

#if !NETSTANDARD1_5

        /// <summary>
        /// Call OpenAsReadOnly within a <see cref="System.Transactions.TransactionScope" /> to open the connection
        /// with a read-only transaction with the given <see cref="TimestampBound" /> settings
        /// </summary>
        /// <param name="timestampBound">Specifies the timestamp or maximum staleness of a read operation. May be null.</param>
        public void OpenAsReadOnly(TimestampBound timestampBound = null)
        {
            // Note: This has to be checked on the current thread, which is why we don't just use Task.Run
            // and delegate to OpenAsReadOnlyAsync
            var transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called within a TransactionScope.");
            }
            if (!EnlistInTransaction)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called with ${nameof(EnlistInTransaction)} set to true.");
            }
            _timestampBound = timestampBound ?? TimestampBound.Strong;
            OpenAsyncImpl(transaction, CancellationToken.None).WaitWithUnwrappedExceptions(); ;
        }

        /// <summary>
        /// If this connection is being opened within a <see cref="System.Transactions.TransactionScope" />, this
        /// will connect to an existing transaction identified by <paramref name="transactionId"/>.
        /// </summary>
        /// <param name="transactionId">The <see cref="TransactionId"/> representing an active readonly <see cref="SpannerTransaction"/>.</param>
        public void OpenAsReadOnly(TransactionId transactionId)
        {
            GaxPreconditions.CheckNotNull(transactionId, nameof(transactionId));
            var transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called within a TransactionScope.");
            }
            if (!EnlistInTransaction)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called with ${nameof(EnlistInTransaction)} set to true.");
            }
            _transactionId = transactionId;
            OpenAsyncImpl(transaction, CancellationToken.None).WaitWithUnwrappedExceptions();
        }

        /// <summary>
        /// If this connection is being opened within a <see cref="System.Transactions.TransactionScope" />, this forces
        /// the created Cloud Spanner transaction to be a read-only transaction with the given
        /// <see cref="TimestampBound" /> settings.
        /// </summary>
        /// <param name="timestampBound">Specifies the timestamp or maximum staleness of a read operation. May be null.</param>
        /// <param name="cancellationToken">An optional token for canceling the call.</param>
        public Task OpenAsReadOnlyAsync(TimestampBound timestampBound = null, CancellationToken cancellationToken = default)
        {
            var transaction = Transaction.Current;
            if (transaction == null)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called within a TransactionScope.");
            }
            if (!EnlistInTransaction)
            {
                throw new InvalidOperationException($"{nameof(OpenAsReadOnlyAsync)} should only be called with ${nameof(EnlistInTransaction)} set to true.");
            }
            _timestampBound = timestampBound ?? TimestampBound.Strong;
            return OpenAsyncImpl(transaction, cancellationToken);
        }

        /// <summary>
        /// Gets or Sets whether to participate in the active <see cref="System.Transactions.TransactionScope" />
        /// </summary>
        public bool EnlistInTransaction { get; set; } = true;

        /// <inheritdoc />
        public override void EnlistTransaction(Transaction transaction)
        {
            if (!EnlistInTransaction)
            {
                return;
            }
            if (_volatileResourceManager != null)
            {
                throw new InvalidOperationException("This connection is already enlisted to a transaction.");
            }
            _volatileResourceManager = new VolatileResourceManager(this, _timestampBound, _transactionId);
            transaction.EnlistVolatile(_volatileResourceManager, System.Transactions.EnlistmentOptions.None);
        }

        /// <inheritdoc />
        protected override DbProviderFactory DbProviderFactory => SpannerProviderFactory.Instance;
#endif
    }
}