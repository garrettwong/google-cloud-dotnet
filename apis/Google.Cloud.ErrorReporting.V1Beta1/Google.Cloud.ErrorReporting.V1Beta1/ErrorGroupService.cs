// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/devtools/clouderrorreporting/v1beta1/error_group_service.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Cloud.ErrorReporting.V1Beta1 {

  /// <summary>Holder for reflection information generated from google/devtools/clouderrorreporting/v1beta1/error_group_service.proto</summary>
  public static partial class ErrorGroupServiceReflection {

    #region Descriptor
    /// <summary>File descriptor for google/devtools/clouderrorreporting/v1beta1/error_group_service.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static ErrorGroupServiceReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CkVnb29nbGUvZGV2dG9vbHMvY2xvdWRlcnJvcnJlcG9ydGluZy92MWJldGEx",
            "L2Vycm9yX2dyb3VwX3NlcnZpY2UucHJvdG8SK2dvb2dsZS5kZXZ0b29scy5j",
            "bG91ZGVycm9ycmVwb3J0aW5nLnYxYmV0YTEaHGdvb2dsZS9hcGkvYW5ub3Rh",
            "dGlvbnMucHJvdG8aF2dvb2dsZS9hcGkvY2xpZW50LnByb3RvGh9nb29nbGUv",
            "YXBpL2ZpZWxkX2JlaGF2aW9yLnByb3RvGhlnb29nbGUvYXBpL3Jlc291cmNl",
            "LnByb3RvGjhnb29nbGUvZGV2dG9vbHMvY2xvdWRlcnJvcnJlcG9ydGluZy92",
            "MWJldGExL2NvbW1vbi5wcm90byJcCg9HZXRHcm91cFJlcXVlc3QSSQoKZ3Jv",
            "dXBfbmFtZRgBIAEoCUI14EEC+kEvCi1jbG91ZGVycm9ycmVwb3J0aW5nLmdv",
            "b2dsZWFwaXMuY29tL0Vycm9yR3JvdXAiYQoSVXBkYXRlR3JvdXBSZXF1ZXN0",
            "EksKBWdyb3VwGAEgASgLMjcuZ29vZ2xlLmRldnRvb2xzLmNsb3VkZXJyb3Jy",
            "ZXBvcnRpbmcudjFiZXRhMS5FcnJvckdyb3VwQgPgQQIy+wMKEUVycm9yR3Jv",
            "dXBTZXJ2aWNlEsEBCghHZXRHcm91cBI8Lmdvb2dsZS5kZXZ0b29scy5jbG91",
            "ZGVycm9ycmVwb3J0aW5nLnYxYmV0YTEuR2V0R3JvdXBSZXF1ZXN0GjcuZ29v",
            "Z2xlLmRldnRvb2xzLmNsb3VkZXJyb3JyZXBvcnRpbmcudjFiZXRhMS5FcnJv",
            "ckdyb3VwIj6C0+STAisSKS92MWJldGExL3tncm91cF9uYW1lPXByb2plY3Rz",
            "LyovZ3JvdXBzLyp92kEKZ3JvdXBfbmFtZRLJAQoLVXBkYXRlR3JvdXASPy5n",
            "b29nbGUuZGV2dG9vbHMuY2xvdWRlcnJvcnJlcG9ydGluZy52MWJldGExLlVw",
            "ZGF0ZUdyb3VwUmVxdWVzdBo3Lmdvb2dsZS5kZXZ0b29scy5jbG91ZGVycm9y",
            "cmVwb3J0aW5nLnYxYmV0YTEuRXJyb3JHcm91cCJAgtPkkwIyGikvdjFiZXRh",
            "MS97Z3JvdXAubmFtZT1wcm9qZWN0cy8qL2dyb3Vwcy8qfToFZ3JvdXDaQQVn",
            "cm91cBpWykEiY2xvdWRlcnJvcnJlcG9ydGluZy5nb29nbGVhcGlzLmNvbdJB",
            "Lmh0dHBzOi8vd3d3Lmdvb2dsZWFwaXMuY29tL2F1dGgvY2xvdWQtcGxhdGZv",
            "cm1CowIKL2NvbS5nb29nbGUuZGV2dG9vbHMuY2xvdWRlcnJvcnJlcG9ydGlu",
            "Zy52MWJldGExQhZFcnJvckdyb3VwU2VydmljZVByb3RvUAFaXmdvb2dsZS5n",
            "b2xhbmcub3JnL2dlbnByb3RvL2dvb2dsZWFwaXMvZGV2dG9vbHMvY2xvdWRl",
            "cnJvcnJlcG9ydGluZy92MWJldGExO2Nsb3VkZXJyb3JyZXBvcnRpbmf4AQGq",
            "AiNHb29nbGUuQ2xvdWQuRXJyb3JSZXBvcnRpbmcuVjFCZXRhMcoCI0dvb2ds",
            "ZVxDbG91ZFxFcnJvclJlcG9ydGluZ1xWMWJldGEx6gImR29vZ2xlOjpDbG91",
            "ZDo6RXJyb3JSZXBvcnRpbmc6OlYxYmV0YTFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Api.AnnotationsReflection.Descriptor, global::Google.Api.ClientReflection.Descriptor, global::Google.Api.FieldBehaviorReflection.Descriptor, global::Google.Api.ResourceReflection.Descriptor, global::Google.Cloud.ErrorReporting.V1Beta1.CommonReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.ErrorReporting.V1Beta1.GetGroupRequest), global::Google.Cloud.ErrorReporting.V1Beta1.GetGroupRequest.Parser, new[]{ "GroupName" }, null, null, null, null),
            new pbr::GeneratedClrTypeInfo(typeof(global::Google.Cloud.ErrorReporting.V1Beta1.UpdateGroupRequest), global::Google.Cloud.ErrorReporting.V1Beta1.UpdateGroupRequest.Parser, new[]{ "Group" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// A request to return an individual group.
  /// </summary>
  public sealed partial class GetGroupRequest : pb::IMessage<GetGroupRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<GetGroupRequest> _parser = new pb::MessageParser<GetGroupRequest>(() => new GetGroupRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<GetGroupRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroupServiceReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetGroupRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetGroupRequest(GetGroupRequest other) : this() {
      groupName_ = other.groupName_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public GetGroupRequest Clone() {
      return new GetGroupRequest(this);
    }

    /// <summary>Field number for the "group_name" field.</summary>
    public const int GroupNameFieldNumber = 1;
    private string groupName_ = "";
    /// <summary>
    /// The group resource name. Written as
    /// `projects/{projectID}/groups/{group_name}`. Call
    /// [`groupStats.list`](https://cloud.google.com/error-reporting/reference/rest/v1beta1/projects.groupStats/list)
    /// to return a list of groups belonging to this project.
    ///
    /// Example: `projects/my-project-123/groups/my-group`
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public string GroupName {
      get { return groupName_; }
      set {
        groupName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as GetGroupRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(GetGroupRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (GroupName != other.GroupName) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (GroupName.Length != 0) hash ^= GroupName.GetHashCode();
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
      if (GroupName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(GroupName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (GroupName.Length != 0) {
        output.WriteRawTag(10);
        output.WriteString(GroupName);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (GroupName.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(GroupName);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(GetGroupRequest other) {
      if (other == null) {
        return;
      }
      if (other.GroupName.Length != 0) {
        GroupName = other.GroupName;
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
            GroupName = input.ReadString();
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
            GroupName = input.ReadString();
            break;
          }
        }
      }
    }
    #endif

  }

  /// <summary>
  /// A request to replace the existing data for the given group.
  /// </summary>
  public sealed partial class UpdateGroupRequest : pb::IMessage<UpdateGroupRequest>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<UpdateGroupRequest> _parser = new pb::MessageParser<UpdateGroupRequest>(() => new UpdateGroupRequest());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<UpdateGroupRequest> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroupServiceReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateGroupRequest() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateGroupRequest(UpdateGroupRequest other) : this() {
      group_ = other.group_ != null ? other.group_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public UpdateGroupRequest Clone() {
      return new UpdateGroupRequest(this);
    }

    /// <summary>Field number for the "group" field.</summary>
    public const int GroupFieldNumber = 1;
    private global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroup group_;
    /// <summary>
    /// Required. The group which replaces the resource on the server.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroup Group {
      get { return group_; }
      set {
        group_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as UpdateGroupRequest);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(UpdateGroupRequest other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Group, other.Group)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (group_ != null) hash ^= Group.GetHashCode();
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
      if (group_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Group);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (group_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Group);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (group_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Group);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(UpdateGroupRequest other) {
      if (other == null) {
        return;
      }
      if (other.group_ != null) {
        if (group_ == null) {
          Group = new global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroup();
        }
        Group.MergeFrom(other.Group);
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
            if (group_ == null) {
              Group = new global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroup();
            }
            input.ReadMessage(Group);
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
            if (group_ == null) {
              Group = new global::Google.Cloud.ErrorReporting.V1Beta1.ErrorGroup();
            }
            input.ReadMessage(Group);
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
