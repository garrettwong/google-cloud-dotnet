﻿// Copyright 2016 Google Inc. All Rights Reserved.
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

using Grpc.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using static Google.Cloud.Datastore.V1.QueryResultBatch.Types;

namespace Google.Cloud.Datastore.V1.IntegrationTests
{
    [Collection(nameof(DatastoreFixture))]
    public class DatastoreDbTest
    {
        private readonly DatastoreFixture _fixture;

        public DatastoreDbTest(DatastoreFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Lookup_FullPartition()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("test");
            var entity = new Entity
            {
                Key = keyFactory.CreateIncompleteKey(),
                ["foo"] = "bar"
            };
            var key = db.Insert(entity);

            var result = db.Lookup(key);
            Assert.NotNull(result);
            Assert.Equal("bar", (string)entity["foo"]);
        }

        [Fact]
        public void Lookup_NamespaceOnly()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("test");
            var entity = new Entity
            {
                Key = keyFactory.CreateIncompleteKey(),
                ["foo"] = "bar"
            };
            var insertedKey = db.Insert(entity);

            var lookupKey = new Key { PartitionId = new PartitionId { NamespaceId = _fixture.NamespaceId }, Path = { insertedKey.Path } };
            var result = db.Lookup(lookupKey);
            Assert.NotNull(result);
            Assert.Equal("bar", (string)entity["foo"]);
        }

        [Fact]
        public async Task Lookup_NoPartition()
        {
            // Deliberately in the empty namespace, which won't be cleaned up automatically - hence the db.Delete call later.
            var db = _fixture.CreateDatastoreDb(namespaceId: "");
            var keyFactory = db.CreateKeyFactory("test");
            var entity = new Entity
            {
                Key = keyFactory.CreateIncompleteKey(),
                ["foo"] = "bar"
            };
            var insertedKey = db.Insert(entity);
            try
            {
                var lookupKey = new Key { Path = { insertedKey.Path } };
                var result = db.Lookup(lookupKey);
                Assert.NotNull(result);
                Assert.Equal("bar", (string)entity["foo"]);

                // And the same lookup asynchronously...
                Assert.Equal(result, await db.LookupAsync(lookupKey));
            }
            finally
            {
                db.Delete(insertedKey);
            }
        }

        [Fact]
        public async Task Lookup()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("lookup_test");
            var entity = new Entity { Key = keyFactory.CreateKey("x"), ["description"] = "predefined_key" };
            db.Insert(entity);

