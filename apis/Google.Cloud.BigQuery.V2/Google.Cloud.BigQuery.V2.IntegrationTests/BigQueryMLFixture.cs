﻿// Copyright 2020 Google LLC
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

using Google.Cloud.ClientTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Google.Cloud.BigQuery.V2.IntegrationTests
{
    [CollectionDefinition(nameof(BigQueryMLFixture))]
    public class BigQueryMLFixture : CloudProjectFixtureBase, ICollectionFixture<BigQueryMLFixture>
    {
        public string DatasetId { get; }
        public string ModelId { get; }

        public BigQueryMLFixture()
        {
            DatasetId = IdGenerator.FromDateTime(prefix: "testml_");
            ModelId = CreateModelId();

            CreateData();
        }

        private void CreateData()
        {
            var client = BigQueryClient.Create(ProjectId);
            var dataset = client.CreateDataset(DatasetId);
            CreateModel(client, dataset.Reference.DatasetId, ModelId);
        }

        internal void CreateModel(BigQueryClient client, string datasetId, string modelId)
        {
            string createModelSql = $@"
CREATE MODEL {datasetId}.{modelId}
OPTIONS
  (model_type='linear_reg',
    input_label_cols=['label'],
    max_iteration = 1,
    learn_rate=0.4,
    learn_rate_strategy='constant') AS
SELECT 'a' AS f1, 2.0 AS label
UNION ALL
SELECT 'b' AS f1, 3.8 AS label";

            var createModelJob = client.CreateQueryJob(createModelSql, null);
            createModelJob.PollUntilCompleted().ThrowOnAnyError();
        }

        internal string CreateModelId() => IdGenerator.FromGuid(prefix: "model_", separator: "_");
    }
}
