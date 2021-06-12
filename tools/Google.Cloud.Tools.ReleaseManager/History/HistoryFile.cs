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

using Google.Cloud.Tools.Common;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Google.Cloud.Tools.ReleaseManager.History
{
    /// <summary>
    /// A history file, which is basically a list of sections. Each section has
    /// a version and release date - except potentially the first section, which might contain
    /// some preamble. All sections then have a list of lines.
    /// </summary>
    internal sealed class HistoryFile
    {
        private const string MarkdownFile = "history.md";
        private static readonly Regex SectionHeader = new Regex(@"# Version (.*), released \d{4}-\d{2}-\d{2}");

        /// <summary>
        /// The sections within the history file.
        /// </summary>
        public List<Section> Sections { get; }

        private HistoryFile(List<Section> sections) => Sections = sections;

        public static string GetPathForPackage(string id) => Path.Combine(DirectoryLayout.ForApi(id).DocsSourceDirectory, MarkdownFile);

        public static HistoryFile Load(string file)
        {
            var sections = new List<Section>();
            var lines = File.ReadAllLines(file);

            StructuredVersion currentVersion = null;
            var currentSectionLines = new List<string>();

            foreach (var line in lines)
            {
                var match = SectionHeader.Match(line);
                if (match.Success)
                {
                    sections.Add(new Section(currentVersion, currentSectionLines));
                    currentSectionLines = new List<string>();
                    currentVersion = StructuredVersion.FromString(match.Groups[1].Value);
                }
                currentSectionLines.Add(line);
            }
            sections.Add(new Section(currentVersion, currentSectionLines));
            return new HistoryFile(sections);
        }

        public void Save(string file) =>
            File.WriteAllLines(file, Sections.SelectMany(section => section.Lines));

        /// <summary>
        /// Merges the given list of releases into the file, ignoring releases that are already present.
        /// </summary>
        /// <param name="releases">The list of releases to merge, in reverse-chronological order (so latest first).</param>
        /// <returns>The new sections inserted into the history.</returns>
        internal List<Section> MergeReleases(List<Release> releases)
        {
            List<Section> sectionsInserted = new List<Section>();

            int latestExistingVersionIndex = Sections.FindIndex(s => s.Version != null);

            var latestExistingVersion = latestExistingVersionIndex == -1 ? null : Sections[latestExistingVersionIndex].Version;
            var insertIndex = latestExistingVersionIndex == -1 ? Sections.Count : latestExistingVersionIndex;

            foreach (var release in releases)
            {
                // If we've reached an already-documented version, stop.
                if (release.Version.Equals(latestExistingVersion))
                {
                    break;
                }
                Section section = new Section(release);
                Sections.Insert(insertIndex, section);
                sectionsInserted.Add(section);
                insertIndex++;
            }
            return sectionsInserted;
        }

        public sealed class Section
        {
            private static readonly StructuredVersion s_expectedInitialReleaseVersion = StructuredVersion.FromString("1.0.0-beta01");

            public StructuredVersion Version { get; }

            /// <summary>
            /// The lines for the section, including the section header.
            /// </summary>
            public List<string> Lines { get; }

            public Section(StructuredVersion version, List<string> lines)
            {
                Version = version;
                Lines = lines;
            }

            internal Section(Release release)
            {
                Version = release.Version;

                Lines = new List<string>();

                Lines.Add($"# Version {Version}, released {release.ReleaseDate:yyyy-MM-dd}");
                Lines.Add("");
                if (Version.Equals(s_expectedInitialReleaseVersion))
                {
                    Lines.Add("Initial release.");
                }
                else
                {
                    foreach (var commit in release.Commits)
                    {
                        Lines.AddRange(commit.GetHistoryLines());
                    }
                    // No "interesting" commits? That usually means it's just a dependency update.
                    if (Lines.Count == 2)
                    {
                        Lines.Add("No API surface changes; just dependency updates.");
                    }
                    Lines.Add("");
                }
            }
        }
    }
}
