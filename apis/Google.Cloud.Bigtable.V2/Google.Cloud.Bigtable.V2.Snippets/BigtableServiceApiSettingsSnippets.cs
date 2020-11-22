﻿// Copyright 2017 Google Inc. All rights reserved.
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

using Google.Cloud.ClientTesting;
using Xunit;

namespace Google.Cloud.Bigtable.V2.Snippets
{
    [SnippetOutputCollector]
    [Collection(nameof(BigtableClientSnippetsFixture))]
    public class BigtableServiceApiSettingsSnippets
    {
        private readonly BigtableClientSnippetsFixture _fixture;

        public BigtableServiceApiSettingsSnippets(BigtableClientSnippetsFixture fixture) => _fixture = fixture;

        [Fact]
        public void CallInvokerSharing()
        {
            // Sample: CallInvokerSharing
            // Additional: MaxChannels
            // Additional: PreferredMaxStreamsPerChannel
            // client1 and client3 will share the same GcpCallInvoker, since they both
            // use the same default settings. This means they will use the same shared
            // set of gRPC channels to the Bigtable service.
            // client2 and client4 will share a different GcpCallInvoker, since they both
            // use the same custom MaxChannels/PreferredMaxStreamsPerChannel settings.
            BigtableServiceApiSettings settings1 = BigtableServiceApiSettings.GetDefault();
            BigtableClient client1 = new BigtableClientBuilder { Settings = settings1 }.Build();

            BigtableServiceApiSettings settings2 = new BigtableServiceApiSettings
            {
                MaxChannels = 10,
                PreferredMaxStreamsPerChannel = 4
            };
            BigtableClient client2 = new BigtableClientBuilder { Settings = settings2 }.Build();

            BigtableServiceApiSettings settings3 = BigtableServiceApiSettings.GetDefault();
            BigtableClient client3 = new BigtableClientBuilder { Settings = settings3 }.Build();

            BigtableServiceApiSettings settings4 = new BigtableServiceApiSettings
            {
                MaxChannels = 10,
                PreferredMaxStreamsPerChannel = 4
            };
            BigtableClient client4 = new BigtableClientBuilder { Settings = settings4 }.Build();

            // client5 will not share a GcpCallInvoker with any of the other clients, since
            // its MaxChannels/PreferredMaxStreamsPerChannel settings differ from the others.
            BigtableServiceApiSettings settings5 = new BigtableServiceApiSettings
            {
                MaxChannels = 15,
                PreferredMaxStreamsPerChannel = 4
            };
            BigtableClient client5 = new BigtableClientBuilder { Settings = settings5 }.Build();
            // End sample
        }
    }
}
