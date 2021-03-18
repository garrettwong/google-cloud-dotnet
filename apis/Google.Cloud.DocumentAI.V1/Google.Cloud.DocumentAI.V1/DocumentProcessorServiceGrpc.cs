// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/cloud/documentai/v1/document_processor_service.proto
// </auto-generated>
// Original file comments:
// Copyright 2021 Google LLC
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

namespace Google.Cloud.DocumentAI.V1 {
  /// <summary>
  /// Service to call Cloud DocumentAI to process documents according to the
  /// processor's definition. Processors are built using state-of-the-art Google
  /// AI such as natural language, computer vision, and translation to extract
  /// structured information from unstructured or semi-structured documents.
  /// </summary>
  public static partial class DocumentProcessorService
  {
    static readonly string __ServiceName = "google.cloud.documentai.v1.DocumentProcessorService";

    static readonly grpc::Marshaller<global::Google.Cloud.DocumentAI.V1.ProcessRequest> __Marshaller_google_cloud_documentai_v1_ProcessRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.DocumentAI.V1.ProcessRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.DocumentAI.V1.ProcessResponse> __Marshaller_google_cloud_documentai_v1_ProcessResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.DocumentAI.V1.ProcessResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.DocumentAI.V1.BatchProcessRequest> __Marshaller_google_cloud_documentai_v1_BatchProcessRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.DocumentAI.V1.BatchProcessRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.LongRunning.Operation> __Marshaller_google_longrunning_Operation = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.LongRunning.Operation.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest> __Marshaller_google_cloud_documentai_v1_ReviewDocumentRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest.Parser.ParseFrom);

