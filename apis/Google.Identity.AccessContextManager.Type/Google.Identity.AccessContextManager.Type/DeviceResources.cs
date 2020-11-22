// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: google/identity/accesscontextmanager/type/device_resources.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Google.Identity.AccessContextManager.Type {

  /// <summary>Holder for reflection information generated from google/identity/accesscontextmanager/type/device_resources.proto</summary>
  public static partial class DeviceResourcesReflection {

    #region Descriptor
    /// <summary>File descriptor for google/identity/accesscontextmanager/type/device_resources.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static DeviceResourcesReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "CkBnb29nbGUvaWRlbnRpdHkvYWNjZXNzY29udGV4dG1hbmFnZXIvdHlwZS9k",
            "ZXZpY2VfcmVzb3VyY2VzLnByb3RvEilnb29nbGUuaWRlbnRpdHkuYWNjZXNz",
            "Y29udGV4dG1hbmFnZXIudHlwZRocZ29vZ2xlL2FwaS9hbm5vdGF0aW9ucy5w",
            "cm90bypwChZEZXZpY2VFbmNyeXB0aW9uU3RhdHVzEhoKFkVOQ1JZUFRJT05f",
            "VU5TUEVDSUZJRUQQABIaChZFTkNSWVBUSU9OX1VOU1VQUE9SVEVEEAESDwoL",
            "VU5FTkNSWVBURUQQAhINCglFTkNSWVBURUQQAyqCAQoGT3NUeXBlEhIKDk9T",
            "X1VOU1BFQ0lGSUVEEAASDwoLREVTS1RPUF9NQUMQARITCg9ERVNLVE9QX1dJ",
            "TkRPV1MQAhIRCg1ERVNLVE9QX0xJTlVYEAMSFQoRREVTS1RPUF9DSFJPTUVf",
            "T1MQBhILCgdBTkRST0lEEAQSBwoDSU9TEAUqVgoVRGV2aWNlTWFuYWdlbWVu",
            "dExldmVsEhoKFk1BTkFHRU1FTlRfVU5TUEVDSUZJRUQQABIICgROT05FEAES",
            "CQoFQkFTSUMQAhIMCghDT01QTEVURRADQpICCi1jb20uZ29vZ2xlLmlkZW50",
            "aXR5LmFjY2Vzc2NvbnRleHRtYW5hZ2VyLnR5cGVCCVR5cGVQcm90b1ABWk1n",
            "b29nbGUuZ29sYW5nLm9yZy9nZW5wcm90by9nb29nbGVhcGlzL2lkZW50aXR5",
            "L2FjY2Vzc2NvbnRleHRtYW5hZ2VyL3R5cGU7dHlwZaoCKUdvb2dsZS5JZGVu",
            "dGl0eS5BY2Nlc3NDb250ZXh0TWFuYWdlci5UeXBlygIpR29vZ2xlXElkZW50",
            "aXR5XEFjY2Vzc0NvbnRleHRNYW5hZ2VyXFR5cGXqAixHb29nbGU6OklkZW50",
            "aXR5OjpBY2Nlc3NDb250ZXh0TWFuYWdlcjo6VHlwZWIGcHJvdG8z"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { global::Google.Api.AnnotationsReflection.Descriptor, },
          new pbr::GeneratedClrTypeInfo(new[] {typeof(global::Google.Identity.AccessContextManager.Type.DeviceEncryptionStatus), typeof(global::Google.Identity.AccessContextManager.Type.OsType), typeof(global::Google.Identity.AccessContextManager.Type.DeviceManagementLevel), }, null, null));
    }
    #endregion

  }
  #region Enums
  /// <summary>
  /// The encryption state of the device.
  /// </summary>
  public enum DeviceEncryptionStatus {
    /// <summary>
    /// The encryption status of the device is not specified or not known.
    /// </summary>
    [pbr::OriginalName("ENCRYPTION_UNSPECIFIED")] EncryptionUnspecified = 0,
    /// <summary>
    /// The device does not support encryption.
    /// </summary>
    [pbr::OriginalName("ENCRYPTION_UNSUPPORTED")] EncryptionUnsupported = 1,
    /// <summary>
    /// The device supports encryption, but is currently unencrypted.
    /// </summary>
    [pbr::OriginalName("UNENCRYPTED")] Unencrypted = 2,
    /// <summary>
    /// The device is encrypted.
    /// </summary>
    [pbr::OriginalName("ENCRYPTED")] Encrypted = 3,
  }

  /// <summary>
  /// The operating system type of the device.
  /// Next id: 7
  /// </summary>
  public enum OsType {
    /// <summary>
    /// The operating system of the device is not specified or not known.
    /// </summary>
    [pbr::OriginalName("OS_UNSPECIFIED")] OsUnspecified = 0,
    /// <summary>
    /// A desktop Mac operating system.
    /// </summary>
    [pbr::OriginalName("DESKTOP_MAC")] DesktopMac = 1,
    /// <summary>
    /// A desktop Windows operating system.
    /// </summary>
    [pbr::OriginalName("DESKTOP_WINDOWS")] DesktopWindows = 2,
    /// <summary>
    /// A desktop Linux operating system.
    /// </summary>
    [pbr::OriginalName("DESKTOP_LINUX")] DesktopLinux = 3,
    /// <summary>
    /// A desktop ChromeOS operating system.
    /// </summary>
    [pbr::OriginalName("DESKTOP_CHROME_OS")] DesktopChromeOs = 6,
    /// <summary>
    /// An Android operating system.
    /// </summary>
    [pbr::OriginalName("ANDROID")] Android = 4,
    /// <summary>
    /// An iOS operating system.
    /// </summary>
    [pbr::OriginalName("IOS")] Ios = 5,
  }

  /// <summary>
  /// The degree to which the device is managed by the Cloud organization.
  /// </summary>
  public enum DeviceManagementLevel {
    /// <summary>
    /// The device's management level is not specified or not known.
    /// </summary>
    [pbr::OriginalName("MANAGEMENT_UNSPECIFIED")] ManagementUnspecified = 0,
    /// <summary>
    /// The device is not managed.
    /// </summary>
    [pbr::OriginalName("NONE")] None = 1,
    /// <summary>
    /// Basic management is enabled, which is generally limited to monitoring and
    /// wiping the corporate account.
    /// </summary>
    [pbr::OriginalName("BASIC")] Basic = 2,
    /// <summary>
    /// Complete device management. This includes more thorough monitoring and the
    /// ability to directly manage the device (such as remote wiping). This can be
    /// enabled through the Android Enterprise Platform.
    /// </summary>
    [pbr::OriginalName("COMPLETE")] Complete = 3,
  }

  #endregion

}

#endregion Designer generated code
