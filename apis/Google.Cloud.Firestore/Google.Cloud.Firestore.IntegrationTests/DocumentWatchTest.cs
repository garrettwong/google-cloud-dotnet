﻿// Copyright 2018, Google LLC
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

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Google.Cloud.Firestore.IntegrationTests
{
    [Collection(nameof(FirestoreFixture))]
    public class DocumentWatchTest
    {
        private readonly FirestoreFixture _fixture;

        public DocumentWatchTest(FirestoreFixture fixture) => _fixture = fixture;

        [Fact]
        public async Task Listen()
        {
            int? latestValue = null;
            var doc = _fixture.NonQueryCollection.Document();
            // Make sure we get each change separately.
            var semaphore = new SemaphoreSlim(0);

            var listener = doc.Listen(snapshot =>
            {
                latestValue = snapshot.Exists ? snapshot.GetValue<int?>("Value") : null;
                semaphore.Release();
            });
            // The document doesn't exist at first, and we should be notified of that initially.
            // If we create the document *immediately* before waiting for any change notification,
            // we sometimes "see" the non-existent state first, and sometimes don't.
            await ExpectChangeAsync(null);

            await doc.CreateAsync(new { Value = 10 });
            await ExpectChangeAsync(10);

            await doc.SetAsync(new { Value = 20 });
            await ExpectChangeAsync(20);

            await doc.DeleteAsync();
            await ExpectChangeAsync(null);

            await doc.CreateAsync(new { Value = 30 });
            await ExpectChangeAsync(30);

            await listener.StopAsync();

            async Task ExpectChangeAsync(int? expected)
            {
                // Wait for the callback to be called
                Assert.True(await semaphore.WaitAsync(1000));
                // Now check the value is as expected
                Assert.Equal(expected, latestValue);
            }
        }

        [Fact]
        public async Task LimitToLast()
        {
            var collection = _fixture.FirestoreDb.Collection(_fixture.CollectionPrefix + "-WatchLimitToLast");
            await collection.Document("doc1").CreateAsync(new { counter = 1 });
            await collection.Document("doc2").CreateAsync(new { counter = 2 });
            await collection.Document("doc3").CreateAsync(new { counter = 3 });
            var query = collection.OrderBy("counter").LimitToLast(2);
            var semaphore = new SemaphoreSlim(0);
            QuerySnapshot receivedSnapshot = null;
            var listener = query.Listen(snapshot =>
            {
                receivedSnapshot = snapshot;
                semaphore.Release();
            });
            // Wait up to 5 seconds for the query to work.
            Assert.True(await semaphore.WaitAsync(5000));
            await listener.StopAsync();
            var ids = receivedSnapshot.Documents.Select(doc => doc.Id).ToList();
            Assert.Equal(new[] { "doc2", "doc3" }, ids);
        }
    }
}