    static readonly grpc::Method<global::Google.Cloud.DocumentAI.V1.ProcessRequest, global::Google.Cloud.DocumentAI.V1.ProcessResponse> __Method_ProcessDocument = new grpc::Method<global::Google.Cloud.DocumentAI.V1.ProcessRequest, global::Google.Cloud.DocumentAI.V1.ProcessResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ProcessDocument",
        __Marshaller_google_cloud_documentai_v1_ProcessRequest,
        __Marshaller_google_cloud_documentai_v1_ProcessResponse);

    static readonly grpc::Method<global::Google.Cloud.DocumentAI.V1.BatchProcessRequest, global::Google.LongRunning.Operation> __Method_BatchProcessDocuments = new grpc::Method<global::Google.Cloud.DocumentAI.V1.BatchProcessRequest, global::Google.LongRunning.Operation>(
        grpc::MethodType.Unary,
        __ServiceName,
        "BatchProcessDocuments",
        __Marshaller_google_cloud_documentai_v1_BatchProcessRequest,
        __Marshaller_google_longrunning_Operation);

    static readonly grpc::Method<global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest, global::Google.LongRunning.Operation> __Method_ReviewDocument = new grpc::Method<global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest, global::Google.LongRunning.Operation>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ReviewDocument",
        __Marshaller_google_cloud_documentai_v1_ReviewDocumentRequest,
        __Marshaller_google_longrunning_Operation);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Google.Cloud.DocumentAI.V1.DocumentProcessorServiceReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of DocumentProcessorService</summary>
    [grpc::BindServiceMethod(typeof(DocumentProcessorService), "BindService")]
    public abstract partial class DocumentProcessorServiceBase
    {
      /// <summary>
      /// Processes a single document.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.Cloud.DocumentAI.V1.ProcessResponse> ProcessDocument(global::Google.Cloud.DocumentAI.V1.ProcessRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// LRO endpoint to batch process many documents. The output is written
      /// to Cloud Storage as JSON in the [Document] format.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.LongRunning.Operation> BatchProcessDocuments(global::Google.Cloud.DocumentAI.V1.BatchProcessRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Send a document for Human Review. The input document should be processed by
      /// the specified processor.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.LongRunning.Operation> ReviewDocument(global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for DocumentProcessorService</summary>
    public partial class DocumentProcessorServiceClient : grpc::ClientBase<DocumentProcessorServiceClient>
    {
      /// <summary>Creates a new client for DocumentProcessorService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public DocumentProcessorServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for DocumentProcessorService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public DocumentProcessorServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected DocumentProcessorServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected DocumentProcessorServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Processes a single document.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.DocumentAI.V1.ProcessResponse ProcessDocument(global::Google.Cloud.DocumentAI.V1.ProcessRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ProcessDocument(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Processes a single document.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.DocumentAI.V1.ProcessResponse ProcessDocument(global::Google.Cloud.DocumentAI.V1.ProcessRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ProcessDocument, null, options, request);
      }
      /// <summary>
      /// Processes a single document.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.DocumentAI.V1.ProcessResponse> ProcessDocumentAsync(global::Google.Cloud.DocumentAI.V1.ProcessRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ProcessDocumentAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Processes a single document.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.DocumentAI.V1.ProcessResponse> ProcessDocumentAsync(global::Google.Cloud.DocumentAI.V1.ProcessRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ProcessDocument, null, options, request);
      }
      /// <summary>
      /// LRO endpoint to batch process many documents. The output is written
      /// to Cloud Storage as JSON in the [Document] format.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.LongRunning.Operation BatchProcessDocuments(global::Google.Cloud.DocumentAI.V1.BatchProcessRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return BatchProcessDocuments(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// LRO endpoint to batch process many documents. The output is written
      /// to Cloud Storage as JSON in the [Document] format.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.LongRunning.Operation BatchProcessDocuments(global::Google.Cloud.DocumentAI.V1.BatchProcessRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_BatchProcessDocuments, null, options, request);
      }
      /// <summary>
      /// LRO endpoint to batch process many documents. The output is written
      /// to Cloud Storage as JSON in the [Document] format.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.LongRunning.Operation> BatchProcessDocumentsAsync(global::Google.Cloud.DocumentAI.V1.BatchProcessRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return BatchProcessDocumentsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// LRO endpoint to batch process many documents. The output is written
      /// to Cloud Storage as JSON in the [Document] format.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.LongRunning.Operation> BatchProcessDocumentsAsync(global::Google.Cloud.DocumentAI.V1.BatchProcessRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_BatchProcessDocuments, null, options, request);
      }
      /// <summary>
      /// Send a document for Human Review. The input document should be processed by
      /// the specified processor.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.LongRunning.Operation ReviewDocument(global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReviewDocument(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Send a document for Human Review. The input document should be processed by
      /// the specified processor.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.LongRunning.Operation ReviewDocument(global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ReviewDocument, null, options, request);
      }
      /// <summary>
      /// Send a document for Human Review. The input document should be processed by
      /// the specified processor.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.LongRunning.Operation> ReviewDocumentAsync(global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ReviewDocumentAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Send a document for Human Review. The input document should be processed by
      /// the specified processor.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.LongRunning.Operation> ReviewDocumentAsync(global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ReviewDocument, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override DocumentProcessorServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new DocumentProcessorServiceClient(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(DocumentProcessorServiceBase serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_ProcessDocument, serviceImpl.ProcessDocument)
          .AddMethod(__Method_BatchProcessDocuments, serviceImpl.BatchProcessDocuments)
          .AddMethod(__Method_ReviewDocument, serviceImpl.ReviewDocument).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, DocumentProcessorServiceBase serviceImpl)
    {
      serviceBinder.AddMethod(__Method_ProcessDocument, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.DocumentAI.V1.ProcessRequest, global::Google.Cloud.DocumentAI.V1.ProcessResponse>(serviceImpl.ProcessDocument));
      serviceBinder.AddMethod(__Method_BatchProcessDocuments, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.DocumentAI.V1.BatchProcessRequest, global::Google.LongRunning.Operation>(serviceImpl.BatchProcessDocuments));
      serviceBinder.AddMethod(__Method_ReviewDocument, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.DocumentAI.V1.ReviewDocumentRequest, global::Google.LongRunning.Operation>(serviceImpl.ReviewDocument));
    }

  }
}
#endregion
