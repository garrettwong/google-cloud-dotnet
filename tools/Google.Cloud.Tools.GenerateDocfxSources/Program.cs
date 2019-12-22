﻿// Copyright 2016 Google Inc. All Rights Reserved.
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
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Google.Cloud.Tools.GenerateDocfxSources
{
    public class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                return MainImpl(args);
            }
            catch (UserErrorException e)
            {
                Console.WriteLine($"Error: {e.Message}");
                return 1;
            }
        }

        private static int MainImpl(string[] args)
        {
            if (args.Length != 1)
            {
                throw new UserErrorException("Please specify the API name");
            }
            string api = args[0];
            var layout = DirectoryLayout.ForApi(api);
            var apiMetadata = ApiMetadata.LoadApis().FirstOrDefault(x => x.Id == api);
            if (apiMetadata == null)
            {
                throw new UserErrorException($"Unable to load API metadata from apis.json for {api}");
            }

            string output = layout.DocsOutputDirectory;
            if (Directory.Exists(output))
            {
                Directory.Delete(output, true);
            }
            Directory.CreateDirectory(output);

            var apiDirectory = layout.SourceDirectory;
            var projects = Project.LoadProjects(apiDirectory).ToList();

            CreateDocfxJson(api, projects, output);
            CopyAndGenerateArticles(apiMetadata, layout.DocsSourceDirectory, output);
            CreateToc(api, output);
            return 0;
        }

        private static void CreateDocfxJson(string api, List<Project> projects, string outputDirectory)
        {
            var src = new JArray();
            foreach (var project in projects)
            {
                src.Add(new JObject
                {
                    ["files"] = new JArray { $"{project.Name}/{project.Name}.csproj" },
                    ["cwd"] = $"../../../apis/{api}"
                });
            }

            var dependencies = projects.SelectMany(p => p.Dependencies).OrderBy(d => d).Distinct().ToList();
            foreach (var dependency in dependencies)
            {
                // Cross-API dependencies (currently for IAM and LRO)
                string candidateDependency = $"../../../apis/{dependency}";
                if (Directory.Exists(Path.Combine(outputDirectory, candidateDependency)))
                {
                    src.Add(new JObject
                    {
                        ["files"] = new JArray { $"{dependency}/{dependency}.csproj" },
                        ["cwd"] = candidateDependency
                    });
                    continue;
                }
            }

            var json = new JObject
            {
                ["metadata"] = new JArray {
                    new JObject
                    {
                        ["src"] = src,
                        ["dest"] = "obj/api",
                        ["filter"] = "filterConfig.yml",
                        // TODO: net45, really?
                        ["properties"] = new JObject { ["TargetFramework"] = "net45" }
                    },
                },
                ["build"] = new JObject {
                    ["content"] = new JArray {
                        new JObject
                        {
                            ["files"] = "*.yml",
                            ["src"] = "obj/api",
                            ["dest"] = "api"
                        },
                        new JObject
                        {
                            ["files"] = new JArray { "*.md", "toc.yml" },
                        }
                    },
                    ["globalMetadata"] = new JObject
                    {
                        ["_appTitle"] = api,
                        ["_disableContribution"] = true,
                        ["_appFooter"] = " "
                    },
                    ["template"] = new JArray { "default", "../../docfx-template" },
                    ["overwrite"] = new JArray { "obj/snippets/*.md" },
                    ["dest"] = "site"
                }
            };
            File.WriteAllText(Path.Combine(outputDirectory, "docfx.json"), json.ToString());

            // We let the build script do work with the dependencies:
            // - Copy all yml files
            // - Concatenate toc.yml files
            var externalDependencies = dependencies
                .Where(d => Directory.Exists(Path.Combine(outputDirectory, $"../../dependencies/api/{d}")))
                .ToList();

            File.WriteAllText(Path.Combine(outputDirectory, "dependencies"), string.Join(" ", externalDependencies));
        }

        private static void CopyAndGenerateArticles(ApiMetadata api, string inputDirectory, string outputDirectory)
        {
            // Make sure there's a landing page.
            var index = Path.Combine(inputDirectory, "index.md");
            if (!File.Exists(index))
            {
                throw new UserErrorException($"No index.md file for {api.Id}. Please add one!");
            }

            // TODO: Do this properly, with templating etc.
            // TODO: Other resources, such as images?

            // Copy any existing documentation
            if (Directory.Exists(inputDirectory))
            {
                foreach (var file in Directory.GetFiles(inputDirectory, "*.md"))
                {
                    string text = File.ReadAllText(file);
                    text = TransformDocTemplate(api, text);
                    File.WriteAllText(Path.Combine(outputDirectory, Path.GetFileName(file)), text);                    
                }
            }
        }

        /// <summary>
        /// Extremely crude templating, but just enough for now... it replaces the following tokens:
        /// {{title}}: Markdown for the page title with the API ID
        /// {{description}}: Markdown for the API description
        /// {{installation}}: Markdown for the installation section
        /// {{auth}}: Markdown for authentication instructions
        /// {{sample:sample ID}}: Include a sample. The sample ID is of the form "Source.Anchor",
        ///   e.g. "Index.GettingStarted" or "StorageClient.Overview"
        /// </summary>
        private static string TransformDocTemplate(ApiMetadata api, string text)
        {
            string title = $"# {api.Id}";
            string description = $"`{api.Id}` is a.NET client library for the [{api.ProductName} API]({api.ProductUrl}).";
            string version =
$@"Note:
This documentation is for version `{ api.Version}` of the library.
Some samples may not work with other versions.";
            string installation =
$@"# Installation

Install the `{api.Id}` package from NuGet. Add it to
your project in the normal way (for example by right-clicking on the
project in Visual Studio and choosing ""Manage NuGet Packages..."").";
            if (!api.IsReleaseVersion)
            {
                installation += $@"
Please ensure you enable pre-release packages (for example, in the
Visual Studio NuGet user interface, check the ""Include prerelease""
box). Some of the following samples might only work with the latest 
pre-release version (`{api.Version}`) of `{api.Id}`.";
            }

            string auth =
@"# Authentication

When running on Google Cloud Platform, no action needs to be taken to authenticate.

Otherwise, the simplest way of authenticating your API calls is to
download a service account JSON file then set the `GOOGLE_APPLICATION_CREDENTIALS` environment variable to refer to it.
The credentials will automatically be used to authenticate. See the [Getting Started With
Authentication](https://cloud.google.com/docs/authentication/getting-started) guide for more details.";

            var clients = GetClientClasses(api);
            string clientClasses = CreateClientClassesDocumentation(api, clients);

            var exampleClient = clients.FirstOrDefault();
            string clientConstruction =
$@"Create a client instance by calling the static `Create` method. Alternatively,
use the builder class associated with each client class (e.g. {exampleClient}Builder for {exampleClient})
as an easy way of specifying custom credentials, settings, or a custom endpoint.";

            string nonProductStub = $@"This package is not a product in its own right; this page is
present as a root for the [API reference documentation](obj/api/{api.Id}.yml)";

            text = text
                .Replace("{{title}}", title)
                .Replace("{{description}}", description)
                .Replace("{{version}}", version)
                .Replace("{{installation}}", installation)
                .Replace("{{auth}}", auth)
                .Replace("{{client-classes}}", clientClasses)
                .Replace("{{client-construction}}", clientConstruction)
                .Replace("{{non-product-stub}}", nonProductStub);
            text = Regex.Replace(text, @"\{\{sample:([^\.]+)\.([^}]+)\}\}", "[!code-cs[](obj/snippets/" + api.Id + ".$1.txt#$2)]");
            return text;
        }

        private static string CreateClientClassesDocumentation(ApiMetadata api, List<string> clients)
        {
            clients = clients.Select(client => $"[{client}](obj/api/{api.Id}.{client}.yml)").ToList(); // Markdown link to API doc
            switch (clients.Count)
            {
                case 0: return "FIXME"; // No automatic templating for this API
                case 1: return $"All operations are performed through {clients[0]}.";
                default:
                    var list = string.Join("\r\n", clients.Select(client => $"- {client}"));
                    return $"All operations are performed through the following client classes:\r\n\r\n{list}";
            }
        }

        // TODO: Find a more robust way of detecting the clients.
        private static List<string> GetClientClasses(ApiMetadata api)
        {
            if (api.Type != ApiType.Grpc)
            {
                return new List<string>();
            }
            var layout = DirectoryLayout.ForApi(api.Id);
            var packageSource = Path.Combine(layout.SourceDirectory, api.Id);
            var sourceFiles = Directory.GetFiles(packageSource, "*Client.cs");
            return sourceFiles
                .Where(file => File.ReadAllText(file).Contains(": gaxgrpc::ServiceSettingsBase")) // Check it contains a generated client
                .Select(file => Path.GetFileName(file))             // Just the file name, not full path
                .Select(file => file.Substring(0, file.Length - 3)) // Trim .cs
                .OrderBy(client => client)
                .ToList();
        }

        private static void CreateToc(string api, string outputDirectory)
        {
            // TODO: Don't create it if it exists already?

            var tocEntries = new List<TocEntry>
            {
                new TocEntry { Name = "All APIs", Href = "../index.html" },
                new TocEntry { Name = "Home", Href = "index.md" },
            };

            // TODO: Ordering
            foreach (var file in Directory.GetFiles(outputDirectory, "*.md"))
            {
                string title = File.ReadLines(file).First().TrimStart(' ', '#');
                if (Path.GetFileName(file) == "index.md")
                {
                    continue;
                }
                tocEntries.Add(new TocEntry { Name = title, Href = Path.GetFileName(file) });
            }

            tocEntries.Add(new TocEntry { Name = "API Reference", Href = $"obj/api/{api}.yml" });

            using (var writer = File.CreateText(Path.Combine(outputDirectory, "toc.yml")))
            {
                var serializer = new SerializerBuilder().WithNamingConvention(new CamelCaseNamingConvention()).Build();
                serializer.Serialize(writer, tocEntries);
            }
        }
    }
}
