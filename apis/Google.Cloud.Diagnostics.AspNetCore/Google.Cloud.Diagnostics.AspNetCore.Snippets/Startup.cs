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

#if NETCOREAPP3_1
namespace Google.Cloud.Diagnostics.AspNetCore3.Snippets
#elif NETCOREAPP2_1 || NET461
namespace Google.Cloud.Diagnostics.AspNetCore.Snippets
#else
#error unknown target framework
#endif
{
    using static IntegrationTests.TestServerHelpers;

    /// <summary>
    /// A simple web application to use as a default Startup.
    /// </summary>
    internal class Startup : BaseStartup
    { }
}
