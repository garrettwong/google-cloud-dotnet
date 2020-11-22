﻿// Copyright 2017 Google Inc. All Rights Reserved.
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

using Google.Cloud.Tools.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Google.Cloud.Tools.ReleaseManager
{
    public class GenerateProjectsCommand : CommandBase
    {
        private static readonly Regex AnyVersionPattern = new Regex(@"^[0-9]\d*\.\d+\.\d+(\.\d+)?(-.*)?$");
        private static readonly Regex StableVersionPattern = new Regex(@"^[1-9]\d*\.\d+\.\d+(\.\d+)?$");
        private static readonly Regex AnyDesktopFramework = new Regex(@";net\d+");

        // Project references which don't just follow the pattern of ..\..\{package}\{package}\{package}.csproj
        private static readonly Dictionary<string, string> KnownProjectReferences = new Dictionary<string, string>
        {
            { "Google.Cloud.AnalyzersTesting", @"..\..\..\tools\Google.Cloud.AnalyzersTesting\Google.Cloud.AnalyzersTesting.csproj" },
            { "Google.Cloud.ClientTesting", @"..\..\..\tools\Google.Cloud.ClientTesting\Google.Cloud.ClientTesting.csproj" },
            { "Google.Cloud.Diagnostics.Common.Tests", @"..\..\Google.Cloud.Diagnostics.Common\Google.Cloud.Diagnostics.Common.Tests\Google.Cloud.Diagnostics.Common.Tests.csproj" },
            { "Google.Cloud.Diagnostics.Common.IntegrationTests", @"..\..\Google.Cloud.Diagnostics.Common\Google.Cloud.Diagnostics.Common.IntegrationTests\Google.Cloud.Diagnostics.Common.IntegrationTests.csproj" },
            { "Google.Cloud.SampleUtil", @"..\..\..\tools\Google.Cloud.SampleUtil\Google.Cloud.SampleUtil.csproj"}
        };

        private const string DefaultRestTargetFrameworks = "netstandard2.0;net461";
        private const string DefaultGrpcTargetFrameworks = "netstandard2.0;net461";
        private const string DefaultTestTargetFrameworks = "netcoreapp2.1;net461";

        private const string AnalyzersTargetFramework = "netstandard1.3";
        private const string AnalyzersTestTargetFramework = "netcoreapp2.0";

        private const string ProjectVersionValue = "project";
        private const string DefaultVersionValue = "default";
        private const string GrpcPackage = "Grpc.Core";
        private const string DefaultGaxVersion = "3.2.0";
        private const string GrpcVersion = "2.31.0";
        private static readonly Dictionary<string, string> DefaultPackageVersions = new Dictionary<string, string>
        {
            { "Google.Api.Gax", DefaultGaxVersion },
            { "Google.Api.Gax.Rest", DefaultGaxVersion },
            { "Google.Api.Gax.Grpc", DefaultGaxVersion },
            { "Google.Api.Gax.Grpc.GrpcCore", DefaultGaxVersion },
            { "Google.Api.Gax.Grpc.Gcp", DefaultGaxVersion },
            { "Google.Api.Gax.Testing", DefaultGaxVersion },
            { "Google.Api.Gax.Grpc.Testing", DefaultGaxVersion },
            { GrpcPackage, GrpcVersion },
            { "Grpc.Core.Testing", GrpcVersion },
            { "Grpc.Core.Api", GrpcVersion },
            { "Google.Api.CommonProtos", "2.2.0" },
            { "Google.Protobuf", "3.13.0" }
        };

        // Hard-coded versions for all analyzer projects.
        private static readonly Dictionary<string, string> CommonAnalyzerDependencies = new Dictionary<string, string>
        {
            { CSharpWorkspacesPackage, "2.4.0" }
        };

        // Hard-coded versions for all test packages. These can be defaulted even for stable packages, whereas
        // the packages in DefaultPackageVersions should be specified precisely in stable packages.
        private static readonly Dictionary<string, string> CommonTestDependencies = new Dictionary<string, string>
        {
            { "Google.Cloud.ClientTesting", ProjectVersionValue }, // Needed for all snippets and some other tests - easiest to just default
            { "Microsoft.NET.Test.Sdk", "16.2.0" },
            { "xunit", "2.4.1" },
            { "xunit.runner.visualstudio", "2.4.1" },
            { "Xunit.SkippableFact", "1.3.12" },
            { "Moq", "4.12.0" },
            { "System.Linq.Async", "4.0.0" },
            { ReferenceAssembliesPackage, ReferenceAssembliesVersion }
        };

        // Hard-coded versions for dependencies for production packages that can be updated arbitrarily, as their assets are all private.
        // The relationship between this and PrivateAssets is tested on startup.
        private static readonly Dictionary<string, string> CommonHiddenProductionDependencies = new Dictionary<string, string>
        {
            { CompatibilityAnalyzer, "0.2.12-alpha" },
            { ConfigureAwaitAnalyzer, "1.0.1" },
            { ReferenceAssembliesPackage, ReferenceAssembliesVersion }
        };

        private static readonly Dictionary<string, string> CommonSampleDependencies = new Dictionary<string, string>
        {
            { "CommandLineParser", "2.6.0" },
            { "Google.Cloud.SampleUtil", "project"},
            { ReferenceAssembliesPackage, ReferenceAssembliesVersion }
        };

        private const string CompatibilityAnalyzer = "Microsoft.DotNet.Analyzers.Compatibility";
        private const string ConfigureAwaitAnalyzer = "ConfigureAwaitChecker.Analyzer";
        private const string CSharpWorkspacesPackage = "Microsoft.CodeAnalysis.CSharp.Workspaces";
        private const string ReferenceAssembliesPackage = "Microsoft.NETFramework.ReferenceAssemblies";
        private const string ReferenceAssembliesVersion = "1.0.0";

        /// <summary>
        /// For packages which need a PrivateAssets attribute in dependencies, this dictionary provides the value of the attribute.
        /// </summary>
        private static readonly Dictionary<string, string> PrivateAssets = new Dictionary<string, string>
        {
            { GrpcPackage, "None" },
            { CompatibilityAnalyzer, "All" },
            { ConfigureAwaitAnalyzer, "All" },
            { CSharpWorkspacesPackage, "All" },
            { ReferenceAssembliesPackage, "All" }
        };

        private const string AnalyzersPath = @"..\..\..\tools\Google.Cloud.Tools.Analyzers\bin\$(Configuration)\netstandard1.3\publish\Google.Cloud.Tools.Analyzers.dll";

        public GenerateProjectsCommand() : base("generate-projects", "Generates project files, coverage files etc from the API catalog")
        {
        }

        protected override void ExecuteImpl(string[] args)
        {
            ValidateCommonHiddenProductionDependencies();
            var root = DirectoryLayout.DetermineRootDirectory();
            var catalog = ApiCatalog.Load();
            Console.WriteLine($"API catalog contains {catalog.Apis.Count} entries");
            // Now we know we can parse the API catalog, let's reformat it.
            ReformatApiCatalog(catalog);
            RewriteReadme(catalog);
            RewriteDocsRootIndex(catalog);
            HashSet<string> apiNames = catalog.CreateIdHashSet();

            foreach (var api in catalog.Apis)
            {
                var path = Path.Combine(root, "apis", api.Id);
                GenerateProjects(path, api, apiNames);
                GenerateSolutionFiles(path, api);
                GenerateDocumentationStub(path, api);
                GenerateSynthConfiguration(path, api);
                GenerateMetadataFile(path, api);
            }            
        }

        private static void ReformatApiCatalog(ApiCatalog catalog)
        {
            string path = ApiCatalog.CatalogPath;
            string existing = File.ReadAllText(path);
            string formatted = catalog.FormatJson();
            if (existing != formatted)
            {
                File.WriteAllText(path, formatted);
                Console.WriteLine("Reformatted apis.json");
            }
        }

        public static void RewriteReadme(ApiCatalog catalog)
        {
            var root = DirectoryLayout.DetermineRootDirectory();
            var readmePath = Path.Combine(root, "README.md");
            RewriteApiTable(readmePath, catalog, api => $"https://googleapis.dev/dotnet/{api.Id}/{api.Version}");
        }

        public static void RewriteDocsRootIndex(ApiCatalog catalog)
        {
            var root = DirectoryLayout.ForRootDocs().DocsSourceDirectory;
            var indexPath = Path.Combine(root, "index.md");
            RewriteApiTable(indexPath, catalog, api => $"{api.Id}/index.html");
        }

        /// <summary>
        /// Updates the dependencies in an API for known packages, but only if the default
        /// version is later than the current one, with the same major version number.
        /// </summary>
        public static void UpdateDependencies(ApiCatalog catalog, ApiMetadata api)
        {
            UpdateDependencyDictionary(api.Dependencies, "dependencies");
            UpdateDependencyDictionary(api.TestDependencies, "testDependencies");

            void UpdateDependencyDictionary(SortedDictionary<string, string> dependencies, string jsonName)
            {
                if (dependencies.Count == 0)
                {
                    return;
                }

                // We want to update any dependencies to "external" packages as listed in DefaultPackageVersions,
                // but also "internal" packages such as Google.LongRunning.
                Dictionary<string, string> allDefaultPackageVersions = DefaultPackageVersions
                    .Concat(catalog.Apis.Select(api => new KeyValuePair<string, string>(api.Id, api.Version)))
                    .ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (var package in dependencies.Keys.ToList())
                {
                    if (allDefaultPackageVersions.TryGetValue(package, out var defaultVersion))
                    {
                        var currentVersion = dependencies[package];
                        if (currentVersion == DefaultVersionValue ||
                            currentVersion == ProjectVersionValue ||
                            defaultVersion == currentVersion)
                        {
                            continue;
                        }
                        var structuredDefaultVersion = StructuredVersion.FromString(defaultVersion);
                        var structuredCurrentVersion = StructuredVersion.FromString(currentVersion);
                        if (structuredDefaultVersion.CompareTo(structuredCurrentVersion) > 0 &&
                            structuredDefaultVersion.Major == structuredCurrentVersion.Major)
                        {
                            dependencies[package] = defaultVersion;
                        }
                    }
                }
                if (api.Json is object)
                {
                    api.Json[jsonName] = new JObject(dependencies.Select(pair => new JProperty(pair.Key, pair.Value)));
                }
            }
        }

        /// <summary>
        /// README.md and docs/root/index.md use the same table, but with slightly different links. This
        /// method is the common code, with a URL provider to indicate how to link to an API's documentation.
        /// </summary>
        private static void RewriteApiTable(string path, ApiCatalog catalog, Func<ApiMetadata, string> urlProvider)
        {
            var existing = File.ReadAllLines(path).ToList();

            string headerLine = "| Package | Latest version | Description |";
            string headerNext = "|---------|----------------|-------------|";
            int headerIndex = existing.IndexOf(headerLine);

            if (headerIndex == -1)
            {
                throw new UserErrorException($"Header line for library table not found.");
            }

            var linesBefore = existing.Take(headerIndex).ToList();
            var linesAfter = existing.Skip(headerIndex).SkipWhile(line => line.StartsWith("|")).ToList();

            var table = new List<string>();
            table.Add(headerLine);
            table.Add(headerNext);

            var ambiguousDescriptions = catalog.Apis
                .Select(api => api.EffectiveListingDescription)
                .GroupBy(description => description)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            foreach (var api in catalog.Apis)
            {
                // TODO: What about 2.0.0-beta00 etc? We'd need to know what version to link to.
                // We can cross that bridge when we come to it.
                if (api.Version == "1.0.0-beta00" || api.Version == "1.0.0-alpha00")
                {
                    continue;
                }
                string packageLink = $"[{api.Id}]({urlProvider(api)})";

                // Disambiguate direct API packages for the same product.
                string description = api.EffectiveListingDescription;
                if (ambiguousDescriptions.Contains(description) && api.ApiVersion is string apiVersion)
                {
                    description += $" ({apiVersion} API)";
                }

                // TODO: It gets awkward when there are multiple description for the same product (e.g. Spanner).
                // This is basically caused by conflating "link to product docs" with "package description", but
                // in most cases that simplifies things. Never mind.
                string productLink = api.ProductUrl is null
                    ? api.ListingDescription ?? api.ProductName ?? api.Description.TrimEnd('.') // No URL
                    : $"[{description}]({api.ProductUrl})";
                table.Add($"| {packageLink} | {api.Version} | {productLink} |");
            }

            var newContent = linesBefore.Concat(table).Concat(linesAfter);
            if (!existing.SequenceEqual(newContent))
            {
                File.WriteAllLines(path, newContent);
                Console.WriteLine($"Rewrote {Path.GetFileName(path)}");
            }
        }

        public static void GenerateProjects(string apiRoot, ApiMetadata api, HashSet<string> apiNames)
        {
            if (api.Type == ApiType.Analyzers)
            {
                Directory.CreateDirectory(apiRoot);

                var mainDirectory = Path.Combine(apiRoot, api.Id);
                Directory.CreateDirectory(mainDirectory);

                var testDirectory = Path.Combine(apiRoot, api.Id + ".Tests");
                Directory.CreateDirectory(testDirectory);
            }

            // We assume the source directories already exist, either because they've just
            // been generated or because they were already there. We infer the type of each
            // project based on the directory name. Expected suffixes:
            // - None: main API
            // - .Snippets: snippets (manual and generated)
            // - .Tests: unit tests
            // - .IntegrationTests: integration tests
            // - .Samples: generated standalone samples

            // Anything else will be ignored for now...
            var projectDirectories = Directory.GetDirectories(apiRoot)
                .Where(pd => Path.GetFileName(pd).StartsWith(api.Id))
                .ToList();
            foreach (var dir in projectDirectories)
            {
                string suffix = Path.GetFileName(dir).Substring(api.Id.Length);
                switch (suffix)
                {
                    case "":
                        GenerateMainProject(api, dir, apiNames);
                        break;
                    case ".SmokeTests":
                        GenerateSmokeTestProject(api, dir, apiNames);
                        break;
                    case ".IntegrationTests":
                    case ".Snippets":
                    case ".Tests":
                        GenerateTestProject(api, dir, apiNames, isForAnalyzers: api.Type == ApiType.Analyzers);
                        GenerateCoverageFile(api, dir);
                        break;
                    case ".Samples":
                        GenerateSampleProject(api, dir, apiNames);
                        break;
                }
            }

            // TODO: Updates for unknown project types? Tricky...
        }

        static void GenerateSolutionFiles(string apiRoot, ApiMetadata api)
        {
            var projectDirectories = Directory.GetDirectories(apiRoot)
                .Where(pd => Path.GetFileName(pd).StartsWith(api.Id))
                .ToList();

            HashSet<string> projects = new HashSet<string>();
            // We want to include all the project files, and all the project references
            // from those project files, being aware that the solution file is already one directory
            // higher than the project file...
            foreach (var dir in projectDirectories)
            {
                string projectName = Path.GetFileName(dir);
                string projectFile = Path.Combine(dir, $"{projectName}.csproj");
                if (File.Exists(projectFile))
                {
                    projects.Add($"{projectName}/{projectName}.csproj");
                    XDocument doc = XDocument.Load(projectFile);
                    var projectReferences = doc.Descendants("ProjectReference")
                        .Select(x => x.Attribute("Include").Value.Replace("\\", "/"))
                        .Select(x => x.StartsWith("../") ? x.Substring(3) : x);
                    foreach (var reference in projectReferences)
                    {
                        projects.Add(reference);
                    }
                }
            }

            var solutionFileName = $"{api.Id}.sln";
            string fullFile = Path.Combine(apiRoot, solutionFileName);
            string beforeHash = GetFileHash(fullFile);
            if (!File.Exists(fullFile))
            {
                Processes.RunDotnet(apiRoot, "new", "sln", "-n", api.Id);
            }
            else
            {
                // Optimization: don't run "dotnet sln add" if we can find project entries for all the relevant project
                // references already. This is crude, but speeds up the overall process significantly.
                var projectLines = File.ReadAllLines(fullFile).Where(line => line.StartsWith("Project(")).ToList();
                if (projects.Select(p => $"\"{p.Replace("/", "\\")}\"")
                            .All(expectedProject => projectLines.Any(pl => pl.Contains(expectedProject))))
                {
                    return;
                }
            }

            // It's much faster to run a single process than to run it once per project.
            Processes.RunDotnet(apiRoot, new[] { "sln", solutionFileName, "add" }.Concat(projects).ToArray());

            string afterHash = GetFileHash(fullFile);
            if (beforeHash != afterHash)
            {
                Console.WriteLine($"{(beforeHash == null ? "Created" : "Modified")} solution file for {api.Id}");
            }
        }

        static void GenerateDocumentationStub(string apiRoot, ApiMetadata api)
        {
            string file = Path.Combine(apiRoot, "docs", "index.md");
            if (File.Exists(file))
            {
                return;
            }
            Directory.CreateDirectory(Path.GetDirectoryName(file));
            string stub = api.ProductName != null && api.ProductUrl != null ?
@"{{title}}

{{description}}

{{version}}

{{installation}}

{{auth}}

# Getting started

{{client-classes}}

{{client-construction}}
"
: "{{non-product-stub}}";
            File.WriteAllText(file, stub);
            Console.WriteLine($"Generated documentation stub for {api.Id}");
        }

        private static void GenerateSynthConfiguration(string apiRoot, ApiMetadata api)
        {
            if (api.Generator == GeneratorType.None)
            {
                return;
            }
            var synthFile = Path.Combine(apiRoot, "synth.py");

            // Currently all APIs use the exact same synth file, so we can just replace it every time.
            // We may need something more sophisticated in the future.
            string content =
@"# GENERATED BY Google.Cloud.Tools.ProjectGenerator - DO NOT EDIT!

import json
import sys
from synthtool import shell
from synthtool import metadata
from pathlib import Path

# generateapis.sh updates synth.metadata itself
metadata.enable_write_metadata(False)
AUTOSYNTH_MULTIPLE_COMMITS = True

# Parent of the script is the API-specific directory
# Parent of the API-specific directory is the apis directory
# Parent of the apis directory is the repo root
root = Path(__file__).parent.parent.parent
package = Path(__file__).parent.name

bash = '/bin/bash'
if sys.platform == 'win32':
  bash = 'C:\\Program Files\\Git\\bin\\bash.exe'

shell.run(
  (bash, 'generateapis.sh', '--check_compatibility', package),
  cwd = root,
  hide_output = False)
";
            File.WriteAllText(synthFile, content);
        }

        /// <summary>
        /// Generates a metadata file (currently .repo-metadata.json; may change name later) with
        /// all the information that language-agnostic tools require.
        /// </summary>
        public static void GenerateMetadataFile(string apiRoot, ApiMetadata api)
        {
            string metadataPath = Path.Combine(apiRoot, ".repo-metadata.json");
            var version = api.StructuredVersion;
            string versionBasedReleaseLevel =
                // Version "1.0.0-beta00" hasn't been released at all, so we don't have a package to talk about.
                (version.Prerelease ?? "").EndsWith("00") && version.Major == 1 && version.Minor == 0 ? "none"
                // If it's not a prerelease now, or it's ever got to 1.0, it's generally "ga"
                : version.Major > 1 || version.Minor > 0 || version.Prerelease == null ? "ga"
                : version.Prerelease.StartsWith("beta") ? "beta" : "alpha";

            string releaseLevel = api.ReleaseLevelOverride ?? versionBasedReleaseLevel;
            if (releaseLevel == "none")
            {
                // If we have temporarily set the version to (say) beta01 and then reset it to beta00,
                // make sure we don't have an obsolete metadata file.
                File.Delete(metadataPath);
                return;
            }
            var metadata = new
            {
                distribution_name = api.Id,
                release_level = releaseLevel,
                client_documentation = $"https://googleapis.dev/dotnet/{api.Id}/latest",
            };
            string json = JsonConvert.SerializeObject(metadata, Formatting.Indented);
            File.WriteAllText(metadataPath, json);
        }

        private static void GenerateMainProject(ApiMetadata api, string directory, HashSet<string> apiNames)
        {
            if (api.Version == null)
            {
                throw new UserErrorException($"No version specified for {api.Id}");
            }
            string targetFrameworks = api.TargetFrameworks;

            SortedList<string, string> dependencies;
            if (api.Type == ApiType.Analyzers)
            {
                if (targetFrameworks != AnalyzersTargetFramework)
                {
                    throw new UserErrorException($"Analyzers are expected to use {AnalyzersTargetFramework}");
                }

                dependencies = new SortedList<string, string>(CommonAnalyzerDependencies, StringComparer.Ordinal);

                // Note: If support is added here for using additional dependencies, we need to resolve
                //       the packaging issues and make sure the onus won't be on the user to add the
                //       dependency references.
            }
            else
            {

                dependencies = new SortedList<string, string>(CommonHiddenProductionDependencies, StringComparer.Ordinal);

                switch (api.Type)
                {
                    case ApiType.Rest:
                        dependencies.Add("Google.Api.Gax.Rest", DefaultVersionValue);
                        targetFrameworks = targetFrameworks ?? DefaultRestTargetFrameworks;
                        break;
                    case ApiType.Grpc:
                        dependencies.Add("Google.Api.Gax.Grpc.GrpcCore", DefaultVersionValue);
                        dependencies.Add("Grpc.Core", DefaultVersionValue);
                        targetFrameworks = targetFrameworks ?? DefaultGrpcTargetFrameworks;
                        break;
                }

                // Deliberately not using Add, so that a project can override the defaults.
                // In particular, stable releases *must* override versions of GRPC and GAX.
                foreach (var dependency in api.Dependencies)
                {
                    dependencies[dependency.Key] = dependency.Value;
                }
            }

            var propertyGroup = new XElement("PropertyGroup",
                // Build-related properties
                new XElement("Version", api.Version), // TODO: Version, or VersionPrefix/VersionSuffix?
                new XElement("TargetFrameworks", targetFrameworks),
                new XElement("GenerateDocumentationFile", api.Type != ApiType.Analyzers),
                // Package-related properties
                new XElement("Description", api.Description),
                new XElement("PackageTags", string.Join(";", api.Tags.Concat(new[] { "Google", "Cloud" }))),
                new XElement("Copyright", $"Copyright {DateTime.UtcNow.Year} Google LLC")
            );
            if (dependencies.ContainsKey(GrpcPackage))
            {
                propertyGroup.Add(new XElement("CodeAnalysisRuleSet", "..\\..\\..\\grpc.ruleset"));
            }
            var dependenciesElement = CreateDependenciesElement(api.Id, dependencies, api.IsReleaseVersion, testProject: false, apiNames: apiNames);

            if (api.Type == ApiType.Analyzers)
            {
                propertyGroup.Add(new XElement("IncludeBuildOutput", false));

                void AddPackFile(string includePath, string packagePath)
                {
                    dependenciesElement.Add(
                        new XElement("None",
                            new XAttribute("Include", includePath),
                            new XAttribute("Pack", "true"),
                            new XAttribute("PackagePath", packagePath),
                            new XAttribute("Visible", "false")));
                }

                AddPackFile(
                    $"$(OutputPath)\\{AnalyzersTargetFramework}\\$(AssemblyName).dll",
                    "analyzers/dotnet/cs");

                // Add install scripts as per
                // https://docs.microsoft.com/en-us/nuget/reference/analyzers-conventions#install-and-uninstall-scripts
                // Name each file rather than using a wildcard so 'dotnet pack' will error out if the files are missing
                // for some reason.
                AddPackFile(
                    @"..\..\..\analyzerScripts\install.ps1",
                    "tools");
                AddPackFile(
                    @"..\..\..\analyzerScripts\uninstall.ps1",
                    "tools");
            }
            WriteProjectFile(api, directory, propertyGroup, dependenciesElement);
        }

        private static string GetTestTargetFrameworks(ApiMetadata api, bool isForAnalyzers) =>
            isForAnalyzers
                ? AnalyzersTestTargetFramework
                : api.TestTargetFrameworks ?? api.TargetFrameworks ?? DefaultTestTargetFrameworks;


        private static void GenerateSmokeTestProject(ApiMetadata api, string directory, HashSet<string> apiNames)
        {
            // Don't generate a project file if we've got a placeholder directory
            if (Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).Length == 0)
            {
                return;
            }
            var propertyGroup =
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "netcoreapp2.1"),
                    new XElement("OutputType", "Exe"),
                    new XElement("IsPackable", false));
            var dependenciesElement =
                new XElement("ItemGroup",
                    CreateDependencyElement(Path.GetFileName(directory), api.Id, ProjectVersionValue, stableRelease: false, testProject: true, apiNames));
            WriteProjectFile(api, directory, propertyGroup, dependenciesElement);
        }

        private static void GenerateSampleProject(ApiMetadata api, string directory, HashSet<string> apiNames)
        {
            // Don't generate a project file if we've got a placeholder directory
            if (Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).Length == 0)
            {
                return;
            }
            var dependencies = new SortedList<string, string>(CommonSampleDependencies, StringComparer.Ordinal);
            dependencies.Add(api.Id, "project");
            var propertyGroup =
                new XElement("PropertyGroup",
                    new XElement("TargetFramework", "netcoreapp2.1"),
                    new XElement("OutputType", "Exe"),
                    new XElement("IsPackable", false),
                    new XElement("StartupObject", api.Id + ".Samples.Program"));

            string project = Path.GetFileName(directory);
            var dependenciesElement = CreateDependenciesElement(project, dependencies, api.IsReleaseVersion, testProject: true, apiNames: apiNames);
            WriteProjectFile(api, directory, propertyGroup, dependenciesElement);
        }

        private static void GenerateTestProject(ApiMetadata api, string directory, HashSet<string> apiNames, bool isForAnalyzers = false)
        {
            // Don't generate a project file if we've got a placeholder directory
            if (Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).Length == 0)
            {
                return;
            }
            var dependencies = new SortedList<string, string>(CommonTestDependencies, StringComparer.Ordinal);
            if (isForAnalyzers)
            {
                dependencies.Remove("Google.Cloud.ClientTesting");
                dependencies.Add("Google.Cloud.AnalyzersTesting", ProjectVersionValue);
            }

            dependencies.Add(api.Id, "project");

            // Deliberately not using Add, so that a project can override the defaults.
            foreach (var dependency in api.TestDependencies)
            {
                dependencies[dependency.Key] = dependency.Value;
            }

            var testTargetFrameworks = GetTestTargetFrameworks(api, isForAnalyzers);
            var propertyGroup =
                new XElement("PropertyGroup",
                    new XElement("TargetFrameworks", testTargetFrameworks),
                    new XElement("TestTargetFrameworks", new XAttribute("Condition", " '$(OS)' != 'Windows_NT' "), AnyDesktopFramework.Replace(testTargetFrameworks, "")),
                    new XElement("IsPackable", false),
                    // 1701, 1702 and 1705 are disabled by default.
                    // xUnit2004 prevents Assert.Equal(true, value) etc, preferring Assert.True and Assert.False, but
                    //   Assert.Equal is clearer (IMO) for comparing values rather than conditions.
                    // xUnit2013 prevents simple checks for the number of items in a collection
                    // AD0001 is the error for an analyzer throwing an exception. We can remove this when
                    // the fix for https://github.com/xunit/xunit/issues/1409 is in NuGet.
                    new XElement("NoWarn", "1701;1702;1705;xUnit2004;xUnit2013;AD0001")
                );
            string project = Path.GetFileName(directory);
            var dependenciesElement = CreateDependenciesElement(project, dependencies, api.IsReleaseVersion, testProject: true, apiNames: apiNames);
            // Test service... it keeps on getting added by Visual Studio, so let's just include it everywhere.
            dependenciesElement.Add(new XElement("Service", new XAttribute("Include", "{82a7f48d-3b50-4b1e-b82e-3ada8210c358}")));
            WriteProjectFile(api, directory, propertyGroup, dependenciesElement);
        }

        private static void GenerateCoverageFile(ApiMetadata api, string directory)
        {
            // Don't generate a coverage file if we've got a placeholder directory
            if (Directory.GetFiles(directory, "*.cs", SearchOption.AllDirectories).Length == 0)
            {
                return;
            }
            var targetExecutable = new XElement("TargetExecutable", "C:/Program Files/dotnet/dotnet.exe");
            var targetArguments = new XElement("TargetArguments", "test --no-build -c Release");
            var filters = new XElement("Filters", new XElement("IncludeFilters",
                new XElement("FilterEntry", new XElement("ModuleMask", api.Id)),
                // Allow tests to contribute coverage to project dependencies, but not package dependencies
                // (as the latter will be covering code we're not reporting on).
                api.Dependencies
                    .Where(dep => dep.Value == ProjectVersionValue)
                    .Select(dep => new XElement("FilterEntry", new XElement("ModuleMask", dep.Key)))
                ));
            var attributeFilters = new XElement("AttributeFilters",
                new XElement("AttributeFilterEntry",
                    "System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverageAttribute"),
                new XElement("AttributeFilterEntry", "System.Diagnostics.DebuggerNonUserCodeAttribute")
            );
            var output = new XElement("Output", $"../../../coverage/{Path.GetFileName(directory)}.dvcr");
            var workingDir = new XElement("TargetWorkingDir", ".");

            var doc = new XElement("CoverageParams",
                targetExecutable,
                targetArguments,
                filters,
                attributeFilters,
                workingDir,
                output
            );

            using (var stream = File.Create(Path.Combine(directory, "coverage.xml")))
            {
                doc.Save(stream);
            }
        }

        private static void WriteProjectFile(
            ApiMetadata api, string directory, XElement propertyGroup, XElement dependenciesItemGroup)
        {
            var file = Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj");
            string beforeHash = GetFileHash(file);
            XElement doc;
            // If the file already exists, load it and replace the elements (leaving any further PropertyGroup and ItemGroup elements).
            if (File.Exists(file))
            {
                doc = XElement.Load(file);
                doc.Elements("PropertyGroup").First().ReplaceWith(propertyGroup);
                doc.Elements("ItemGroup").First().ReplaceWith(dependenciesItemGroup);
            }
            // Otherwise, create a new one
            else
            {
                doc = new XElement("Project",
                    new XAttribute("Sdk", "Microsoft.NET.Sdk"),
                    propertyGroup,
                    dependenciesItemGroup);
            }

            // Don't use File.CreateText as that omits the byte order mark.
            // While byte order marks are nasty, Visual Studio will add it back any time a project file is
            // manually edited, so it's best if we follow suit.
            using (var stream = File.Create(Path.Combine(directory, $"{Path.GetFileName(directory)}.csproj")))
            {
                doc.Save(stream);
            }
            string afterHash = GetFileHash(file);
            if (beforeHash != afterHash)
            {
                Console.WriteLine($"{(beforeHash == null ? "Created" : "Modified")} project file {Path.GetFileName(file)}");
            }
        }

        private static XElement CreateDependenciesElement(string project, IDictionary<string, string> dependencies, bool stableRelease, bool testProject, HashSet<string> apiNames) =>
            new XElement("ItemGroup",
                // Use the GAX version for all otherwise-unversioned GAX dependencies
                dependencies.Select(pair => CreateDependencyElement(project, pair.Key, pair.Value, stableRelease, testProject, apiNames)),
                new XElement("Analyzer",
                    new XAttribute("Condition", $"Exists('{AnalyzersPath}')"),
                    new XAttribute("Include", AnalyzersPath)));

        /// <summary>
        /// Creates a single XElement for a dependency. This can be a package reference or a project reference:
        /// - If the version is "project" it will be handled as a project reference:
        ///   - The project must be known, either as the ID of an API or a member of <see cref="KnownProjectReferences"/>
        /// - If the version is "default" it will be handled as a package reference to a common package, e.g. Grpc.Core or GAX
        ///   with the version automatically populated. This is only valid for prereleases; stable packages must specify
        ///   a version number (so that it's not accidentally increased)
        /// - Otherwise, it will be treated as a package reference with the 
        /// </summary>
        /// <param name="project">The project depending on <paramref name="package"/></param>
        /// <param name="package">Package name of dependency</param>
        /// <param name="version">Version of dependency, or "project" or "default"</param>
        /// <param name="allowDefaultVersions">Whether to allow default versions for </param>
        /// <param name="apiNames">Names of all APIs in apis.json, valid for project references</param>
        /// <returns>The element to include in the project file to represent the dependency</returns>
        private static XElement CreateDependencyElement(string project, string package, string version, bool stableRelease, bool testProject, HashSet<string> apiNames)
        {
            if (version == ProjectVersionValue)
            {
                string path;
                if (apiNames.Contains(package))
                {
                    // Simplify path for test packages - no need to go all the way back to the api directory
                    path = project.StartsWith(package + ".")
                        ? $@"..\{package}\{package}.csproj"
                        : $@"..\..\{package}\{package}\{package}.csproj";
                }
                else if (KnownProjectReferences.ContainsKey(package))
                {
                    path = KnownProjectReferences[package];
                }
                else
                {
                    throw new UserErrorException($"Invalid project reference from {project} to {package}");
                }
                return new XElement("ProjectReference", new XAttribute("Include", path));
            }

            if (version == DefaultVersionValue)
            {
                if (stableRelease)
                {
                    throw new UserErrorException($"Project {project} cannot use the default version for {package}");
                }
                if (!DefaultPackageVersions.TryGetValue(package, out version))
                {
                    throw new UserErrorException($"Project {project} depends on default version of unknown package {package}");
                }
            }

            PrivateAssets.TryGetValue(package, out string privateAssetValue);

            if (!AnyVersionPattern.IsMatch(version))
            {
                throw new UserErrorException($"Project {project} has invalid version '{version}' for package {package}");
            }
            // Dependencies from production projects, other than "hidden" packages, must be stable
            if (stableRelease && !testProject && !StableVersionPattern.IsMatch(version) && privateAssetValue != "All")
            {
                throw new UserErrorException($"Project {project} uses prerelease version '{version}' for package {package}");
            }
            var element = new XElement("PackageReference",
                new XAttribute("Include", package),
                new XAttribute("Version", GetDependencyVersionRange(package, version)));
            if (privateAssetValue != null)
            {
                element.Add(new XAttribute("PrivateAssets", privateAssetValue));
            }
            return element;
        }

        /// <summary>
        /// Returns the appropriate version to include in a package dependency.
        /// For Google.* and Grpc.*, this is major-version pinned. For other packages, we just leave it as the version
        /// specified in the string - as some packages are fine to upgrade beyond major version boundaries.
        /// </summary>
        private static string GetDependencyVersionRange(string package, string specifiedVersion)
        {
            if (!(package.StartsWith("Google.") || package.StartsWith("Grpc.")))
            {
                return specifiedVersion;
            }
            var structuredVersion = StructuredVersion.FromString(specifiedVersion);
            var nextMajor = StructuredVersion.FromMajorMinorPatchBuild(structuredVersion.Major + 1, 0, 0, structuredVersion.Build is null ? default(int?) : 0, null);
            return $"[{structuredVersion}, {nextMajor})";
        }

        /// <summary>
        /// This is in lieu of a unit test... just make sure that all the dependencies we're hard-coding the values for
        /// really do hide all their assets.
        /// </summary>
        private static void ValidateCommonHiddenProductionDependencies()
        {
            var brokenDependencies = CommonHiddenProductionDependencies.Keys.Where(p => !PrivateAssets.ContainsKey(p) || PrivateAssets[p] != "All").ToList();
            if (brokenDependencies.Any())
            {
                throw new Exception($"ProjectGenerator error: invalid CommonHiddenProductionDependencies: {string.Join(", ", brokenDependencies)}");
            }
        }

        private static string GetFileHash(string file)
        {
            if (!File.Exists(file))
            {
                return null;
            }
            using (var sha256 = SHA256.Create())
            {
                using (var stream = File.OpenRead(file))
                {
                    byte[] hash = sha256.ComputeHash(stream);
                    return BitConverter.ToString(hash);
                }
            }
        }
    }
}
