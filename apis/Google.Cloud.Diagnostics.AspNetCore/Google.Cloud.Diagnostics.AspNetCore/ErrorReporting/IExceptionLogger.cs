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

using Microsoft.AspNetCore.Http;
using System;
using System.Threading;
using System.Threading.Tasks;

#if NETCOREAPP3_1
namespace Google.Cloud.Diagnostics.AspNetCore3
#elif NETSTANDARD2_0
namespace Google.Cloud.Diagnostics.AspNetCore
#else
#error unknown target framework
#endif
{
    /// <summary>
    /// A generic exception logger.
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// Logs an exception that occurred.
        /// </summary>
        /// <param name="exception">The exception to log. Must not be null.</param>
        /// <param name="context">Optional, the current HTTP context. If unset the
        ///     current context will be retrieved automatically.</param>
        void Log(Exception exception, HttpContext context = null);

        /// <summary>
        /// Asynchronously logs an exception that occurred.
        /// </summary>
        /// <param name="exception">The exception to log. Must not be null.</param>
        /// <param name="context">Optional, the current HTTP context. If unset the
        ///     current context will be retrieved automatically.</param>
        /// <param name="cancellationToken">Optional, The token to monitor for cancellation requests.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogAsync(Exception exception, HttpContext context = null, CancellationToken cancellationToken = default);
    }
}
