// Copyright 2021 Google LLC
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

// Generated code. DO NOT EDIT!

using gaxgrpc = Google.Api.Gax.Grpc;
using grpccore = Grpc.Core;
using moq = Moq;
using st = System.Threading;
using stt = System.Threading.Tasks;
using xunit = Xunit;

namespace Google.Cloud.Compute.V1.Tests
{
    /// <summary>Generated unit tests.</summary>
    public sealed class GeneratedRegionNotificationEndpointsClientTest
    {
        [xunit::FactAttribute]
        public void DeleteRequestObject()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            DeleteRegionNotificationEndpointRequest request = new DeleteRegionNotificationEndpointRequest
            {
                RequestId = "request_id362c8df6",
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.Delete(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation response = client.Delete(request);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task DeleteRequestObjectAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            DeleteRegionNotificationEndpointRequest request = new DeleteRegionNotificationEndpointRequest
            {
                RequestId = "request_id362c8df6",
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.DeleteAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<Operation>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation responseCallSettings = await client.DeleteAsync(request, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            Operation responseCancellationToken = await client.DeleteAsync(request, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void Delete()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            DeleteRegionNotificationEndpointRequest request = new DeleteRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.Delete(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation response = client.Delete(request.Project, request.Region, request.NotificationEndpoint);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task DeleteAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            DeleteRegionNotificationEndpointRequest request = new DeleteRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.DeleteAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<Operation>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation responseCallSettings = await client.DeleteAsync(request.Project, request.Region, request.NotificationEndpoint, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            Operation responseCancellationToken = await client.DeleteAsync(request.Project, request.Region, request.NotificationEndpoint, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void GetRequestObject()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            GetRegionNotificationEndpointRequest request = new GetRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            NotificationEndpoint expectedResponse = new NotificationEndpoint
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                CreationTimestamp = "creation_timestamp235e59a1",
                Region = "regionedb20d96",
                Description = "description2cf9da67",
                GrpcSettings = new NotificationEndpointGrpcSettings(),
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.Get(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpoint response = client.Get(request);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task GetRequestObjectAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            GetRegionNotificationEndpointRequest request = new GetRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            NotificationEndpoint expectedResponse = new NotificationEndpoint
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                CreationTimestamp = "creation_timestamp235e59a1",
                Region = "regionedb20d96",
                Description = "description2cf9da67",
                GrpcSettings = new NotificationEndpointGrpcSettings(),
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.GetAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<NotificationEndpoint>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpoint responseCallSettings = await client.GetAsync(request, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            NotificationEndpoint responseCancellationToken = await client.GetAsync(request, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void Get()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            GetRegionNotificationEndpointRequest request = new GetRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            NotificationEndpoint expectedResponse = new NotificationEndpoint
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                CreationTimestamp = "creation_timestamp235e59a1",
                Region = "regionedb20d96",
                Description = "description2cf9da67",
                GrpcSettings = new NotificationEndpointGrpcSettings(),
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.Get(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpoint response = client.Get(request.Project, request.Region, request.NotificationEndpoint);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task GetAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            GetRegionNotificationEndpointRequest request = new GetRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpoint = "notification_endpointbe78db22",
            };
            NotificationEndpoint expectedResponse = new NotificationEndpoint
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                CreationTimestamp = "creation_timestamp235e59a1",
                Region = "regionedb20d96",
                Description = "description2cf9da67",
                GrpcSettings = new NotificationEndpointGrpcSettings(),
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.GetAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<NotificationEndpoint>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpoint responseCallSettings = await client.GetAsync(request.Project, request.Region, request.NotificationEndpoint, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            NotificationEndpoint responseCancellationToken = await client.GetAsync(request.Project, request.Region, request.NotificationEndpoint, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void InsertRequestObject()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            InsertRegionNotificationEndpointRequest request = new InsertRegionNotificationEndpointRequest
            {
                RequestId = "request_id362c8df6",
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpointResource = new NotificationEndpoint(),
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.Insert(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation response = client.Insert(request);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task InsertRequestObjectAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            InsertRegionNotificationEndpointRequest request = new InsertRegionNotificationEndpointRequest
            {
                RequestId = "request_id362c8df6",
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpointResource = new NotificationEndpoint(),
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.InsertAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<Operation>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation responseCallSettings = await client.InsertAsync(request, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            Operation responseCancellationToken = await client.InsertAsync(request, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void Insert()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            InsertRegionNotificationEndpointRequest request = new InsertRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpointResource = new NotificationEndpoint(),
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.Insert(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation response = client.Insert(request.Project, request.Region, request.NotificationEndpointResource);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task InsertAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            InsertRegionNotificationEndpointRequest request = new InsertRegionNotificationEndpointRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
                NotificationEndpointResource = new NotificationEndpoint(),
            };
            Operation expectedResponse = new Operation
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Name = "name1c9368b0",
                User = "userb1cb11ee",
                Zone = "zone255f4ea8",
                CreationTimestamp = "creation_timestamp235e59a1",
                StartTime = "start_timebd8dd9c4",
                TargetLink = "target_link9b435dc0",
                Progress = 278622268,
                Error = new Error(),
                EndTime = "end_time89285d30",
                Region = "regionedb20d96",
                OperationType = "operation_typeece9e153",
                Status = Operation.Types.Status.Pending,
                HttpErrorMessage = "http_error_messageb5ef3c7f",
                TargetId = "target_id16dfe255",
                ClientOperationId = "client_operation_id4e51b631",
                StatusMessage = "status_message2c618f86",
                HttpErrorStatusCode = 1766362655,
                Description = "description2cf9da67",
                InsertTime = "insert_time7467185a",
                SelfLink = "self_link7e87f12d",
                Warnings = { new Warnings(), },
            };
            mockGrpcClient.Setup(x => x.InsertAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<Operation>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            Operation responseCallSettings = await client.InsertAsync(request.Project, request.Region, request.NotificationEndpointResource, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            Operation responseCancellationToken = await client.InsertAsync(request.Project, request.Region, request.NotificationEndpointResource, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void ListRequestObject()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            ListRegionNotificationEndpointsRequest request = new ListRegionNotificationEndpointsRequest
            {
                PageToken = "page_tokenf09e5538",
                MaxResults = 2806814450U,
                Region = "regionedb20d96",
                OrderBy = "order_byb4d33ada",
                Project = "projectaa6ff846",
                Filter = "filtere47ac9b2",
                ReturnPartialSuccess = false,
            };
            NotificationEndpointList expectedResponse = new NotificationEndpointList
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Warning = new Warning(),
                NextPageToken = "next_page_tokendbee0940",
                Items =
                {
                    new NotificationEndpoint(),
                },
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.List(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpointList response = client.List(request);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task ListRequestObjectAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            ListRegionNotificationEndpointsRequest request = new ListRegionNotificationEndpointsRequest
            {
                PageToken = "page_tokenf09e5538",
                MaxResults = 2806814450U,
                Region = "regionedb20d96",
                OrderBy = "order_byb4d33ada",
                Project = "projectaa6ff846",
                Filter = "filtere47ac9b2",
                ReturnPartialSuccess = false,
            };
            NotificationEndpointList expectedResponse = new NotificationEndpointList
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Warning = new Warning(),
                NextPageToken = "next_page_tokendbee0940",
                Items =
                {
                    new NotificationEndpoint(),
                },
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.ListAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<NotificationEndpointList>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpointList responseCallSettings = await client.ListAsync(request, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            NotificationEndpointList responseCancellationToken = await client.ListAsync(request, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public void List()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            ListRegionNotificationEndpointsRequest request = new ListRegionNotificationEndpointsRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
            };
            NotificationEndpointList expectedResponse = new NotificationEndpointList
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Warning = new Warning(),
                NextPageToken = "next_page_tokendbee0940",
                Items =
                {
                    new NotificationEndpoint(),
                },
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.List(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(expectedResponse);
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpointList response = client.List(request.Project, request.Region);
            xunit::Assert.Same(expectedResponse, response);
            mockGrpcClient.VerifyAll();
        }

        [xunit::FactAttribute]
        public async stt::Task ListAsync()
        {
            moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient> mockGrpcClient = new moq::Mock<RegionNotificationEndpoints.RegionNotificationEndpointsClient>(moq::MockBehavior.Strict);
            ListRegionNotificationEndpointsRequest request = new ListRegionNotificationEndpointsRequest
            {
                Region = "regionedb20d96",
                Project = "projectaa6ff846",
            };
            NotificationEndpointList expectedResponse = new NotificationEndpointList
            {
                Id = "id74b70bb8",
                Kind = "kindf7aa39d9",
                Warning = new Warning(),
                NextPageToken = "next_page_tokendbee0940",
                Items =
                {
                    new NotificationEndpoint(),
                },
                SelfLink = "self_link7e87f12d",
            };
            mockGrpcClient.Setup(x => x.ListAsync(request, moq::It.IsAny<grpccore::CallOptions>())).Returns(new grpccore::AsyncUnaryCall<NotificationEndpointList>(stt::Task.FromResult(expectedResponse), null, null, null, null));
            RegionNotificationEndpointsClient client = new RegionNotificationEndpointsClientImpl(mockGrpcClient.Object, null);
            NotificationEndpointList responseCallSettings = await client.ListAsync(request.Project, request.Region, gaxgrpc::CallSettings.FromCancellationToken(st::CancellationToken.None));
            xunit::Assert.Same(expectedResponse, responseCallSettings);
            NotificationEndpointList responseCancellationToken = await client.ListAsync(request.Project, request.Region, st::CancellationToken.None);
            xunit::Assert.Same(expectedResponse, responseCancellationToken);
            mockGrpcClient.VerifyAll();
        }
    }
}
