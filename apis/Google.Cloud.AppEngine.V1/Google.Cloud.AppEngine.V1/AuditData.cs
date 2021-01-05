// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/appengine/v1/audit_data.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Cloud.AppEngine.V1 {

  /// <summary>Holder for reflection information generated from google/appengine/v1/audit_data.proto</summary>
  public static partial class AuditDataReflection {

    #region Descriptor
    /// <summary>File descriptor for google/appengine/v1/audit_data.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AuditDataReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CiRnb29nbGUvYXBwZW5naW5lL3YxL2F1ZGl0X2RhdGEucHJvdG8SE2dvb2ds",
            "ZS5hcHBlbmdpbmUudjEaI2dvb2dsZS9hcHBlbmdpbmUvdjEvYXBwZW5naW5l",
            "LnByb3RvGhxnb29nbGUvYXBpL2Fubm90YXRpb25zLnByb3RvIp0BCglBdWRp",
            "dERhdGESQgoOdXBkYXRlX3NlcnZpY2UYASABKAsyKC5nb29nbGUuYXBwZW5n",
            "aW5lLnYxLlVwZGF0ZVNlcnZpY2VNZXRob2RIABJCCg5jcmVhdGVfdmVyc2lv",
            "bhgCIAEoCzIoLmdvb2dsZS5hcHBlbmdpbmUudjEuQ3JlYXRlVmVyc2lvbk1l",
            "dGhvZEgAQggKBm1ldGhvZCJRChNVcGRhdGVTZXJ2aWNlTWV0aG9kEjoKB3Jl",
            "cXVlc3QYASABKAsyKS5nb29nbGUuYXBwZW5naW5lLnYxLlVwZGF0ZVNlcnZp",
            "Y2VSZXF1ZXN0IlEKE0NyZWF0ZVZlcnNpb25NZXRob2QSOgoHcmVxdWVzdBgB",
            "IAEoCzIpLmdvb2dsZS5hcHBlbmdpbmUudjEuQ3JlYXRlVmVyc2lvblJlcXVl",
            "c3RCwAEKF2NvbS5nb29nbGUuYXBwZW5naW5lLnYxQg5BdWRpdERhdGFQcm90",
            "b1ABWjxnb29nbGUuZ29sYW5nLm9yZy9nZW5wcm90by9nb29nbGVhcGlzL2Fw",
            "cGVuZ2luZS92MTthcHBlbmdpbmWqAhlHb29nbGUuQ2xvdWQuQXBwRW5naW5l",
            "LlYxygIZR29vZ2xlXENsb3VkXEFwcEVuZ2luZVxWMeoCHEdvb2dsZTo6Q2xv",
            "dWQ6OkFwcEVuZ2luZTo6VjFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Cloud.AppEngine.V1.AppengineReflection.Descriptor, global::Google.Api.AnnotationsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.AppEngine.V1.AuditData), global::Google.Cloud.AppEngine.V1.AuditData.Parser, new[]{ "UpdateService", "CreateVersion" }, new[]{ "Method" }, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.AppEngine.V1.UpdateServiceMethod), global::Google.Cloud.AppEngine.V1.UpdateServiceMethod.Parser, new[]{ "Request" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.AppEngine.V1.CreateVersionMethod), global::Google.Cloud.AppEngine.V1.CreateVersionMethod.Parser, new[]{ "Request" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// App Engine admin service audit log.
  /// </summary>
  public sealed partial class AuditData : pb::IMessage<AuditData>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AuditData> _parser = new pb::MessageParser<AuditData>(() => new AuditData());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AuditData> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.AppEngine.V1.AuditDataReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AuditData() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AuditData(AuditData other) : this() {
      switch (other.MethodCase) {
        case MethodOneofCase.UpdateService:
          UpdateService = other.UpdateService.Clone();
          break;
        case MethodOneofCase.CreateVersion:
          CreateVersion = other.CreateVersion.Clone();
          break;
      }

      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AuditData Clone() {
      return new AuditData(this);
    }

    /// <summary>Field number for the "update_service" field.</summary>
    public const int UpdateServiceFieldNumber = 1;
    /// <summary>
    /// Detailed information about UpdateService call.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.AppEngine.V1.UpdateServiceMethod UpdateService {
      get { return methodCase_ == MethodOneofCase.UpdateService ? (global::Google.Cloud.AppEngine.V1.UpdateServiceMethod) method_ : null; }
      set {
        method_ = value;
        methodCase_ = value == null ? MethodOneofCase.None : MethodOneofCase.UpdateService;
      }
    }

    /// <summary>Field number for the "create_version" field.</summary>
    public const int CreateVersionFieldNumber = 2;
    /// <summary>
    /// Detailed information about CreateVersion call.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.AppEngine.V1.CreateVersionMethod CreateVersion {
      get { return methodCase_ == MethodOneofCase.CreateVersion ? (global::Google.Cloud.AppEngine.V1.CreateVersionMethod) method_ : null; }
      set {
        method_ = value;
        methodCase_ = value == null ? MethodOneofCase.None : MethodOneofCase.CreateVersion;
      }
    }

    private object method_;
    /// <summary>Enum of possible cases for the "method" oneof.</summary>
    public enum MethodOneofCase {
      None = 0,
      UpdateService = 1,
      CreateVersion = 2,
    }
    private MethodOneofCase methodCase_ = MethodOneofCase.None;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public MethodOneofCase MethodCase {
      get { return methodCase_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void ClearMethod() {
      methodCase_ = MethodOneofCase.None;
      method_ = null;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AuditData);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AuditData other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(UpdateService, other.UpdateService)) return false;
      if (!object.Equals(CreateVersion, other.CreateVersion)) return false;
      if (MethodCase != other.MethodCase) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (methodCase_ == MethodOneofCase.UpdateService) hash ^= UpdateService.GetHashCode();
      if (methodCase_ == MethodOneofCase.CreateVersion) hash ^= CreateVersion.GetHashCode();
      hash ^= (int) methodCase_;
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (methodCase_ == MethodOneofCase.UpdateService) {
        output.WriteRawTag(10);
        output.WriteMessage(UpdateService);
      }
      if (methodCase_ == MethodOneofCase.CreateVersion) {
        output.WriteRawTag(18);
        output.WriteMessage(CreateVersion);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (methodCase_ == MethodOneofCase.UpdateService) {
        output.WriteRawTag(10);
        output.WriteMessage(UpdateService);
      }
      if (methodCase_ == MethodOneofCase.CreateVersion) {
        output.WriteRawTag(18);
        output.WriteMessage(CreateVersion);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (methodCase_ == MethodOneofCase.UpdateService) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(UpdateService);
      }
      if (methodCase_ == MethodOneofCase.CreateVersion) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(CreateVersion);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AuditData other) {
      if (other == null) {
        return;
      }
      switch (other.MethodCase) {
        case MethodOneofCase.UpdateService:
          if (UpdateService == null) {
            UpdateService = new global::Google.Cloud.AppEngine.V1.UpdateServiceMethod();
          }
          UpdateService.MergeFrom(other.UpdateService);
          break;
        case MethodOneofCase.CreateVersion:
          if (CreateVersion == null) {
            CreateVersion = new global::Google.Cloud.AppEngine.V1.CreateVersionMethod();
          }
          CreateVersion.MergeFrom(other.CreateVersion);
          break;
      }

      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            global::Google.Cloud.AppEngine.V1.UpdateServiceMethod subBuilder = new global::Google.Cloud.AppEngine.V1.UpdateServiceMethod();
            if (methodCase_ == MethodOneofCase.UpdateService) {
              subBuilder.MergeFrom(UpdateService);
            }
            input.ReadMessage(subBuilder);
            UpdateService = subBuilder;
            break;
          }
          case 18: {
            global::Google.Cloud.AppEngine.V1.CreateVersionMethod subBuilder = new global::Google.Cloud.AppEngine.V1.CreateVersionMethod();
            if (methodCase_ == MethodOneofCase.CreateVersion) {
              subBuilder.MergeFrom(CreateVersion);
            }
            input.ReadMessage(subBuilder);
            CreateVersion = subBuilder;
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            global::Google.Cloud.AppEngine.V1.UpdateServiceMethod subBuilder = new global::Google.Cloud.AppEngine.V1.UpdateServiceMethod();
            if (methodCase_ == MethodOneofCase.UpdateService) {
              subBuilder.MergeFrom(UpdateService);
            }
            input.ReadMessage(subBuilder);
            UpdateService = subBuilder;
            break;
          }
          case 18: {
            global::Google.Cloud.AppEngine.V1.CreateVersionMethod subBuilder = new global::Google.Cloud.AppEngine.V1.CreateVersionMethod();
            if (methodCase_ == MethodOneofCase.CreateVersion) {
              subBuilder.MergeFrom(CreateVersion);
            }
            input.ReadMessage(subBuilder);
            CreateVersion = subBuilder;
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// Detailed information about UpdateService call.
  /// </summary>
  public sealed partial class UpdateServiceMethod : pb::IMessage<UpdateServiceMethod>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<UpdateServiceMethod> _parser = new pb::MessageParser<UpdateServiceMethod>(() => new UpdateServiceMethod());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UpdateServiceMethod> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.AppEngine.V1.AuditDataReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateServiceMethod() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateServiceMethod(UpdateServiceMethod other) : this() {
      request_ = other.request_ != null ? other.request_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateServiceMethod Clone() {
      return new UpdateServiceMethod(this);
    }

    /// <summary>Field number for the "request" field.</summary>
    public const int RequestFieldNumber = 1;
    private global::Google.Cloud.AppEngine.V1.UpdateServiceRequest request_;
    /// <summary>
    /// Update service request.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.AppEngine.V1.UpdateServiceRequest Request {
      get { return request_; }
      set {
        request_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UpdateServiceMethod);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UpdateServiceMethod other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Request, other.Request)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (request_ != null) hash ^= Request.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (request_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Request);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (request_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Request);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (request_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Request);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UpdateServiceMethod other) {
      if (other == null) {
        return;
      }
      if (other.request_ != null) {
        if (request_ == null) {
          Request = new global::Google.Cloud.AppEngine.V1.UpdateServiceRequest();
        }
        Request.MergeFrom(other.Request);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (request_ == null) {
              Request = new global::Google.Cloud.AppEngine.V1.UpdateServiceRequest();
            }
            input.ReadMessage(Request);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (request_ == null) {
              Request = new global::Google.Cloud.AppEngine.V1.UpdateServiceRequest();
            }
            input.ReadMessage(Request);
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// Detailed information about CreateVersion call.
  /// </summary>
  public sealed partial class CreateVersionMethod : pb::IMessage<CreateVersionMethod>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<CreateVersionMethod> _parser = new pb::MessageParser<CreateVersionMethod>(() => new CreateVersionMethod());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CreateVersionMethod> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.AppEngine.V1.AuditDataReflection.Descriptor.MessageTypes[2]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateVersionMethod() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateVersionMethod(CreateVersionMethod other) : this() {
      request_ = other.request_ != null ? other.request_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CreateVersionMethod Clone() {
      return new CreateVersionMethod(this);
    }

    /// <summary>Field number for the "request" field.</summary>
    public const int RequestFieldNumber = 1;
    private global::Google.Cloud.AppEngine.V1.CreateVersionRequest request_;
    /// <summary>
    /// Create version request.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.AppEngine.V1.CreateVersionRequest Request {
      get { return request_; }
      set {
        request_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CreateVersionMethod);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CreateVersionMethod other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Request, other.Request)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (request_ != null) hash ^= Request.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (request_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Request);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (request_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Request);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (request_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Request);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CreateVersionMethod other) {
      if (other == null) {
        return;
      }
      if (other.request_ != null) {
        if (request_ == null) {
          Request = new global::Google.Cloud.AppEngine.V1.CreateVersionRequest();
        }
        Request.MergeFrom(other.Request);
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            if (request_ == null) {
              Request = new global::Google.Cloud.AppEngine.V1.CreateVersionRequest();
            }
            input.ReadMessage(Request);
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            if (request_ == null) {
              Request = new global::Google.Cloud.AppEngine.V1.CreateVersionRequest();
            }
            input.ReadMessage(Request);
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
