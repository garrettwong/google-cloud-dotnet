﻿// Copyright 2017, Google Inc. All rights reserved.
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

using Google.Api.Gax.Grpc.GrpcCore;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.Bigtable.Admin.V2;
using Google.Cloud.Bigtable.Common.V2;
using Google.Cloud.ClientTesting;
using Google.Rpc;
using Grpc.Core;
using Grpc.Gcp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Google.Cloud.Bigtable.V2.IntegrationTests
{
    public abstract class BigtableFixtureBase : IDisposable
    {
        private const string EmulatorEnvironmentVariable = "BIGTABLE_EMULATOR_HOST";
        private const string TestInstanceEnvironmentVariable = "BIGTABLE_TEST_INSTANCE";
        private const string TestProjectEnvironmentVariable = TestEnvironment.TestProjectEnvironmentVariable;

        public ProjectName ProjectName { get; private set; }
        public InstanceName InstanceName { get; private set; }
        public TableName TableName { get; private set; }

        public BigtableInstanceAdminClient InstanceAdminClient { get; private set; }
        public BigtableTableAdminClient TableAdminClient { get; private set; }
        public BigtableClient TableClient { get; private set; }

        public List<TableName> CreatedTables { get; } = new List<TableName>();
        public CallInvoker EmulatorCallInvoker { get; }

        public BigtableFixtureBase()
        {
            GrpcInfo.EnableSubchannelCounting();

            string emulatorHost = Environment.GetEnvironmentVariable(EmulatorEnvironmentVariable);

            string projectId;
            string instanceId;
            if (!string.IsNullOrEmpty(emulatorHost))
            {
                projectId = "emulator-test-project";
                EmulatorCallInvoker = new GcpCallInvoker(
                    emulatorHost,
                    ChannelCredentials.Insecure,
                    GrpcCoreAdapter.Instance.ConvertOptions(BigtableServiceApiSettings.GetDefault().CreateChannelOptions()));

                instanceId = "doesnt-matter";
            }
            else
            {
                projectId = Environment.GetEnvironmentVariable(TestProjectEnvironmentVariable);
                if (string.IsNullOrEmpty(projectId))
                {
                    throw new InvalidOperationException(
                        $"Please set either the {EmulatorEnvironmentVariable} or {TestProjectEnvironmentVariable} environment variable before running tests");
                }

                instanceId = Environment.GetEnvironmentVariable(TestInstanceEnvironmentVariable);
                if (string.IsNullOrEmpty(instanceId))
                {
                    throw new InvalidOperationException(
                        $"Please set the {TestInstanceEnvironmentVariable} environment variable before running non-emulator tests.");
                }
            }

            ProjectName = new ProjectName(projectId);
            InstanceName = new InstanceName(projectId, instanceId);

            Task.Run(InitBigtableInstanceAndTable).Wait();
        }

        protected abstract Table CreateDefaultTable();

        public async Task<TableName> CreateTable()
        {
            var tableName = new TableName(ProjectName.ProjectId, InstanceName.InstanceId, IdGenerator.FromGuid());
            CreatedTables.Add(tableName);
            await TableAdminClient.CreateTableAsync(
                new InstanceName(tableName.ProjectId, tableName.InstanceId),
                tableName.TableId,
                CreateDefaultTable());
            return tableName;
        }

        private async Task InitBigtableInstanceAndTable()
        {
            if (EmulatorCallInvoker == null)
            {
                InstanceAdminClient = BigtableInstanceAdminClient.Create();
                TableAdminClient = BigtableTableAdminClient.Create();
                TableClient = BigtableClient.Create();

                try
                {
                    await InstanceAdminClient.GetInstanceAsync(InstanceName);
                }
                catch (RpcException e) when (e.Status.StatusCode == StatusCode.NotFound)
                {
                    Assert.True(false, $"The Bigtable instance for testing does not exist: {InstanceName}");
                }
            }
            else
            {
                TableAdminClient = new BigtableTableAdminClientBuilder { CallInvoker = EmulatorCallInvoker }.Build();
                TableClient = new BigtableClientBuilder { CallInvoker = EmulatorCallInvoker }.Build();
            }

            TableName = await CreateTable();
        }

        public void Dispose()
        {
            if (TableAdminClient != null)
            {
                foreach (var tableName in CreatedTables)
                {
                    try
                    {
                        TableAdminClient.DeleteTable(new DeleteTableRequest { Name = tableName.ToString() });
                    }
                    catch { }
                }
            }
        }
    }
}
