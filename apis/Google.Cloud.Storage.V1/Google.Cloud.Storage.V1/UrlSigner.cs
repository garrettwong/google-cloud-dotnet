// Copyright 2015 Google Inc. All Rights Reserved.
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

using Google.Api.Gax;
using Google.Apis.Auth.OAuth2;
using System;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Google.Cloud.Storage.V1
{
    /// <summary>
    /// Class which helps create signed URLs which can be used to provide limited access to specific buckets and objects
    /// to anyone in possession of the URL, regardless of whether they have a Google account.
    /// </summary>
    /// <remarks>
    /// See https://cloud.google.com/storage/docs/access-control/signed-urls for more information on signed URLs.
    /// </remarks>
    public sealed partial class UrlSigner
    {
        private const string StorageHost = "storage.googleapis.com";
        private static readonly ISigner s_v2Signer = new V2Signer();
        private static readonly ISigner s_v4Signer = new V4Signer();

        private const string GoogHeaderPrefix = "x-goog-";

        /// <summary>
        /// Gets a special HTTP method which can be used to create a signed URL for initiating a resumable upload.
        /// See https://cloud.google.com/storage/docs/access-control/signed-urls#signing-resumable for more information.
        /// </summary>
        /// <remarks>
        /// Note: When using the RESUMABLE method to create a signed URL, a URL will actually be signed for the POST method with a header of
        /// 'x-goog-resumable:start'. The caller must perform a POST request with this URL and specify the 'x-goog-resumable:start' header as
        /// well or signature validation will fail.
        /// </remarks>
        public static HttpMethod ResumableHttpMethod { get; } = new HttpMethod("RESUMABLE");

        private static readonly DateTimeOffset UnixEpoch = new DateTimeOffset(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc), TimeSpan.Zero);

        private readonly IBlobSigner _blobSigner;
        private readonly IClock _clock;

        private UrlSigner(IBlobSigner blobSigner, IClock clock)
        {
            _blobSigner = blobSigner;
            _clock = clock;
        }

        /// <summary>
        /// Creates a new <see cref="UrlSigner"/> instance for a service account.
        /// </summary>
        /// <param name="credentialFilePath">The path to the JSON key file for a service account. Must not be null.</param>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="credentialFilePath"/> does not refer to a valid JSON service account key file.
        /// </exception>
        public static UrlSigner FromServiceAccountPath(string credentialFilePath)
        {
            GaxPreconditions.CheckNotNull(credentialFilePath, nameof(credentialFilePath));
            using (var credentialData = File.OpenRead(credentialFilePath))
            {
                return FromServiceAccountData(credentialData);
            }
        }

        /// <summary>
        /// Creates a new <see cref="UrlSigner"/> instance for a service account.
        /// </summary>
        /// <param name="credentialData">The stream from which to read the JSON key data for a service account. Must not be null.</param>
        /// <exception cref="InvalidOperationException">
        /// The <paramref name="credentialData"/> does not contain valid JSON service account key data.
        /// </exception>
        public static UrlSigner FromServiceAccountData(Stream credentialData)
        {
            GaxPreconditions.CheckNotNull(credentialData, nameof(credentialData));
            return UrlSigner.FromServiceAccountCredential(ServiceAccountCredential.FromServiceAccountData(credentialData));
        }

        /// <summary>
        /// Creates a new <see cref="UrlSigner"/> instance for a service account.
        /// </summary>
        /// <param name="credential">The credential for the a service account. Must not be null.</param>
        public static UrlSigner FromServiceAccountCredential(ServiceAccountCredential credential)
        {
            GaxPreconditions.CheckNotNull(credential, nameof(credential));
            return new UrlSigner(new ServiceAccountCredentialBlobSigner(credential), SystemClock.Instance);
        }

        /// <summary>
        /// Creates a new <see cref="UrlSigner"/> instance for a custom blob signer.
        /// </summary>
        /// <remarks>
        /// This method is typically used when a service account credential file isn't available, either
        /// for testing or to use the IAM service's blob signing capabilities.
        /// </remarks>
        /// <param name="signer">The blob signer to use. Must not be null.</param>
        /// <returns>A new <see cref="UrlSigner"/> using the specified blob signer.</returns>
        public static UrlSigner FromBlobSigner(IBlobSigner signer)
        {
            GaxPreconditions.CheckNotNull(signer, nameof(signer));
            return new UrlSigner(signer, SystemClock.Instance);
        }

        /// <summary>
        /// Only available for testing purposes, this allows the clock used for signature generation to be replaced.
        /// </summary>
        internal UrlSigner WithClock(IClock clock) => new UrlSigner(_blobSigner, clock);

        /// <summary>
        /// Creates a signed URL which can be used to provide limited access to specific buckets and objects to anyone
        /// in possession of the URL, regardless of whether they have a Google account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/access-control/signed-urls for more information on signed URLs.
        /// </para>
        /// <para>
        /// Note that when GET is specified as the <paramref name="httpMethod"/> (or it is null, in which case GET is
        /// used), both GET and HEAD requests can be made with the created signed URL.
        /// </para>
        /// </remarks>
        /// <param name="bucket">The name of the bucket. Must not be null.</param>
        /// <param name="objectName">The name of the object within the bucket. May be null, in which case the signed URL
        /// will refer to the bucket instead of an object.</param>
        /// <param name="duration">The length of time for which the signed URL should remain usable.</param>
        /// <param name="httpMethod">The HTTP request method for which the signed URL is allowed to be used. May be null,
        /// in which case GET will be used.</param>
        /// <param name="signingVersion">The signing version to use to generate the signed URL. May be null, in which case
        /// <see cref="SigningVersion.Default"/> will be used.</param>
        /// <returns>The signed URL which can be used to provide access to a bucket or object for a limited amount of time.</returns>
        public string Sign(string bucket, string objectName, TimeSpan duration, HttpMethod httpMethod = null, SigningVersion? signingVersion = null)
        {
            var template = RequestTemplate
                .FromBucket(bucket)
                .WithObjectName(objectName)
                .WithHttpMethod(httpMethod);
            var options = Options.FromDuration(duration);
            if (signingVersion.HasValue)
            {
                options = options.WithSigningVersion(signingVersion.Value);
            }
            return Sign(template, options);
        }

        /// <summary>
        /// Creates a signed URL which can be used to provide limited access to specific buckets and objects to anyone
        /// in possession of the URL, regardless of whether they have a Google account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/access-control/signed-urls for more information on signed URLs.
        /// </para>
        /// </remarks>
        /// <param name="requestTemplate">The request template that will be used to generate the signed URL for. Must not be null.</param>
        /// <param name="options">The options used to generate the signed URL. Must not be null.</param>
        /// <returns>
        /// The signed URL which can be used to provide access to a bucket or object for a limited amount of time.
        /// </returns>
        public string Sign(RequestTemplate requestTemplate, Options options) =>
            GetEffectiveSigner(GaxPreconditions.CheckNotNull(options, nameof(options)).SigningVersion).Sign(
                GaxPreconditions.CheckNotNull(requestTemplate, nameof(requestTemplate)), options, _blobSigner, _clock);

        /// <summary>
        /// Signs the given post policy. The result can be used to make form posting requests matching the conditions
        /// set in the post policy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Signing post policies is not supported by <see cref="SigningVersion.V2"/>. A <see cref="NotSupportedException"/>
        /// will be thrown if an attempt is made to sign a post policy using <see cref="SigningVersion.V2"/>.
        /// </para>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/xml-api/post-object for more information on signed post policies.
        /// </para>
        /// </remarks>
        /// <param name="postPolicy">The post policy to signed and that will be enforced when making the post request.
        /// Must not be null.</param>
        /// <param name="options">The options used to generate the signed post policy. Must not be null.</param>
        /// <returns>The signed post policy, which contains all the fields that should be including in the form to post.</returns>
        public SignedPostPolicy Sign(PostPolicy postPolicy, Options options) =>
            GetEffectiveSigner(GaxPreconditions.CheckNotNull(options, nameof(options)).SigningVersion).Sign(
                GaxPreconditions.CheckNotNull(postPolicy, nameof(postPolicy)), options, _blobSigner, _clock);

        /// <summary>
        /// Creates a signed URL which can be used to provide limited access to specific buckets and objects to anyone
        /// in possession of the URL, regardless of whether they have a Google account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/access-control/signed-urls for more information on signed URLs.
        /// </para>
        /// <para>
        /// Note that when GET is specified as the <paramref name="httpMethod"/> (or it is null, in which case GET is
        /// used), both GET and HEAD requests can be made with the created signed URL.
        /// </para>
        /// </remarks>
        /// <param name="bucket">The name of the bucket. Must not be null.</param>
        /// <param name="objectName">The name of the object within the bucket. May be null, in which case the signed URL
        /// will refer to the bucket instead of an object.</param>
        /// <param name="duration">The length of time for which the signed URL should remain usable.</param>
        /// <param name="httpMethod">The HTTP request method for which the signed URL is allowed to be used. May be null,
        /// in which case GET will be used.</param>
        /// <param name="signingVersion">The signing version to use to generate the signed URL. May be null, in which case
        /// <see cref="SigningVersion.Default"/> will be used.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns> A task representing the asynchronous operation, with a result returning the
        /// signed URL which can be used to provide access to a bucket or object for a limited amount of time.</returns>
        public async Task<string> SignAsync(string bucket, string objectName, TimeSpan duration, HttpMethod httpMethod = null, SigningVersion? signingVersion = null, CancellationToken cancellationToken = default)
        {
            var template = RequestTemplate
                .FromBucket(bucket)
                .WithObjectName(objectName)
                .WithHttpMethod(httpMethod);
            var options = Options.FromDuration(duration);
            if (signingVersion.HasValue)
            {
                options = options.WithSigningVersion(signingVersion.Value);
            }
            return await SignAsync(template, options, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Asynchronously creates a signed URL which can be used to provide limited access to specific buckets and objects to anyone
        /// in possession of the URL, regardless of whether they have a Google account.
        /// </summary>
        /// <remarks>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/access-control/signed-urls for more information on signed URLs.
        /// </para>
        /// </remarks>
        /// <param name="requestTemplate">The request template that will be used to generate the signed URL for. Must not be null.</param>
        /// <param name="options">The options used to generate the signed URL. Must not be null.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task representing the asynchronous operation, with a result returning the
        /// signed URL which can be used to provide access to a bucket or object for a limited amount of time.
        /// </returns>
        public Task<string> SignAsync(RequestTemplate requestTemplate, Options options, CancellationToken cancellationToken = default) =>
            GetEffectiveSigner(GaxPreconditions.CheckNotNull(options, nameof(options)).SigningVersion).SignAsync(
                GaxPreconditions.CheckNotNull(requestTemplate, nameof(requestTemplate)), options, _blobSigner, _clock, cancellationToken);

        /// <summary>
        /// Signs the given post policy. The result can be used to make form posting requests matching the conditions
        /// set in the post policy.
        /// </summary>
        /// <remarks>
        /// <para>
        /// Signing post policies is not supported by <see cref="SigningVersion.V2"/>. A <see cref="NotSupportedException"/>
        /// will be thrown if an attempt is made to sign a post policy using <see cref="SigningVersion.V2"/>.
        /// </para>
        /// <para>
        /// When a <see cref="UrlSigner"/> is created with a service account credential, the signing can be performed
        /// with no network access. When it is created with an implementation of <see cref="IBlobSigner"/>, that may require
        /// network access or other IO. In that case, one of the asynchronous methods should be used when the caller is
        /// in a context that should not block.
        /// </para>
        /// <para>
        /// See https://cloud.google.com/storage/docs/xml-api/post-object for more information on signed post policies.
        /// </para>
        /// </remarks>
        /// <param name="postPolicy">The post policy to signed and that will be enforced when making the post request.
        /// Most not be null.</param>
        /// <param name="options">The options used to generate the signed post policy. Must not be null.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <returns>The signed post policy, which contains all the fields that should be including in the form to post.</returns>
        public Task<SignedPostPolicy> SignAsync(PostPolicy postPolicy, Options options, CancellationToken cancellationToken = default) =>
            GetEffectiveSigner(GaxPreconditions.CheckNotNull(options, nameof(options)).SigningVersion).SignAsync(
                GaxPreconditions.CheckNotNull(postPolicy, nameof(postPolicy)), options, _blobSigner, _clock, cancellationToken);

        private ISigner GetEffectiveSigner(SigningVersion signingVersion) =>
            signingVersion switch
            {
                SigningVersion.Default => s_v4Signer,
                SigningVersion.V2 => s_v2Signer,
                SigningVersion.V4 => s_v4Signer,
                // We really shouldn't get here, as we validate any user-specified signing version.
                _ => throw new InvalidOperationException($"Invalid signing version: {signingVersion}")
            };

        private static readonly Regex s_newlineRegex = new Regex(@"\r?\n", RegexOptions.Compiled);
        private static readonly Regex s_tabRegex = new Regex(@"\t+", RegexOptions.Compiled);
        private static readonly Regex s_whitespaceRegex = new Regex(@"\s+", RegexOptions.Compiled);

        /// <summary>
        /// Prepares a header value for signing, trimming both ends and collapsing internal whitespace.
        /// </summary>
        internal static string PrepareHeaderValue(string value, bool collapseTabs)
        {
            // Remove leading/trailing whitespace
            value = value.Trim();

            if (collapseTabs)
            {
                // Replaces all consecutive tabs by a space.
                // If consecutive spaces result out of this, then the next line will
                // collapse all the spaces.
                value = s_tabRegex.Replace(value, " ");
            }

            // Collapse whitespace runs: only keep the last character
            value = s_whitespaceRegex.Replace(value, match => match.Value[match.Value.Length - 1].ToString());

            // Remove newlines
            value = s_newlineRegex.Replace(value, "");

            return value;
        }

    }
}
