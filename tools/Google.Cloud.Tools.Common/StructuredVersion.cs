﻿// Copyright 2019 Google LLC.
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

using System;
using System.Text;
using System.Text.RegularExpressions;

namespace Google.Cloud.Tools.Common
{
    /// <summary>
    /// A structured SemVer version.
    /// </summary>
    public class StructuredVersion : IEquatable<StructuredVersion>, IComparable<StructuredVersion>
    {
        private static readonly Regex s_pattern = new Regex(@"^(?<major>[1-9]\d*)\.(?<minor>\d+)\.(?<patch>\d+)(\.(?<build>\d+))?(-(?<prerelease>.*))?$");

        public int Major { get; }
        public int Minor { get; }
        public int Patch { get; }
        public int? Build { get; }
        public string Prerelease { get; }

        private StructuredVersion(int major, int minor, int patch, int? build, string prerelease)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
            Build = build;
            Prerelease = prerelease;
        }

        public static StructuredVersion FromMajorMinorPatch(int major, int minor, int patch, string prerelease) =>
            new StructuredVersion(major, minor, patch, null, prerelease);

        public static StructuredVersion FromMajorMinorPatchBuild(int major, int minor, int patch, int? build, string prerelease) =>
            new StructuredVersion(major, minor, patch, build, prerelease);

        public static StructuredVersion FromString(string version)
        {
            var match = s_pattern.Match(version);
            if (!match.Success)
            {
                throw new ArgumentException($"Invalid version: {version}");
            }
            var major = int.Parse(match.Groups["major"].Value);
            var minor = int.Parse(match.Groups["minor"].Value);
            var patch = int.Parse(match.Groups["patch"].Value);
            var build = match.Groups["build"].Success ? int.Parse(match.Groups["build"].Value) : default(int?);
            var prerelease = match.Groups["prerelease"].Success ? match.Groups["prerelease"].Value : null;
            return new StructuredVersion(major, minor, patch, build, prerelease);
        }

        public int CompareTo(StructuredVersion other)
        {
            var majorDiff = Major.CompareTo(other.Major);
            if (majorDiff != 0)
            {
                return majorDiff;
            }
            var minorDiff = Minor.CompareTo(other.Minor);
            if (minorDiff != 0)
            {
                return minorDiff;
            }
            var patchDiff = Patch.CompareTo(other.Patch);
            if (patchDiff != 0)
            {
                return patchDiff;
            }
            if (Build != other.Build)
            {
                if (Build is null)
                {
                    return -1;
                }
                if (other.Build is null)
                {
                    return -1;
                }
                return Build.Value.CompareTo(other.Build.Value);
            }
            // Null comes *after* anything else when it comes to prereleases,
            // so we can't just use StringComparer.
            if (Prerelease is null && other.Prerelease is null)
            {
                return 0;
            }
            if (Prerelease is null)
            {
                return 1;
            }
            if (other.Prerelease is null)
            {
                return -1;
            }
            return string.CompareOrdinal(Prerelease, other.Prerelease);
        }

        public bool Equals(StructuredVersion other) =>
            other is object &&
            Major == other.Major &&
            Minor == other.Minor &&
            Patch == other.Patch &&
            Prerelease == other.Prerelease;

        // Not a brilliant hash code, but we're not expecting performance to
        // be an issue in our tools.
        public override int GetHashCode() =>
            Major ^ Minor ^ Patch ^ (Build ?? 0) ^ (Prerelease ?? "").GetHashCode();

        public override string ToString() => new StringBuilder($"{Major}.{Minor}.{Patch}")
            .Append(Build is null ? "" : $".{Build}")
            .Append(Prerelease is null ? "" : $"-{Prerelease}")
            .ToString();
    }
}
