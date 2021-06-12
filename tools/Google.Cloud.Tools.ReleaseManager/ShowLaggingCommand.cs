﻿// Copyright 2020 Google LLC
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
using LibGit2Sharp;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Google.Cloud.Tools.ReleaseManager
{
    /// <summary>
    /// Command to show packages whose last release was a pre-release, but which represent a GA
    /// API (and should therefore be considered for a GA release).
    /// </summary>
    public class ShowLaggingCommand : CommandBase
    {
        public ShowLaggingCommand() : base("show-lagging", "Shows pre-release packages where a GA should be considered")
        {
        }

        protected override void ExecuteImpl(string[] args)
        {
            Console.WriteLine($"Lagging packages (package ID, current version, date range of current version prerelease series):");
            var root = DirectoryLayout.DetermineRootDirectory();
            var catalog = ApiCatalog.Load();
            using (var repo = new Repository(root))
            {
                var allTags = repo.Tags.OrderByDescending(GitHelpers.GetDate).ToList();
                foreach (var api in catalog.Apis)
                {
                    MaybeShowLagging(allTags, api);
                }
            }
        }

        private static void MaybeShowLagging(List<Tag> allTags, ApiMetadata api)
        {
            var currentVersion = api.StructuredVersion;
            // Skip anything that is naturally pre-release (in the API), or where the current release is GA already.
            if (!api.CanHaveGaRelease || currentVersion.Prerelease is null)
            {
                return;
            }

            // Find all the existing prereleases for the expected "next GA" release.
            var expectedGa = StructuredVersion.FromMajorMinorPatch(currentVersion.Major, currentVersion.Minor, currentVersion.Patch, prerelease: null);
            string expectedGaPrefix = $"{api.Id}-{expectedGa}";
            var matchingReleaseTags = allTags.Where(tag => tag.FriendlyName.StartsWith(expectedGaPrefix, StringComparison.Ordinal)).ToList();

            // Skip if we haven't even released the current prerelease.
            if (matchingReleaseTags.Count == 0)
            {
                return;
            }

            var latest = GitHelpers.GetDate(matchingReleaseTags.First());
            var earliest = GitHelpers.GetDate(matchingReleaseTags.Last());

            string dateRange = latest == earliest
                ? $"{latest:yyyy-MM-dd}"
                : $"{earliest:yyyy-MM-dd} - {latest:yyyy-MM-dd}";
            Console.WriteLine($"{api.Id,-50}{api.Version,-20}{dateRange}");
        }
    }
}
