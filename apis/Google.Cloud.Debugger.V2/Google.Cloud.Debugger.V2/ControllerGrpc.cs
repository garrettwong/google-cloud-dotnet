// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/devtools/clouddebugger/v2/controller.proto
// </auto-generated>
// Original file comments:
// Copyright 2019 Google LLC.
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
//
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace Google.Cloud.Debugger.V2 {
  /// <summary>
  /// The Controller service provides the API for orchestrating a collection of
  /// debugger agents to perform debugging tasks. These agents are each attached
  /// to a process of an application which may include one or more replicas.
  ///
  /// The debugger agents register with the Controller to identify the application
  /// being debugged, the Debuggee. All agents that register with the same data,
  /// represent the same Debuggee, and are assigned the same `debuggee_id`.
  ///
  /// The debugger agents call the Controller to retrieve  the list of active
  /// Breakpoints. Agents with the same `debuggee_id` get the same breakpoints
  /// list. An agent that can fulfill the breakpoint request updates the
  /// Controller with the breakpoint result. The controller selects the first
  /// result received and discards the rest of the results.
  /// Agents that poll again for active breakpoints will no longer have
  /// the completed breakpoint in the list and should remove that breakpoint from
  /// their attached process.
  ///
  /// The Controller service does not provide a way to retrieve the results of
  /// a completed breakpoint. This functionality is available using the Debugger
  /// service.
  /// </summary>
  public static partial class Controller2
  {
    static readonly string __ServiceName = "google.devtools.clouddebugger.v2.Controller2";

    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest> __Marshaller_google_devtools_clouddebugger_v2_RegisterDebuggeeRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse> __Marshaller_google_devtools_clouddebugger_v2_RegisterDebuggeeResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest> __Marshaller_google_devtools_clouddebugger_v2_ListActiveBreakpointsRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse> __Marshaller_google_devtools_clouddebugger_v2_ListActiveBreakpointsResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest> __Marshaller_google_devtools_clouddebugger_v2_UpdateActiveBreakpointRequest = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest.Parser.ParseFrom);
    static readonly grpc::Marshaller<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse> __Marshaller_google_devtools_clouddebugger_v2_UpdateActiveBreakpointResponse = grpc::Marshallers.Create((arg) => global::Google.Protobuf.MessageExtensions.ToByteArray(arg), global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse.Parser.ParseFrom);

    static readonly grpc::Method<global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest, global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse> __Method_RegisterDebuggee = new grpc::Method<global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest, global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "RegisterDebuggee",
        __Marshaller_google_devtools_clouddebugger_v2_RegisterDebuggeeRequest,
        __Marshaller_google_devtools_clouddebugger_v2_RegisterDebuggeeResponse);

    static readonly grpc::Method<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest, global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse> __Method_ListActiveBreakpoints = new grpc::Method<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest, global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "ListActiveBreakpoints",
        __Marshaller_google_devtools_clouddebugger_v2_ListActiveBreakpointsRequest,
        __Marshaller_google_devtools_clouddebugger_v2_ListActiveBreakpointsResponse);

    static readonly grpc::Method<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest, global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse> __Method_UpdateActiveBreakpoint = new grpc::Method<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest, global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "UpdateActiveBreakpoint",
        __Marshaller_google_devtools_clouddebugger_v2_UpdateActiveBreakpointRequest,
        __Marshaller_google_devtools_clouddebugger_v2_UpdateActiveBreakpointResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::Google.Cloud.Debugger.V2.ControllerReflection.Descriptor.Services[0]; }
    }

    /// <summary>Base class for server-side implementations of Controller2</summary>
    [grpc::BindServiceMethod(typeof(Controller2), "BindService")]
    public abstract partial class Controller2Base
    {
      /// <summary>
      /// Registers the debuggee with the controller service.
      ///
      /// All agents attached to the same application must call this method with
      /// exactly the same request content to get back the same stable `debuggee_id`.
      /// Agents should call this method again whenever `google.rpc.Code.NOT_FOUND`
      /// is returned from any controller method.
      ///
      /// This protocol allows the controller service to disable debuggees, recover
      /// from data loss, or change the `debuggee_id` format. Agents must handle
      /// `debuggee_id` value changing upon re-registration.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse> RegisterDebuggee(global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Returns the list of all active breakpoints for the debuggee.
      ///
      /// The breakpoint specification (`location`, `condition`, and `expressions`
      /// fields) is semantically immutable, although the field values may
      /// change. For example, an agent may update the location line number
      /// to reflect the actual line where the breakpoint was set, but this
      /// doesn't change the breakpoint semantics.
      ///
      /// This means that an agent does not need to check if a breakpoint has changed
      /// when it encounters the same breakpoint on a successive call.
      /// Moreover, an agent should remember the breakpoints that are completed
      /// until the controller removes them from the active list to avoid
      /// setting those breakpoints again.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse> ListActiveBreakpoints(global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

      /// <summary>
      /// Updates the breakpoint state or mutable fields.
      /// The entire Breakpoint message must be sent back to the controller service.
      ///
      /// Updates to active breakpoint fields are only allowed if the new value
      /// does not change the breakpoint specification. Updates to the `location`,
      /// `condition` and `expressions` fields should not alter the breakpoint
      /// semantics. These may only make changes such as canonicalizing a value
      /// or snapping the location to the correct line of code.
      /// </summary>
      /// <param name="request">The request received from the client.</param>
      /// <param name="context">The context of the server-side call handler being invoked.</param>
      /// <returns>The response to send back to the client (wrapped by a task).</returns>
      public virtual global::System.Threading.Tasks.Task<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse> UpdateActiveBreakpoint(global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest request, grpc::ServerCallContext context)
      {
        throw new grpc::RpcException(new grpc::Status(grpc::StatusCode.Unimplemented, ""));
      }

    }

    /// <summary>Client for Controller2</summary>
    public partial class Controller2Client : grpc::ClientBase<Controller2Client>
    {
      /// <summary>Creates a new client for Controller2</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      public Controller2Client(grpc::Channel channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for Controller2 that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      public Controller2Client(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      protected Controller2Client() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      protected Controller2Client(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      /// <summary>
      /// Registers the debuggee with the controller service.
      ///
      /// All agents attached to the same application must call this method with
      /// exactly the same request content to get back the same stable `debuggee_id`.
      /// Agents should call this method again whenever `google.rpc.Code.NOT_FOUND`
      /// is returned from any controller method.
      ///
      /// This protocol allows the controller service to disable debuggees, recover
      /// from data loss, or change the `debuggee_id` format. Agents must handle
      /// `debuggee_id` value changing upon re-registration.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse RegisterDebuggee(global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterDebuggee(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Registers the debuggee with the controller service.
      ///
      /// All agents attached to the same application must call this method with
      /// exactly the same request content to get back the same stable `debuggee_id`.
      /// Agents should call this method again whenever `google.rpc.Code.NOT_FOUND`
      /// is returned from any controller method.
      ///
      /// This protocol allows the controller service to disable debuggees, recover
      /// from data loss, or change the `debuggee_id` format. Agents must handle
      /// `debuggee_id` value changing upon re-registration.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse RegisterDebuggee(global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_RegisterDebuggee, null, options, request);
      }
      /// <summary>
      /// Registers the debuggee with the controller service.
      ///
      /// All agents attached to the same application must call this method with
      /// exactly the same request content to get back the same stable `debuggee_id`.
      /// Agents should call this method again whenever `google.rpc.Code.NOT_FOUND`
      /// is returned from any controller method.
      ///
      /// This protocol allows the controller service to disable debuggees, recover
      /// from data loss, or change the `debuggee_id` format. Agents must handle
      /// `debuggee_id` value changing upon re-registration.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse> RegisterDebuggeeAsync(global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return RegisterDebuggeeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Registers the debuggee with the controller service.
      ///
      /// All agents attached to the same application must call this method with
      /// exactly the same request content to get back the same stable `debuggee_id`.
      /// Agents should call this method again whenever `google.rpc.Code.NOT_FOUND`
      /// is returned from any controller method.
      ///
      /// This protocol allows the controller service to disable debuggees, recover
      /// from data loss, or change the `debuggee_id` format. Agents must handle
      /// `debuggee_id` value changing upon re-registration.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse> RegisterDebuggeeAsync(global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_RegisterDebuggee, null, options, request);
      }
      /// <summary>
      /// Returns the list of all active breakpoints for the debuggee.
      ///
      /// The breakpoint specification (`location`, `condition`, and `expressions`
      /// fields) is semantically immutable, although the field values may
      /// change. For example, an agent may update the location line number
      /// to reflect the actual line where the breakpoint was set, but this
      /// doesn't change the breakpoint semantics.
      ///
      /// This means that an agent does not need to check if a breakpoint has changed
      /// when it encounters the same breakpoint on a successive call.
      /// Moreover, an agent should remember the breakpoints that are completed
      /// until the controller removes them from the active list to avoid
      /// setting those breakpoints again.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse ListActiveBreakpoints(global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListActiveBreakpoints(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Returns the list of all active breakpoints for the debuggee.
      ///
      /// The breakpoint specification (`location`, `condition`, and `expressions`
      /// fields) is semantically immutable, although the field values may
      /// change. For example, an agent may update the location line number
      /// to reflect the actual line where the breakpoint was set, but this
      /// doesn't change the breakpoint semantics.
      ///
      /// This means that an agent does not need to check if a breakpoint has changed
      /// when it encounters the same breakpoint on a successive call.
      /// Moreover, an agent should remember the breakpoints that are completed
      /// until the controller removes them from the active list to avoid
      /// setting those breakpoints again.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse ListActiveBreakpoints(global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_ListActiveBreakpoints, null, options, request);
      }
      /// <summary>
      /// Returns the list of all active breakpoints for the debuggee.
      ///
      /// The breakpoint specification (`location`, `condition`, and `expressions`
      /// fields) is semantically immutable, although the field values may
      /// change. For example, an agent may update the location line number
      /// to reflect the actual line where the breakpoint was set, but this
      /// doesn't change the breakpoint semantics.
      ///
      /// This means that an agent does not need to check if a breakpoint has changed
      /// when it encounters the same breakpoint on a successive call.
      /// Moreover, an agent should remember the breakpoints that are completed
      /// until the controller removes them from the active list to avoid
      /// setting those breakpoints again.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse> ListActiveBreakpointsAsync(global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return ListActiveBreakpointsAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Returns the list of all active breakpoints for the debuggee.
      ///
      /// The breakpoint specification (`location`, `condition`, and `expressions`
      /// fields) is semantically immutable, although the field values may
      /// change. For example, an agent may update the location line number
      /// to reflect the actual line where the breakpoint was set, but this
      /// doesn't change the breakpoint semantics.
      ///
      /// This means that an agent does not need to check if a breakpoint has changed
      /// when it encounters the same breakpoint on a successive call.
      /// Moreover, an agent should remember the breakpoints that are completed
      /// until the controller removes them from the active list to avoid
      /// setting those breakpoints again.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse> ListActiveBreakpointsAsync(global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_ListActiveBreakpoints, null, options, request);
      }
      /// <summary>
      /// Updates the breakpoint state or mutable fields.
      /// The entire Breakpoint message must be sent back to the controller service.
      ///
      /// Updates to active breakpoint fields are only allowed if the new value
      /// does not change the breakpoint specification. Updates to the `location`,
      /// `condition` and `expressions` fields should not alter the breakpoint
      /// semantics. These may only make changes such as canonicalizing a value
      /// or snapping the location to the correct line of code.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse UpdateActiveBreakpoint(global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateActiveBreakpoint(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Updates the breakpoint state or mutable fields.
      /// The entire Breakpoint message must be sent back to the controller service.
      ///
      /// Updates to active breakpoint fields are only allowed if the new value
      /// does not change the breakpoint specification. Updates to the `location`,
      /// `condition` and `expressions` fields should not alter the breakpoint
      /// semantics. These may only make changes such as canonicalizing a value
      /// or snapping the location to the correct line of code.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The response received from the server.</returns>
      public virtual global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse UpdateActiveBreakpoint(global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_UpdateActiveBreakpoint, null, options, request);
      }
      /// <summary>
      /// Updates the breakpoint state or mutable fields.
      /// The entire Breakpoint message must be sent back to the controller service.
      ///
      /// Updates to active breakpoint fields are only allowed if the new value
      /// does not change the breakpoint specification. Updates to the `location`,
      /// `condition` and `expressions` fields should not alter the breakpoint
      /// semantics. These may only make changes such as canonicalizing a value
      /// or snapping the location to the correct line of code.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="headers">The initial metadata to send with the call. This parameter is optional.</param>
      /// <param name="deadline">An optional deadline for the call. The call will be cancelled if deadline is hit.</param>
      /// <param name="cancellationToken">An optional token for canceling the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse> UpdateActiveBreakpointAsync(global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return UpdateActiveBreakpointAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      /// <summary>
      /// Updates the breakpoint state or mutable fields.
      /// The entire Breakpoint message must be sent back to the controller service.
      ///
      /// Updates to active breakpoint fields are only allowed if the new value
      /// does not change the breakpoint specification. Updates to the `location`,
      /// `condition` and `expressions` fields should not alter the breakpoint
      /// semantics. These may only make changes such as canonicalizing a value
      /// or snapping the location to the correct line of code.
      /// </summary>
      /// <param name="request">The request to send to the server.</param>
      /// <param name="options">The options for the call.</param>
      /// <returns>The call object.</returns>
      public virtual grpc::AsyncUnaryCall<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse> UpdateActiveBreakpointAsync(global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_UpdateActiveBreakpoint, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      protected override Controller2Client NewInstance(ClientBaseConfiguration configuration)
      {
        return new Controller2Client(configuration);
      }
    }

    /// <summary>Creates service definition that can be registered with a server</summary>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static grpc::ServerServiceDefinition BindService(Controller2Base serviceImpl)
    {
      return grpc::ServerServiceDefinition.CreateBuilder()
          .AddMethod(__Method_RegisterDebuggee, serviceImpl.RegisterDebuggee)
          .AddMethod(__Method_ListActiveBreakpoints, serviceImpl.ListActiveBreakpoints)
          .AddMethod(__Method_UpdateActiveBreakpoint, serviceImpl.UpdateActiveBreakpoint).Build();
    }

    /// <summary>Register service method with a service binder with or without implementation. Useful when customizing the  service binding logic.
    /// Note: this method is part of an experimental API that can change or be removed without any prior notice.</summary>
    /// <param name="serviceBinder">Service methods will be bound by calling <c>AddMethod</c> on this object.</param>
    /// <param name="serviceImpl">An object implementing the server-side handling logic.</param>
    public static void BindService(grpc::ServiceBinderBase serviceBinder, Controller2Base serviceImpl)
    {
      serviceBinder.AddMethod(__Method_RegisterDebuggee, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.Debugger.V2.RegisterDebuggeeRequest, global::Google.Cloud.Debugger.V2.RegisterDebuggeeResponse>(serviceImpl.RegisterDebuggee));
      serviceBinder.AddMethod(__Method_ListActiveBreakpoints, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.Debugger.V2.ListActiveBreakpointsRequest, global::Google.Cloud.Debugger.V2.ListActiveBreakpointsResponse>(serviceImpl.ListActiveBreakpoints));
      serviceBinder.AddMethod(__Method_UpdateActiveBreakpoint, serviceImpl == null ? null : new grpc::UnaryServerMethod<global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointRequest, global::Google.Cloud.Debugger.V2.UpdateActiveBreakpointResponse>(serviceImpl.UpdateActiveBreakpoint));
    }

  }
}
#endregion