            // Test both sync and async lookup
            Assert.Equal(entity, db.Lookup(entity.Key));
            Assert.Equal(entity, await db.LookupAsync(entity.Key));
        }

        [Fact]
        public void RunQuery_NoResults()
        {
            var db = _fixture.CreateDatastoreDb();
            var query = db.RunQueryLazily(new Query("absent"));
            // Each of the checks below will run the query again, as the query is only lazily
            // evaluated.
            Assert.Equal(0, query.Count());
            var singleResponse = query.AsResponses().Single();
            Assert.Equal(MoreResultsType.NoMoreResults, singleResponse.Batch.MoreResults);
        }

        [Fact]
        public void SyncQueries()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("syncqueries");
            using (var transaction = db.BeginTransaction())
            {
                var entities = Enumerable.Range(0, 5)
                    .Select(x => new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = x })
                    .ToList();
                transaction.Insert(entities);
                transaction.Commit();
            }

            var query = new Query("syncqueries") { Filter = Filter.LessThan("x", 3) };
            var gql = new GqlQuery { QueryString = "SELECT * FROM syncqueries WHERE x < 3", AllowLiterals = true };

            _fixture.RetryQuery(() =>
            {
                ValidateQueryResults(db.RunQuery(gql).Entities);
                ValidateQueryResults(db.RunQuery(query).Entities);
                ValidateQueryResults(db.RunQueryLazily(query));
                ValidateQueryResults(db.RunQueryLazily(gql));
            });
        }

        [Fact]
        public async Task AsyncQueries()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("asyncqueries");
            using (var transaction = await db.BeginTransactionAsync())
            {
                var entities = Enumerable.Range(0, 5)
                    .Select(x => new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = x })
                    .ToList();
                transaction.Insert(entities);
                await transaction.CommitAsync();
            }

            var query = new Query("asyncqueries") { Filter = Filter.LessThan("x", 3) };
            var gql = new GqlQuery { QueryString = "SELECT * FROM asyncqueries WHERE x < 3", AllowLiterals = true };

            await _fixture.RetryQueryAsync(async () =>
            {
                ValidateQueryResults((await db.RunQueryAsync(gql)).Entities);
                ValidateQueryResults((await db.RunQueryAsync(query)).Entities);
                ValidateQueryResults(db.RunQueryLazilyAsync(query).ToEnumerable());
                ValidateQueryResults(db.RunQueryLazilyAsync(gql).ToEnumerable());
            });
        }

        [Fact]
        public void BeginTransaction_ReadOnly()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("readonly_test");
            var options = TransactionOptions.CreateReadOnly();
            using (var transaction = db.BeginTransaction(options))
            {
                transaction.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = 3 });
                var exception = Assert.Throws<RpcException>(() => transaction.Commit());
                Assert.Equal(StatusCode.InvalidArgument, exception.Status.StatusCode);
            }
        }

        [Fact]
        public async Task BeginTransactionAsync_ReadOnly()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("readonly_test");
            var options = TransactionOptions.CreateReadOnly();
            using (var transaction = await db.BeginTransactionAsync(options))
            {
                transaction.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = 3 });
                var exception = await Assert.ThrowsAsync<RpcException>(() => transaction.CommitAsync());
                Assert.Equal(StatusCode.InvalidArgument, exception.Status.StatusCode);
            }
        }

        [Fact]
        public void BeginTransaction_ReadWrite()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("readonly_test");
            var options = TransactionOptions.CreateReadWrite();
            using (var transaction = db.BeginTransaction(options))
            {
                transaction.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = 3 });
                transaction.Commit();
            }
        }

        [Fact]
        public async Task BeginTransactionAsync_ReadWrite()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("readonly_test");
            var options = TransactionOptions.CreateReadWrite();
            using (var transaction = await db.BeginTransactionAsync(options))
            {
                transaction.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["x"] = 3 });
                await transaction.CommitAsync();
            }
        }

        // TODO: Tests for retry-based transaction options

        private void ValidateQueryResults(IEnumerable<Entity> entities)
        {
            Assert.Equal(new[] { 0, 1, 2 }, entities.Select(e => (int)e["x"]).OrderBy(x => x));
        }

        [Fact]
        public void Insert_ResultKeys()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("insert_test");
            var entities = new[]
            {
                new Entity { Key = keyFactory.CreateKey("x"), ["description"] = "predefined_key" },
                new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "incomplete_key" }
            };

            var keys = db.Insert(entities);
            Assert.Null(keys[0]); // Insert with predefined key 
            Assert.NotNull(keys[1]); // Insert with incomplete key

            // Inserted key is propagated into entity
            Assert.Equal(keys[1], entities[1].Key);

            var fetchedEntity = db.Lookup(keys[1]);
            Assert.Equal("incomplete_key", fetchedEntity["description"]);
        }

        [Fact]
        public void Upsert_ResultKeys()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("upsert_test");
            var insertedKey = db.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });

            var revisedEntity = new Entity { Key = insertedKey, ["description"] = "changed" };
            var newEntity1 = new Entity { Key = keyFactory.CreateKey("x"), ["description"] = "predefined_key" };
            var newEntity2 = new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "incomplete_key" };

            var keys = db.Upsert(revisedEntity, newEntity1, newEntity2);
            Assert.Null(keys[0]); // Update
            Assert.Null(keys[1]); // Insert with predefined key 
            Assert.NotNull(keys[2]); // Insert with incomplete key

            // Inserted key is propagated into entity
            Assert.Equal(keys[2], newEntity2.Key);

            var fetchedEntity = db.Lookup(keys[2]);
            Assert.Equal("incomplete_key", fetchedEntity["description"]);
        }

        [Fact]
        public async Task InsertAsync_ResultKeys()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("insertasync_test");
            var entities = new[]
            {
                new Entity { Key = keyFactory.CreateKey("x"), ["description"] = "predefined_key" },
                new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "incomplete_key" }
            };

            var keys = await db.InsertAsync(entities);
            Assert.Null(keys[0]); // Insert with predefined key 
            Assert.NotNull(keys[1]); // Insert with incomplete key

            // Inserted key is propagated into entity
            Assert.Equal(keys[1], entities[1].Key);

            var fetchedEntity = await db.LookupAsync(keys[1]);
            Assert.Equal("incomplete_key", fetchedEntity["description"]);
        }

        [Fact]
        public async Task UpsertAsync_ResultKeys()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("upsertasync_test");
            var insertedKey = db.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });

            var revisedEntity = new Entity { Key = insertedKey, ["description"] = "changed" };
            var newEntity1 = new Entity { Key = keyFactory.CreateKey("x"), ["description"] = "predefined_key" };
            var newEntity2 = new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "incomplete_key" };

            var keys = await db.UpsertAsync(revisedEntity, newEntity1, newEntity2);
            Assert.Null(keys[0]); // Update
            Assert.Null(keys[1]); // Insert with predefined key 
            Assert.NotNull(keys[2]); // Insert with incomplete key

            // Inserted key is propagated into entity
            Assert.Equal(keys[2], newEntity2.Key);

            var fetchedEntity = await db.LookupAsync(keys[2]);
            Assert.Equal("incomplete_key", fetchedEntity["description"]);
        }

        [Fact]
        public void Update()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("update_test");

            var insertedKey = db.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });

            var fetched = db.Lookup(insertedKey);
            Assert.Equal("original", (string) fetched["description"]);

            db.Update(new Entity { Key = fetched.Key, ["description"] = "updated" });
            fetched = db.Lookup(insertedKey);
            Assert.Equal("updated", (string) fetched["description"]);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("update_test");

            var insertedKey = await db.InsertAsync(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });

            var fetched = await db.LookupAsync(insertedKey);
            Assert.Equal("original", (string)fetched["description"]);

            await db.UpdateAsync(new Entity { Key = fetched.Key, ["description"] = "updated" });
            fetched = await db.LookupAsync(insertedKey);
            Assert.Equal("updated", (string)fetched["description"]);
        }

        [Fact]
        public void Delete()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("update_test");

            var insertedKey = db.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });
            Assert.NotNull(db.Lookup(insertedKey));
            db.Delete(insertedKey);
            Assert.Null(db.Lookup(insertedKey));
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var db = _fixture.CreateDatastoreDb();
            var keyFactory = db.CreateKeyFactory("update_test");

            var insertedKey = await db.InsertAsync(new Entity { Key = keyFactory.CreateIncompleteKey(), ["description"] = "original" });
            Assert.NotNull(await db.LookupAsync(insertedKey));
            await db.DeleteAsync(insertedKey);
            Assert.Null(await db.LookupAsync(insertedKey));
        }

        [Fact]
        public void TimestampFromProjection()
        {
            var db = _fixture.CreateDatastoreDb();
            string kind = "projection_test";
            var keyFactory = db.CreateKeyFactory(kind);

            DateTime sampleTimestamp = new DateTime(2018, 12, 3, 15, 8, 32, 123, DateTimeKind.Utc);

            using (var transaction = db.BeginTransaction())
            {
                transaction.Insert(new Entity { Key = keyFactory.CreateIncompleteKey(), ["ts"] = sampleTimestamp, ["ignore"] = "ignore me" });
                transaction.Commit();
            }

            var query = new Query(kind)
            {
                Projection = { "ts" }
            };

            _fixture.RetryQuery(() =>
            {
                var results = db.RunQuery(query).Entities;
                Assert.Equal(1, results.Count);
                Assert.Equal(sampleTimestamp, results[0]["ts"].ToDateTimeFromProjection());
                Assert.Equal((DateTimeOffset)sampleTimestamp, results[0]["ts"].ToDateTimeOffsetFromProjection());
            });
        }
    }
}
