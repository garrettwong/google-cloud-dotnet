﻿// Copyright 2019 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Api.Gax;
using Google.Cloud.Spanner.V1;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Cloud.Spanner.Data
{
    internal sealed class RetriableTransaction
    {
        private readonly SpannerConnection _connection;
        private readonly IClock _clock;
        private readonly IScheduler _scheduler;
        private readonly RetriableTransactionOptions _options;

        internal RetriableTransaction(SpannerConnection connection, IClock clock, IScheduler scheduler, RetriableTransactionOptions options = null)
        {
            _connection = GaxPreconditions.CheckNotNull(connection, nameof(connection));
            _clock = GaxPreconditions.CheckNotNull(clock, nameof(clock));
            _scheduler = GaxPreconditions.CheckNotNull(scheduler, nameof(scheduler));
            _options = options ?? RetriableTransactionOptions.CreateDefault();
        }

        internal async Task<TResult> RunAsync<TResult>(Func<SpannerTransaction, Task<TResult>> asyncWork, CancellationToken cancellationToken = default)
        {
            GaxPreconditions.CheckNotNull(asyncWork, nameof(asyncWork));

            // Session will be initialized and subsequently modified by CommitAttempt.
            PooledSession session = null;
            try
            {
                return await ExecuteWithRetryAsync(CommitAttempt, cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                session?.Dispose();
            }

            async Task<TResult> CommitAttempt()
            {
                return await ExecuteHelper.WithErrorTranslationAndProfiling(
                    async () =>
                    {
                        SpannerTransaction transaction = null;

                        try
                        {
                            session = await (session?.WithFreshTransactionOrNewAsync(SpannerConnection.s_readWriteTransactionOptions, cancellationToken) ?? _connection.AcquireReadWriteSessionAsync(cancellationToken)).ConfigureAwait(false);
                            transaction = new SpannerTransaction(_connection, TransactionMode.ReadWrite, session, null);

                            TResult result = await asyncWork(transaction).ConfigureAwait(false);
                            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);

                            return result;
                        }
                        // We only catch to attempt a rollback, and that is possible if we have a transaction already.
                        // If the exception was thrown when refreshing the session the we don't have a transaction yet.
                        catch when (transaction != null)
                        {
                            try
                            {
                                await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
                            }
                            catch (Exception e)
                            {
                                // If the commit failed, calling Rollback will fail as well.
                                // We don't want that or any other rollback exception to propagate,
                                // it will not trigger the retry
                                _connection.Logger.Warn("A rollback attempt failed on RetriableTransaction.RunAsync.CommitAttempt", e);
                            }

                            // Throw, the retry helper will know when to retry.
                            throw;
                        }
                        finally
                        {
                            if (transaction != null)
                            {
                                // Let's make sure that the associated session is not released to the pool
                                // because we'll be attempting to get a fresh transaction for this same session first.
                                // If that fails will attempt a new session acquisition.
                                // This session will be disposed of by the pool if it can't be refreshed or by the RunAsync method
                                // if we are not retrying anymore.
                                transaction.DisposeBehavior = DisposeBehavior.Detach;
                                transaction.Dispose();
                            }
                        }
                    }, "RetriableTransaction.RunAsync.CommitAttempt", _connection.Logger).ConfigureAwait(false);
            }
        }

        internal async Task<TResult> ExecuteWithRetryAsync<TResult>(Func<Task<TResult>> fn, CancellationToken cancellationToken)
        {
            DateTime? overallDeadline = _options.CalculateDeadline(_clock);
            TimeSpan retryDelay = _options.InitialDelay;
            while (true)
            {
                try
                {
                    var result = await fn().ConfigureAwait(false);
                    return result;
                }
                catch (SpannerException e) when (e.IsRetryable && !e.SessionExpired)
                {
                    // If there's a recommended retry delay specified on the exception
                    // we should respect it.
                    retryDelay = e.RecommendedRetryDelay ?? retryDelay;

                    TimeSpan actualDelay = _options.Jitter(retryDelay);
                    DateTime expectedRetryTime = _clock.GetCurrentDateTimeUtc() + actualDelay;
                    if (expectedRetryTime > overallDeadline)
                    {
                        throw;
                    }
                    await _scheduler.Delay(actualDelay, cancellationToken).ConfigureAwait(false);
                    retryDelay = _options.NextDelay(retryDelay);
                }
            }
        }
    }
}
