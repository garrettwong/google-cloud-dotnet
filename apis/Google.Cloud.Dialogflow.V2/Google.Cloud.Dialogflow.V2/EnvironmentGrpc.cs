// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/cloud/dialogflow/v2/environment.proto
// </auto-generated>
// Original file comments:
// Copyright 2020 Google LLC
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
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Google.Cloud.Dialogflow.V2 {
  /// <summary>
  /// Service for managing [Environments][google.cloud.dialogflow.v2.Environment].
  /// </summary>
  public static partial class Environments
  {
    static readonly string __ServiceName = "google.cloud.dialogflow.v2.Environments";

    static readonly grpc::Marshaller<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest> __Marshaller_google_cloud_dialogflow_v2_ListEnvironmentsRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse> __Marshaller_google_cloud_dialogflow_v2_ListEnvironmentsResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest, global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse> __Method_ListEnvironments = new grpc::Method<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest, global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ListEnvironments",
        __Marshaller_google_cloud_dialogflow_v2_ListEnvironmentsRequest,
        __Marshaller_google_cloud_dialogflow_v2_ListEnvironmentsResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Google.Cloud.Dialogflow.V2.EnvironmentReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Environments</summary>
    [grpc::BindServiceMethod(typeof(Environments), "BindService")]
    public abstract partial class EnvironmentsBase
    {
      /// <summary>
      /// Returns the list of all non-draft environments of the specified agent.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse> ListEnvironments(global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Environments</summary>
    public partial class EnvironmentsClient : grpc::ClientBase<EnvironmentsClient>
    {
      /// <summary>Creates a new client for Environments</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public EnvironmentsClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Environments that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public EnvironmentsClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected EnvironmentsClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected EnvironmentsClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Returns the list of all non-draft environments of the specified agent.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse ListEnvironments(global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListEnvironments(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Returns the list of all non-draft environments of the specified agent.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse ListEnvironments(global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ListEnvironments, null, options, request);
      }
      /// <summary>
      /// Returns the list of all non-draft environments of the specified agent.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse> ListEnvironmentsAsync(global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListEnvironmentsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Returns the list of all non-draft environments of the specified agent.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse> ListEnvironmentsAsync(global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ListEnvironments, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override EnvironmentsClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new EnvironmentsClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(EnvironmentsBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ListEnvironments, serviceImpl.ListEnvironments).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, EnvironmentsBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ListEnvironments, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.Dialogflow.V2.ListEnvironmentsRequest, global::Google.Cloud.Dialogflow.V2.ListEnvironmentsResponse>(serviceImpl.ListEnvironments));
    }

  }
}
#endregion
