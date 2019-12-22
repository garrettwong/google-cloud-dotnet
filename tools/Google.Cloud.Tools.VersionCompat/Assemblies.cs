﻿// Copyright 2019 Google LLC
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

using Google.Cloud.Tools.VersionCompat.CecilUtils;
using Google.Cloud.Tools.VersionCompat.Detectors;
using Mono.Cecil;
using SharpCompress.Archives.Zip;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Google.Cloud.Tools.VersionCompat
{
    /// <summary>
    /// Utility methods for loading and comparing assemblies.
    /// </summary>
    public static class Assemblies
    {
        private static DiffResult Compare(IReadOnlyList<TypeDefinition> olderTypes, IReadOnlyList<TypeDefinition> newerTypes)
        {
            var oWithNested = olderTypes.WithNested().ToImmutableList();
            var nWithNested = newerTypes.WithNested().ToImmutableList();

            var diffs = TopLevel.Diffs(oWithNested, nWithNested).ToImmutableList();

            return new DiffResult(diffs);
        }

        public static DiffResult Compare(AssemblyDefinition older, AssemblyDefinition newer, string testNamespace)
        {
            if (testNamespace == null)
            {
                return Compare(older.Modules.SelectMany(x => x.Types).ToList(), newer.Modules.SelectMany(x => x.Types).ToList());
            }
            else
            {
                var olderTypesList = new List<TypeDefinition>();
                var newerTypesList = new List<TypeDefinition>();
                var nsOlder = $"{testNamespace}.A.";
                var nsNewer = $"{testNamespace}.B.";
                foreach (var type in older.Modules.SelectMany(x => x.Types).Where(x => x.FullName.StartsWith(nsOlder)))
                {
                    type.Namespace = type.Namespace.Replace($"{testNamespace}.A", testNamespace);
                    olderTypesList.Add(type);
                }
                foreach (var type in newer.Modules.SelectMany(x => x.Types).Where(x => x.FullName.StartsWith(nsNewer)))
                {
                    type.Namespace = type.Namespace.Replace($"{testNamespace}.B", testNamespace);
                    newerTypesList.Add(type);
                }
                if (!olderTypesList.Any() || !newerTypesList.Any())
                {
                    throw new InvalidOperationException("Test data has no relevant types.");
                }
                return Compare(olderTypesList, newerTypesList);
            }
        }

        /// <summary>
        /// Loads a package from NuGet and extracts an assembly definition.
        /// </summary>
        /// <param name="package">The name of the package to load. Must not be null.</param>
        /// <param name="version">The version of the package to load. May be null, in which case the latest stable version is downloaded.</param>
        /// <param name="tfm">The target framework to find within the package. May be null, in which case "netstandard2.0" is assumed.</param>
        /// <param name="assemblyName">The name of the assembly to find within the package. May be null, in which case it's assumed to be the same as the package.</param>
        /// <returns>The assembly definition loaded from the given package.</returns>
        public async static Task<AssemblyDefinition> LoadPackageAsync(string package, string version, string tfm, string assemblyName)
        {
            tfm = tfm ?? "netstandard2.0";
            assemblyName = assemblyName ?? package;
            using (var client = new HttpClient())
            {
                // Handily, this automatically loads the latest stable release if version is null.
                var url = $"https://www.nuget.org/api/v2/package/{package}/{version}";
                var response = await client.GetAsync(url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                var bytes = await response.Content.ReadAsByteArrayAsync().ConfigureAwait(false);

                string expectedPath = $"lib/{tfm}/{assemblyName}.dll";

                using (var zip = ZipArchive.Open(new MemoryStream(bytes)))
                {
                    foreach (var entry in zip.Entries)
                    {
                        if (entry.Key == expectedPath)
                        {
                            var path = entry.Key.Substring(4);
                            string targetFramework = Path.GetDirectoryName(path);
                            using (var stream = entry.OpenEntryStream())
                            {
                                // Mono.Cecil requires the stream to be seekable. It's simplest
                                // just to copy the whole DLL to a MemoryStream and pass that to Cecil.
                                var ms = new MemoryStream();
                                await stream.CopyToAsync(ms).ConfigureAwait(false);
                                ms.Position = 0;
                                return AssemblyDefinition.ReadAssembly(ms);
                            }
                        }
                    }
                }
                throw new InvalidOperationException($"Unable to find entry '{expectedPath}' in package");
            }
        }

        public static AssemblyDefinition LoadFile(string file)
        {
            var bytes = File.ReadAllBytes(file);
            return AssemblyDefinition.ReadAssembly(new MemoryStream(bytes));
        }

    }
}
