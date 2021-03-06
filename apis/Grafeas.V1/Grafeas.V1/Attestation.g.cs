// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: grafeas/v1/attestation.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Grafeas.V1 {

  /// <summary>Holder for reflection information generated from grafeas/v1/attestation.proto</summary>
  public static partial class AttestationReflection {

    #region Descriptor
    /// <summary>File descriptor for grafeas/v1/attestation.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static AttestationReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChxncmFmZWFzL3YxL2F0dGVzdGF0aW9uLnByb3RvEgpncmFmZWFzLnYxGhdn",
            "cmFmZWFzL3YxL2NvbW1vbi5wcm90byJmCg9BdHRlc3RhdGlvbk5vdGUSLgoE",
            "aGludBgBIAEoCzIgLmdyYWZlYXMudjEuQXR0ZXN0YXRpb25Ob3RlLkhpbnQa",
            "IwoESGludBIbChNodW1hbl9yZWFkYWJsZV9uYW1lGAEgASgJIl4KFUF0dGVz",
            "dGF0aW9uT2NjdXJyZW5jZRIaChJzZXJpYWxpemVkX3BheWxvYWQYASABKAwS",
            "KQoKc2lnbmF0dXJlcxgCIAMoCzIVLmdyYWZlYXMudjEuU2lnbmF0dXJlQlEK",
            "DWlvLmdyYWZlYXMudjFQAVo4Z29vZ2xlLmdvbGFuZy5vcmcvZ2VucHJvdG8v",
            "Z29vZ2xlYXBpcy9ncmFmZWFzL3YxO2dyYWZlYXOiAgNHUkFiBnByb3RvMw=="));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Grafeas.V1.CommonReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Grafeas.V1.AttestationNote), global::Grafeas.V1.AttestationNote.Parser, new[]{ "Hint" }, null, null, null, new pbr::GeneratedClrTypeInfo[] { new pbr::GeneratedClrTypeInfo(typeof(global::Grafeas.V1.AttestationNote.Types.Hint), global::Grafeas.V1.AttestationNote.Types.Hint.Parser, new[]{ "HumanReadableName" }, null, null, null, null)}),
            new pbr::GeneratedClrTypeInfo(typeof(global::Grafeas.V1.AttestationOccurrence), global::Grafeas.V1.AttestationOccurrence.Parser, new[]{ "SerializedPayload", "Signatures" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Note kind that represents a logical attestation "role" or "authority". For
  /// example, an organization might have one `Authority` for "QA" and one for
  /// "build". This note is intended to act strictly as a grouping mechanism for
  /// the attached occurrences (Attestations). This grouping mechanism also
  /// provides a security boundary, since IAM ACLs gate the ability for a principle
  /// to attach an occurrence to a given note. It also provides a single point of
  /// lookup to find all attached attestation occurrences, even if they don't all
  /// live in the same project.
  /// </summary>
  public sealed partial class AttestationNote : pb::IMessage<AttestationNote>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AttestationNote> _parser = new pb::MessageParser<AttestationNote>(() => new AttestationNote());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AttestationNote> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Grafeas.V1.AttestationReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationNote() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationNote(AttestationNote other) : this() {
      hint_ = other.hint_ != null ? other.hint_.Clone() : null;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationNote Clone() {
      return new AttestationNote(this);
    }

    /// <summary>Field number for the "hint" field.</summary>
    public const int HintFieldNumber = 1;
    private global::Grafeas.V1.AttestationNote.Types.Hint hint_;
    /// <summary>
    /// Hint hints at the purpose of the attestation authority.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.AttestationNote.Types.Hint Hint {
      get { return hint_; }
      set {
        hint_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AttestationNote);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AttestationNote other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!object.Equals(Hint, other.Hint)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (hint_ != null) hash ^= Hint.GetHashCode();
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
      if (hint_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Hint);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (hint_ != null) {
        output.WriteRawTag(10);
        output.WriteMessage(Hint);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (hint_ != null) {
        size += 1 + pb::CodedOutputStream.ComputeMessageSize(Hint);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AttestationNote other) {
      if (other == null) {
        return;
      }
      if (other.hint_ != null) {
        if (hint_ == null) {
          Hint = new global::Grafeas.V1.AttestationNote.Types.Hint();
        }
        Hint.MergeFrom(other.Hint);
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
            if (hint_ == null) {
              Hint = new global::Grafeas.V1.AttestationNote.Types.Hint();
            }
            input.ReadMessage(Hint);
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
            if (hint_ == null) {
              Hint = new global::Grafeas.V1.AttestationNote.Types.Hint();
            }
            input.ReadMessage(Hint);
            break;
          }
        }
      }
    }
    #endif

    #region Nested types
    /// <summary>Container for nested types declared in the AttestationNote message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      /// <summary>
      /// This submessage provides human-readable hints about the purpose of the
      /// authority. Because the name of a note acts as its resource reference, it is
      /// important to disambiguate the canonical name of the Note (which might be a
      /// UUID for security purposes) from "readable" names more suitable for debug
      /// output. Note that these hints should not be used to look up authorities in
      /// security sensitive contexts, such as when looking up attestations to
      /// verify.
      /// </summary>
      public sealed partial class Hint : pb::IMessage<Hint>
      #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
          , pb::IBufferMessage
      #endif
      {
        private static readonly pb::MessageParser<Hint> _parser = new pb::MessageParser<Hint>(() => new Hint());
        private pb::UnknownFieldSet _unknownFields;
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pb::MessageParser<Hint> Parser { get { return _parser; } }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public static pbr::MessageDescriptor Descriptor {
          get { return global::Grafeas.V1.AttestationNote.Descriptor.NestedTypes[0]; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        pbr::MessageDescriptor pb::IMessage.Descriptor {
          get { return Descriptor; }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Hint() {
          OnConstruction();
        }

        partial void OnConstruction();

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Hint(Hint other) : this() {
          humanReadableName_ = other.humanReadableName_;
          _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public Hint Clone() {
          return new Hint(this);
        }

        /// <summary>Field number for the "human_readable_name" field.</summary>
        public const int HumanReadableNameFieldNumber = 1;
        private string humanReadableName_ = "";
        /// <summary>
        /// Required. The human readable name of this attestation authority, for
        /// example "qa".
        /// </summary>
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public string HumanReadableName {
          get { return humanReadableName_; }
          set {
            humanReadableName_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
          }
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override bool Equals(object other) {
          return Equals(other as Hint);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public bool Equals(Hint other) {
          if (ReferenceEquals(other, null)) {
            return false;
          }
          if (ReferenceEquals(other, this)) {
            return true;
          }
          if (HumanReadableName != other.HumanReadableName) return false;
          return Equals(_unknownFields, other._unknownFields);
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public override int GetHashCode() {
          int hash = 1;
          if (HumanReadableName.Length != 0) hash ^= HumanReadableName.GetHashCode();
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
          if (HumanReadableName.Length != 0) {
            output.WriteRawTag(10);
            output.WriteString(HumanReadableName);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(output);
          }
        #endif
        }

        #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
          if (HumanReadableName.Length != 0) {
            output.WriteRawTag(10);
            output.WriteString(HumanReadableName);
          }
          if (_unknownFields != null) {
            _unknownFields.WriteTo(ref output);
          }
        }
        #endif

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public int CalculateSize() {
          int size = 0;
          if (HumanReadableName.Length != 0) {
            size += 1 + pb::CodedOutputStream.ComputeStringSize(HumanReadableName);
          }
          if (_unknownFields != null) {
            size += _unknownFields.CalculateSize();
          }
          return size;
        }

        [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
        public void MergeFrom(Hint other) {
          if (other == null) {
            return;
          }
          if (other.HumanReadableName.Length != 0) {
            HumanReadableName = other.HumanReadableName;
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
                HumanReadableName = input.ReadString();
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
                HumanReadableName = input.ReadString();
                break;
              }
            }
          }
        }
        #endif

      }

    }
    #endregion

  }

  /// <summary>
  /// Occurrence that represents a single "attestation". The authenticity of an
  /// attestation can be verified using the attached signature. If the verifier
  /// trusts the public key of the signer, then verifying the signature is
  /// sufficient to establish trust. In this circumstance, the authority to which
  /// this attestation is attached is primarily useful for lookup (how to find
  /// this attestation if you already know the authority and artifact to be
  /// verified) and intent (for which authority this attestation was intended to
  /// sign.
  /// </summary>
  public sealed partial class AttestationOccurrence : pb::IMessage<AttestationOccurrence>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<AttestationOccurrence> _parser = new pb::MessageParser<AttestationOccurrence>(() => new AttestationOccurrence());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<AttestationOccurrence> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Grafeas.V1.AttestationReflection.Descriptor.MessageTypes[1]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationOccurrence() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationOccurrence(AttestationOccurrence other) : this() {
      serializedPayload_ = other.serializedPayload_;
      signatures_ = other.signatures_.Clone();
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public AttestationOccurrence Clone() {
      return new AttestationOccurrence(this);
    }

    /// <summary>Field number for the "serialized_payload" field.</summary>
    public const int SerializedPayloadFieldNumber = 1;
    private pb::ByteString serializedPayload_ = pb::ByteString.Empty;
    /// <summary>
    /// Required. The serialized payload that is verified by one or more
    /// `signatures`.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pb::ByteString SerializedPayload {
      get { return serializedPayload_; }
      set {
        serializedPayload_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }

    /// <summary>Field number for the "signatures" field.</summary>
    public const int SignaturesFieldNumber = 2;
    private static readonly pb::FieldCodec<global::Grafeas.V1.Signature> _repeated_signatures_codec
        = pb::FieldCodec.ForMessage(18, global::Grafeas.V1.Signature.Parser);
    private readonly pbc::RepeatedField<global::Grafeas.V1.Signature> signatures_ = new pbc::RepeatedField<global::Grafeas.V1.Signature>();
    /// <summary>
    /// One or more signatures over `serialized_payload`.  Verifier implementations
    /// should consider this attestation message verified if at least one
    /// `signature` verifies `serialized_payload`.  See `Signature` in common.proto
    /// for more details on signature structure and verification.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public pbc::RepeatedField<global::Grafeas.V1.Signature> Signatures {
      get { return signatures_; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as AttestationOccurrence);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(AttestationOccurrence other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (SerializedPayload != other.SerializedPayload) return false;
      if(!signatures_.Equals(other.signatures_)) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (SerializedPayload.Length != 0) hash ^= SerializedPayload.GetHashCode();
      hash ^= signatures_.GetHashCode();
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
      if (SerializedPayload.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(SerializedPayload);
      }
      signatures_.WriteTo(output, _repeated_signatures_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (SerializedPayload.Length != 0) {
        output.WriteRawTag(10);
        output.WriteBytes(SerializedPayload);
      }
      signatures_.WriteTo(ref output, _repeated_signatures_codec);
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (SerializedPayload.Length != 0) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(SerializedPayload);
      }
      size += signatures_.CalculateSize(_repeated_signatures_codec);
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(AttestationOccurrence other) {
      if (other == null) {
        return;
      }
      if (other.SerializedPayload.Length != 0) {
        SerializedPayload = other.SerializedPayload;
      }
      signatures_.Add(other.signatures_);
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
            SerializedPayload = input.ReadBytes();
            break;
          }
          case 18: {
            signatures_.AddEntriesFrom(input, _repeated_signatures_codec);
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
            SerializedPayload = input.ReadBytes();
            break;
          }
          case 18: {
            signatures_.AddEntriesFrom(ref input, _repeated_signatures_codec);
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
