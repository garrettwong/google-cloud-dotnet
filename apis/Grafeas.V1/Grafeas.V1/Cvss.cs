// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: grafeas/v1/cvss.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Grafeas.V1 {

  /// <summary>Holder for reflection information generated from grafeas/v1/cvss.proto</summary>
  public static partial class CvssReflection {

    #region Descriptor
    /// <summary>File descriptor for grafeas/v1/cvss.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static CvssReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChVncmFmZWFzL3YxL2N2c3MucHJvdG8SCmdyYWZlYXMudjEixQkKBkNWU1N2",
            "MxISCgpiYXNlX3Njb3JlGAEgASgCEhwKFGV4cGxvaXRhYmlsaXR5X3Njb3Jl",
            "GAIgASgCEhQKDGltcGFjdF9zY29yZRgDIAEoAhI2Cg1hdHRhY2tfdmVjdG9y",
            "GAUgASgOMh8uZ3JhZmVhcy52MS5DVlNTdjMuQXR0YWNrVmVjdG9yEj4KEWF0",
            "dGFja19jb21wbGV4aXR5GAYgASgOMiMuZ3JhZmVhcy52MS5DVlNTdjMuQXR0",
            "YWNrQ29tcGxleGl0eRJCChNwcml2aWxlZ2VzX3JlcXVpcmVkGAcgASgOMiUu",
            "Z3JhZmVhcy52MS5DVlNTdjMuUHJpdmlsZWdlc1JlcXVpcmVkEjwKEHVzZXJf",
            "aW50ZXJhY3Rpb24YCCABKA4yIi5ncmFmZWFzLnYxLkNWU1N2My5Vc2VySW50",
            "ZXJhY3Rpb24SJwoFc2NvcGUYCSABKA4yGC5ncmFmZWFzLnYxLkNWU1N2My5T",
            "Y29wZRI5ChZjb25maWRlbnRpYWxpdHlfaW1wYWN0GAogASgOMhkuZ3JhZmVh",
            "cy52MS5DVlNTdjMuSW1wYWN0EjMKEGludGVncml0eV9pbXBhY3QYCyABKA4y",
            "GS5ncmFmZWFzLnYxLkNWU1N2My5JbXBhY3QSNgoTYXZhaWxhYmlsaXR5X2lt",
            "cGFjdBgMIAEoDjIZLmdyYWZlYXMudjEuQ1ZTU3YzLkltcGFjdCKZAQoMQXR0",
            "YWNrVmVjdG9yEh0KGUFUVEFDS19WRUNUT1JfVU5TUEVDSUZJRUQQABIZChVB",
            "VFRBQ0tfVkVDVE9SX05FVFdPUksQARIaChZBVFRBQ0tfVkVDVE9SX0FESkFD",
            "RU5UEAISFwoTQVRUQUNLX1ZFQ1RPUl9MT0NBTBADEhoKFkFUVEFDS19WRUNU",
            "T1JfUEhZU0lDQUwQBCJsChBBdHRhY2tDb21wbGV4aXR5EiEKHUFUVEFDS19D",
            "T01QTEVYSVRZX1VOU1BFQ0lGSUVEEAASGQoVQVRUQUNLX0NPTVBMRVhJVFlf",
            "TE9XEAESGgoWQVRUQUNLX0NPTVBMRVhJVFlfSElHSBACIpIBChJQcml2aWxl",
            "Z2VzUmVxdWlyZWQSIwofUFJJVklMRUdFU19SRVFVSVJFRF9VTlNQRUNJRklF",
            "RBAAEhwKGFBSSVZJTEVHRVNfUkVRVUlSRURfTk9ORRABEhsKF1BSSVZJTEVH",
            "RVNfUkVRVUlSRURfTE9XEAISHAoYUFJJVklMRUdFU19SRVFVSVJFRF9ISUdI",
            "EAMibQoPVXNlckludGVyYWN0aW9uEiAKHFVTRVJfSU5URVJBQ1RJT05fVU5T",
            "UEVDSUZJRUQQABIZChVVU0VSX0lOVEVSQUNUSU9OX05PTkUQARIdChlVU0VS",
            "X0lOVEVSQUNUSU9OX1JFUVVJUkVEEAIiRgoFU2NvcGUSFQoRU0NPUEVfVU5T",
            "UEVDSUZJRUQQABITCg9TQ09QRV9VTkNIQU5HRUQQARIRCg1TQ09QRV9DSEFO",
            "R0VEEAIiUgoGSW1wYWN0EhYKEklNUEFDVF9VTlNQRUNJRklFRBAAEg8KC0lN",
            "UEFDVF9ISUdIEAESDgoKSU1QQUNUX0xPVxACEg8KC0lNUEFDVF9OT05FEANC",
            "UQoNaW8uZ3JhZmVhcy52MVABWjhnb29nbGUuZ29sYW5nLm9yZy9nZW5wcm90",
            "by9nb29nbGVhcGlzL2dyYWZlYXMvdjE7Z3JhZmVhc6ICA0dSQWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Grafeas.V1.CVSSv3), global::Grafeas.V1.CVSSv3.Parser, new[]{ "BaseScore", "ExploitabilityScore", "ImpactScore", "AttackVector", "AttackComplexity", "PrivilegesRequired", "UserInteraction", "Scope", "ConfidentialityImpact", "IntegrityImpact", "AvailabilityImpact" }, null, new[]{ typeof(global::Grafeas.V1.CVSSv3.Types.AttackVector), typeof(global::Grafeas.V1.CVSSv3.Types.AttackComplexity), typeof(global::Grafeas.V1.CVSSv3.Types.PrivilegesRequired), typeof(global::Grafeas.V1.CVSSv3.Types.UserInteraction), typeof(global::Grafeas.V1.CVSSv3.Types.Scope), typeof(global::Grafeas.V1.CVSSv3.Types.Impact) }, null)
          }));
    }
    #endregion

  }
  #region Messages
  /// <summary>
  /// Common Vulnerability Scoring System version 3.
  /// For details, see https://www.first.org/cvss/specification-document
  /// </summary>
  public sealed partial class CVSSv3 : pb::IMessage<CVSSv3> {
    private static readonly pb::MessageParser<CVSSv3> _parser = new pb::MessageParser<CVSSv3>(() => new CVSSv3());
    private pb::UnknownFieldSet _unknownFields;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pb::MessageParser<CVSSv3> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Grafeas.V1.CvssReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CVSSv3() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CVSSv3(CVSSv3 other) : this() {
      baseScore_ = other.baseScore_;
      exploitabilityScore_ = other.exploitabilityScore_;
      impactScore_ = other.impactScore_;
      attackVector_ = other.attackVector_;
      attackComplexity_ = other.attackComplexity_;
      privilegesRequired_ = other.privilegesRequired_;
      userInteraction_ = other.userInteraction_;
      scope_ = other.scope_;
      confidentialityImpact_ = other.confidentialityImpact_;
      integrityImpact_ = other.integrityImpact_;
      availabilityImpact_ = other.availabilityImpact_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public CVSSv3 Clone() {
      return new CVSSv3(this);
    }

    /// <summary>Field number for the "base_score" field.</summary>
    public const int BaseScoreFieldNumber = 1;
    private float baseScore_;
    /// <summary>
    /// The base score is a function of the base metric scores.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float BaseScore {
      get { return baseScore_; }
      set {
        baseScore_ = value;
      }
    }

    /// <summary>Field number for the "exploitability_score" field.</summary>
    public const int ExploitabilityScoreFieldNumber = 2;
    private float exploitabilityScore_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ExploitabilityScore {
      get { return exploitabilityScore_; }
      set {
        exploitabilityScore_ = value;
      }
    }

    /// <summary>Field number for the "impact_score" field.</summary>
    public const int ImpactScoreFieldNumber = 3;
    private float impactScore_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public float ImpactScore {
      get { return impactScore_; }
      set {
        impactScore_ = value;
      }
    }

    /// <summary>Field number for the "attack_vector" field.</summary>
    public const int AttackVectorFieldNumber = 5;
    private global::Grafeas.V1.CVSSv3.Types.AttackVector attackVector_ = 0;
    /// <summary>
    /// Base Metrics
    /// Represents the intrinsic characteristics of a vulnerability that are
    /// constant over time and across user environments.
    /// </summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.AttackVector AttackVector {
      get { return attackVector_; }
      set {
        attackVector_ = value;
      }
    }

    /// <summary>Field number for the "attack_complexity" field.</summary>
    public const int AttackComplexityFieldNumber = 6;
    private global::Grafeas.V1.CVSSv3.Types.AttackComplexity attackComplexity_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.AttackComplexity AttackComplexity {
      get { return attackComplexity_; }
      set {
        attackComplexity_ = value;
      }
    }

    /// <summary>Field number for the "privileges_required" field.</summary>
    public const int PrivilegesRequiredFieldNumber = 7;
    private global::Grafeas.V1.CVSSv3.Types.PrivilegesRequired privilegesRequired_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.PrivilegesRequired PrivilegesRequired {
      get { return privilegesRequired_; }
      set {
        privilegesRequired_ = value;
      }
    }

    /// <summary>Field number for the "user_interaction" field.</summary>
    public const int UserInteractionFieldNumber = 8;
    private global::Grafeas.V1.CVSSv3.Types.UserInteraction userInteraction_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.UserInteraction UserInteraction {
      get { return userInteraction_; }
      set {
        userInteraction_ = value;
      }
    }

    /// <summary>Field number for the "scope" field.</summary>
    public const int ScopeFieldNumber = 9;
    private global::Grafeas.V1.CVSSv3.Types.Scope scope_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.Scope Scope {
      get { return scope_; }
      set {
        scope_ = value;
      }
    }

    /// <summary>Field number for the "confidentiality_impact" field.</summary>
    public const int ConfidentialityImpactFieldNumber = 10;
    private global::Grafeas.V1.CVSSv3.Types.Impact confidentialityImpact_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.Impact ConfidentialityImpact {
      get { return confidentialityImpact_; }
      set {
        confidentialityImpact_ = value;
      }
    }

    /// <summary>Field number for the "integrity_impact" field.</summary>
    public const int IntegrityImpactFieldNumber = 11;
    private global::Grafeas.V1.CVSSv3.Types.Impact integrityImpact_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.Impact IntegrityImpact {
      get { return integrityImpact_; }
      set {
        integrityImpact_ = value;
      }
    }

    /// <summary>Field number for the "availability_impact" field.</summary>
    public const int AvailabilityImpactFieldNumber = 12;
    private global::Grafeas.V1.CVSSv3.Types.Impact availabilityImpact_ = 0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public global::Grafeas.V1.CVSSv3.Types.Impact AvailabilityImpact {
      get { return availabilityImpact_; }
      set {
        availabilityImpact_ = value;
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override bool Equals(object other) {
      return Equals(other as CVSSv3);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public bool Equals(CVSSv3 other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(BaseScore, other.BaseScore)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ExploitabilityScore, other.ExploitabilityScore)) return false;
      if (!pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.Equals(ImpactScore, other.ImpactScore)) return false;
      if (AttackVector != other.AttackVector) return false;
      if (AttackComplexity != other.AttackComplexity) return false;
      if (PrivilegesRequired != other.PrivilegesRequired) return false;
      if (UserInteraction != other.UserInteraction) return false;
      if (Scope != other.Scope) return false;
      if (ConfidentialityImpact != other.ConfidentialityImpact) return false;
      if (IntegrityImpact != other.IntegrityImpact) return false;
      if (AvailabilityImpact != other.AvailabilityImpact) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public override int GetHashCode() {
      int hash = 1;
      if (BaseScore != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(BaseScore);
      if (ExploitabilityScore != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ExploitabilityScore);
      if (ImpactScore != 0F) hash ^= pbc::ProtobufEqualityComparers.BitwiseSingleEqualityComparer.GetHashCode(ImpactScore);
      if (AttackVector != 0) hash ^= AttackVector.GetHashCode();
      if (AttackComplexity != 0) hash ^= AttackComplexity.GetHashCode();
      if (PrivilegesRequired != 0) hash ^= PrivilegesRequired.GetHashCode();
      if (UserInteraction != 0) hash ^= UserInteraction.GetHashCode();
      if (Scope != 0) hash ^= Scope.GetHashCode();
      if (ConfidentialityImpact != 0) hash ^= ConfidentialityImpact.GetHashCode();
      if (IntegrityImpact != 0) hash ^= IntegrityImpact.GetHashCode();
      if (AvailabilityImpact != 0) hash ^= AvailabilityImpact.GetHashCode();
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
      if (BaseScore != 0F) {
        output.WriteRawTag(13);
        output.WriteFloat(BaseScore);
      }
      if (ExploitabilityScore != 0F) {
        output.WriteRawTag(21);
        output.WriteFloat(ExploitabilityScore);
      }
      if (ImpactScore != 0F) {
        output.WriteRawTag(29);
        output.WriteFloat(ImpactScore);
      }
      if (AttackVector != 0) {
        output.WriteRawTag(40);
        output.WriteEnum((int) AttackVector);
      }
      if (AttackComplexity != 0) {
        output.WriteRawTag(48);
        output.WriteEnum((int) AttackComplexity);
      }
      if (PrivilegesRequired != 0) {
        output.WriteRawTag(56);
        output.WriteEnum((int) PrivilegesRequired);
      }
      if (UserInteraction != 0) {
        output.WriteRawTag(64);
        output.WriteEnum((int) UserInteraction);
      }
      if (Scope != 0) {
        output.WriteRawTag(72);
        output.WriteEnum((int) Scope);
      }
      if (ConfidentialityImpact != 0) {
        output.WriteRawTag(80);
        output.WriteEnum((int) ConfidentialityImpact);
      }
      if (IntegrityImpact != 0) {
        output.WriteRawTag(88);
        output.WriteEnum((int) IntegrityImpact);
      }
      if (AvailabilityImpact != 0) {
        output.WriteRawTag(96);
        output.WriteEnum((int) AvailabilityImpact);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public int CalculateSize() {
      int size = 0;
      if (BaseScore != 0F) {
        size += 1 + 4;
      }
      if (ExploitabilityScore != 0F) {
        size += 1 + 4;
      }
      if (ImpactScore != 0F) {
        size += 1 + 4;
      }
      if (AttackVector != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AttackVector);
      }
      if (AttackComplexity != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AttackComplexity);
      }
      if (PrivilegesRequired != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) PrivilegesRequired);
      }
      if (UserInteraction != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) UserInteraction);
      }
      if (Scope != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) Scope);
      }
      if (ConfidentialityImpact != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) ConfidentialityImpact);
      }
      if (IntegrityImpact != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) IntegrityImpact);
      }
      if (AvailabilityImpact != 0) {
        size += 1 + pb::CodedOutputStream.ComputeEnumSize((int) AvailabilityImpact);
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(CVSSv3 other) {
      if (other == null) {
        return;
      }
      if (other.BaseScore != 0F) {
        BaseScore = other.BaseScore;
      }
      if (other.ExploitabilityScore != 0F) {
        ExploitabilityScore = other.ExploitabilityScore;
      }
      if (other.ImpactScore != 0F) {
        ImpactScore = other.ImpactScore;
      }
      if (other.AttackVector != 0) {
        AttackVector = other.AttackVector;
      }
      if (other.AttackComplexity != 0) {
        AttackComplexity = other.AttackComplexity;
      }
      if (other.PrivilegesRequired != 0) {
        PrivilegesRequired = other.PrivilegesRequired;
      }
      if (other.UserInteraction != 0) {
        UserInteraction = other.UserInteraction;
      }
      if (other.Scope != 0) {
        Scope = other.Scope;
      }
      if (other.ConfidentialityImpact != 0) {
        ConfidentialityImpact = other.ConfidentialityImpact;
      }
      if (other.IntegrityImpact != 0) {
        IntegrityImpact = other.IntegrityImpact;
      }
      if (other.AvailabilityImpact != 0) {
        AvailabilityImpact = other.AvailabilityImpact;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public void MergeFrom(pb::CodedInputStream input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 13: {
            BaseScore = input.ReadFloat();
            break;
          }
          case 21: {
            ExploitabilityScore = input.ReadFloat();
            break;
          }
          case 29: {
            ImpactScore = input.ReadFloat();
            break;
          }
          case 40: {
            AttackVector = (global::Grafeas.V1.CVSSv3.Types.AttackVector) input.ReadEnum();
            break;
          }
          case 48: {
            AttackComplexity = (global::Grafeas.V1.CVSSv3.Types.AttackComplexity) input.ReadEnum();
            break;
          }
          case 56: {
            PrivilegesRequired = (global::Grafeas.V1.CVSSv3.Types.PrivilegesRequired) input.ReadEnum();
            break;
          }
          case 64: {
            UserInteraction = (global::Grafeas.V1.CVSSv3.Types.UserInteraction) input.ReadEnum();
            break;
          }
          case 72: {
            Scope = (global::Grafeas.V1.CVSSv3.Types.Scope) input.ReadEnum();
            break;
          }
          case 80: {
            ConfidentialityImpact = (global::Grafeas.V1.CVSSv3.Types.Impact) input.ReadEnum();
            break;
          }
          case 88: {
            IntegrityImpact = (global::Grafeas.V1.CVSSv3.Types.Impact) input.ReadEnum();
            break;
          }
          case 96: {
            AvailabilityImpact = (global::Grafeas.V1.CVSSv3.Types.Impact) input.ReadEnum();
            break;
          }
        }
      }
    }

    #region Nested types
    /// <summary>Container for nested types declared in the CVSSv3 message type.</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    public static partial class Types {
      public enum AttackVector {
        [pbr::OriginalName("ATTACK_VECTOR_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("ATTACK_VECTOR_NETWORK")] Network = 1,
        [pbr::OriginalName("ATTACK_VECTOR_ADJACENT")] Adjacent = 2,
        [pbr::OriginalName("ATTACK_VECTOR_LOCAL")] Local = 3,
        [pbr::OriginalName("ATTACK_VECTOR_PHYSICAL")] Physical = 4,
      }

      public enum AttackComplexity {
        [pbr::OriginalName("ATTACK_COMPLEXITY_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("ATTACK_COMPLEXITY_LOW")] Low = 1,
        [pbr::OriginalName("ATTACK_COMPLEXITY_HIGH")] High = 2,
      }

      public enum PrivilegesRequired {
        [pbr::OriginalName("PRIVILEGES_REQUIRED_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("PRIVILEGES_REQUIRED_NONE")] None = 1,
        [pbr::OriginalName("PRIVILEGES_REQUIRED_LOW")] Low = 2,
        [pbr::OriginalName("PRIVILEGES_REQUIRED_HIGH")] High = 3,
      }

      public enum UserInteraction {
        [pbr::OriginalName("USER_INTERACTION_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("USER_INTERACTION_NONE")] None = 1,
        [pbr::OriginalName("USER_INTERACTION_REQUIRED")] Required = 2,
      }

      public enum Scope {
        [pbr::OriginalName("SCOPE_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("SCOPE_UNCHANGED")] Unchanged = 1,
        [pbr::OriginalName("SCOPE_CHANGED")] Changed = 2,
      }

      public enum Impact {
        [pbr::OriginalName("IMPACT_UNSPECIFIED")] Unspecified = 0,
        [pbr::OriginalName("IMPACT_HIGH")] High = 1,
        [pbr::OriginalName("IMPACT_LOW")] Low = 2,
        [pbr::OriginalName("IMPACT_NONE")] None = 3,
      }

    }
    #endregion

  }

  #endregion

}

#endregion Designer generated code
