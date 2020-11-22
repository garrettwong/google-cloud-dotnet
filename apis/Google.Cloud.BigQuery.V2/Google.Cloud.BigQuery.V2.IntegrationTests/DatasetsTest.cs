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

using Google.Apis.Bigquery.v2.Data;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Google.Cloud.BigQuery.V2.IntegrationTests
{
    [Collection(nameof(BigQueryFixture))]
    public class DatasetsTest
    {
        private readonly BigQueryFixture _fixture;

        public DatasetsTest(BigQueryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void GetDataset_CorrectETag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            // Fetch without specifying an ETag first
            var unconditional = client.GetDataset(_fixture.DatasetId);

            var options = new GetDatasetOptions { ETag = unconditional.Resource.ETag };
            client.GetDataset(_fixture.DatasetId, options);
        }

        [Fact]
        public void GetDataset_IncorrectETag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            // Fetch without specifying an ETag first
            var unconditional = client.GetDataset(_fixture.DatasetId);

            var options = new GetDatasetOptions { ETag = "wrong-" + unconditional.Resource.ETag };
            var exception = Assert.Throws<GoogleApiException>(() => client.GetDataset(_fixture.DatasetId, options));
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        [Fact]
        public async Task GetDatasetAsync_CorrectETag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            // Fetch without specifying an ETag first
            var unconditional = await client.GetDatasetAsync(_fixture.DatasetId);

            var options = new GetDatasetOptions { ETag = unconditional.Resource.ETag };
            await client.GetDatasetAsync(_fixture.DatasetId, options);
        }

        [Fact]
        public async Task GetDatasetAsync_IncorrectETag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            // Fetch without specifying an ETag first
            var unconditional = await client.GetDatasetAsync(_fixture.DatasetId);

            var options = new GetDatasetOptions { ETag = "wrong-" + unconditional.Resource.ETag };
            var exception = await Assert.ThrowsAsync<GoogleApiException>(() => client.GetDatasetAsync(_fixture.DatasetId, options));
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        // Regression test against issue where the etag returned on create wasn't the same as
        // for the initial fetch.
        [Fact]
        public void CreateDataset_InitialEtag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var created = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });
            var fetched = client.GetDataset(id);
            Assert.Equal(created.Resource.ETag, fetched.Resource.ETag);
        }

        [Fact]
        public void CreateDataset_WithCustomDefaultLocation()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId).WithDefaultLocation(Locations.Tokyo);
            var id = _fixture.CreateDatasetId();

            var created = client.CreateDataset(id);
            Assert.Equal(Locations.Tokyo, created.Resource.Location);
        }

        [Fact]
        public void CreateDataset_WithCustomLocation()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var created = client.CreateDataset(id, new Dataset { Location = Locations.Tokyo } );
            Assert.Equal(Locations.Tokyo, created.Resource.Location);
        }

        [Fact]
        public void CreateDataset_WithCustomLocationOverridingDefaultLocation()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId).WithDefaultLocation(Locations.Tokyo);
            var id = _fixture.CreateDatasetId();

            var created = client.CreateDataset(id, new Dataset { Location = Locations.EuropeanUnion });
            Assert.Equal(Locations.EuropeanUnion, created.Resource.Location);
        }

        [Fact]
        public void CreateDataset_NoDataset()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var created = client.CreateDataset(id);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public async Task CreateDatasetAsync_NoDataset()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var created = await client.CreateDatasetAsync(id);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public void CreateDataset_DatasetNoReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset();

            var created = client.CreateDataset(id, dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public async Task CreateDatasetAsync_DatasetNoReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset();

            var created = await client.CreateDatasetAsync(id, dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public void CreateDataset_DatasetSameReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset 
            { 
                DatasetReference = new DatasetReference 
                { 
                    ProjectId = _fixture.ProjectId,
                    DatasetId = id
                } 
            };

            var created = client.CreateDataset(id, dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public async Task CreateDatasetAsync_DatasetSameReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = id
                }
            };

            var created = await client.CreateDatasetAsync(id, dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public void CreateDataset_DatasetDifferentReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var anotherId = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = anotherId
                }
            };

            Assert.Throws<ArgumentException>(() => client.CreateDataset(id, dataset));
        }

        [Fact]
        public async Task CreateDatasetAsync_DatasetDifferentReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var anotherId = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = anotherId
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => client.CreateDatasetAsync(id, dataset));
        }

        [Fact]
        public void GetOrCreateDataset_DatasetSameReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = id
                }
            };

            var created = client.GetOrCreateDataset(id, resource: dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public async Task GetOrCreateDatasetAsync_DatasetSameReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = id
                }
            };

            var created = await client.GetOrCreateDatasetAsync(id, resource: dataset);
            Assert.Equal(_fixture.ProjectId, created.Reference.ProjectId);
            Assert.Equal(id, created.Reference.DatasetId);
        }

        [Fact]
        public void GetOrCreateDataset_DatasetDifferentReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var anotherId = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = anotherId
                }
            };

            Assert.Throws<ArgumentException>(() => client.GetOrCreateDataset(id, resource: dataset));
        }

        [Fact]
        public async Task GetOrCreateDatasetAsync_DatasetDifferentReference()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();
            var anotherId = _fixture.CreateDatasetId();
            var dataset = new Dataset
            {
                DatasetReference = new DatasetReference
                {
                    ProjectId = _fixture.ProjectId,
                    DatasetId = anotherId
                }
            };

            await Assert.ThrowsAsync<ArgumentException>(() => client.GetOrCreateDatasetAsync(id, resource: dataset));
        }

        [Fact]
        public void UpdateDataset()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify locally...
            original.Resource.Description = "Description2";
            original.Resource.FriendlyName = "FriendlyName2";

            // Check the results of the update
            var updated = original.Update();
            Assert.Equal("Description2", updated.Resource.Description);
            Assert.Equal("FriendlyName2", updated.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = client.GetDataset(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("FriendlyName2", fetched.Resource.FriendlyName);
            Assert.NotEqual(original.Resource.ETag, updated.Resource.ETag);
            Assert.Equal(fetched.Resource.ETag, updated.Resource.ETag);
        }

        [Fact]
        public void UpdateDataset_Conflict()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = client.GetDataset(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Modify the originally-created version...
            original.Resource.Description = "Description2";

            // Fails due to the conflict.
            var exception = Assert.Throws<GoogleApiException>(() => original.Update());
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        [Fact]
        public void PatchDataset()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });
            var patched = original.Patch(new Dataset { Description = "Description2" }, matchETag: false);

            // Check the results of the patch
            Assert.Equal("Description2", patched.Resource.Description);
            Assert.Equal("FriendlyName1", patched.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = client.GetDataset(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("FriendlyName1", fetched.Resource.FriendlyName);
            Assert.Equal(fetched.Resource.ETag, patched.Resource.ETag);
        }

        [Fact]
        public void PatchDataset_ConflictMatchEtag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = client.GetDataset(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Fails due to the conflict.
            var exception = Assert.Throws<GoogleApiException>(() => original.Patch(new Dataset { Description = "Description2" }, matchETag: true));
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        [Fact]
        public void PatchDataset_ConflictDontMatchEtag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = client.CreateDataset(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = client.GetDataset(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Patch from the original, but don't bother with optimistic concurrency checks. (Don't propagate the etag.)
            var patched = original.Patch(new Dataset { Description = "Description2" }, matchETag: false);

            // Check the results of the patch
            Assert.Equal("Description2", patched.Resource.Description);
            Assert.Equal("Sneak attack!", patched.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = client.GetDataset(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("Sneak attack!", fetched.Resource.FriendlyName);
        }

        [Fact]
        public async Task UpdateDatasetAsync()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = await client.CreateDatasetAsync(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify locally...
            original.Resource.Description = "Description2";
            original.Resource.FriendlyName = "FriendlyName2";

            // Check the results of the update
            var updated = await original.UpdateAsync();
            Assert.Equal("Description2", updated.Resource.Description);
            Assert.Equal("FriendlyName2", updated.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = await client.GetDatasetAsync(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("FriendlyName2", fetched.Resource.FriendlyName);
            Assert.NotEqual(original.Resource.ETag, updated.Resource.ETag);
            Assert.Equal(fetched.Resource.ETag, updated.Resource.ETag);
        }

        [Fact]
        public async Task UpdateDatasetAsync_Conflict()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = await client.CreateDatasetAsync(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = await client.GetDatasetAsync(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Modify the originally-created version...
            original.Resource.Description = "Description2";

            // Fails due to the conflict.
            var exception = await Assert.ThrowsAsync<GoogleApiException>(() => original.UpdateAsync());
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        [Fact]
        public async Task PatchDatasetAsync()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = await client.CreateDatasetAsync(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });
            var patched = await original.PatchAsync(new Dataset { Description = "Description2" }, matchETag: false);

            // Check the results of the patch
            Assert.Equal("Description2", patched.Resource.Description);
            Assert.Equal("FriendlyName1", patched.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = await client.GetDatasetAsync(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("FriendlyName1", fetched.Resource.FriendlyName);
            Assert.Equal(fetched.Resource.ETag, patched.Resource.ETag);
        }

        [Fact]
        public async Task PatchDatasetAsync_ConflictMatchEtag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = await client.CreateDatasetAsync(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = await client.GetDatasetAsync(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Fails due to the conflict.
            var exception = await Assert.ThrowsAsync<GoogleApiException>(() => original.PatchAsync(new Dataset { Description = "Description2" }, matchETag: true));
            Assert.Equal(HttpStatusCode.PreconditionFailed, exception.HttpStatusCode);
        }

        [Fact]
        public async Task PatchDatasetAsync_ConflictDontMatchEtag()
        {
            var client = BigQueryClient.Create(_fixture.ProjectId);
            var id = _fixture.CreateDatasetId();

            var original = await client.CreateDatasetAsync(id, new Dataset { Description = "Description1", FriendlyName = "FriendlyName1" });

            // Modify on the server, which will change the etag
            var sneaky = await client.GetDatasetAsync(id);
            sneaky.Resource.FriendlyName = "Sneak attack!";
            sneaky.Update();

            // Patch from the original, but don't bother with optimistic concurrency checks. (Don't propagate the etag.)
            var patched = await original.PatchAsync(new Dataset { Description = "Description2" }, matchETag: false);

            // Check the results of the patch
            Assert.Equal("Description2", patched.Resource.Description);
            Assert.Equal("Sneak attack!", patched.Resource.FriendlyName);

            // Check that it's still valid if fetched directly
            var fetched = await client.GetDatasetAsync(id);
            Assert.Equal("Description2", fetched.Resource.Description);
            Assert.Equal("Sneak attack!", fetched.Resource.FriendlyName);
        }

    }
}
