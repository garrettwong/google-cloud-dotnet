﻿// Copyright 2018 Google Inc. All Rights Reserved.
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

namespace Google.Cloud.Diagnostics.Common.IntegrationTests
{
    /// <summary>
    /// Class that contains data and helper methods used to include data in
    /// log entries.
    /// </summary>
    public static class EntryData
    {
        public const string Service = "service-name";
        public const string Version = "version-id";

        /// <summary>
        /// Returns a formatted message based on the two parametes.
        /// </summary>
        public static string GetMessage(string message, string id) => $"{message} - {id}";

        public static string SpanIdToHex(ulong? spanId) => spanId is null ? null : $"{spanId:x16}";
    }
}
