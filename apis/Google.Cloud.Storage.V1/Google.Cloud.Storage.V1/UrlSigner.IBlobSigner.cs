﻿// Copyright 2018 Google LLC
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

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Cloud.Storage.V1
{
    public sealed partial class UrlSigner
    {
        /// <summary>
        /// Represents the capability of signing a blob in a suitable form for Google Cloud Storage signed URLs.
        /// This allows testing URL signing without credentials being available, as well as using Google Cloud IAM
        /// to sign blobs.
        /// </summary>
        public interface IBlobSigner
        {
            /// <summary>
            /// Synchronously signs the given blob.
            /// </summary>
            /// <param name="data">The data to sign. Must not be null.</param>
            /// <returns>The blob signature as base64 text.</returns>
            string CreateSignature(byte[] data);

            /// <summary>
            /// Asynchronously signs the given blob.
            /// </summary>
            /// <param name="data">The data to sign. Must not be null.</param>
            /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
            /// <returns>A task representing the asynchronous operation, with a result returning the
            /// blob signature as base64 text.
            /// </returns>
            Task<string> CreateSignatureAsync(byte[] data, CancellationToken cancellationToken);

            /// <summary>
            /// The identity of the signer, typically an email address.
            /// </summary>
            string Id { get; }
        }
    }
}
