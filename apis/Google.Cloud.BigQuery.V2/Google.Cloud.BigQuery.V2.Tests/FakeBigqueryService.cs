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

using Google.Apis.Bigquery.v2;
using Google.Apis.Requests;
using Google.Apis.Util;
using Google.Cloud.ClientTesting;
using System.Net;
using System.Net.Http;

namespace Google.Cloud.BigQuery.V2.Tests
{
    /// <summary>
    /// A fake service allowing request/response expect/replay.
    /// </summary>
    public class FakeBigqueryService : BigqueryService
    {
        private readonly ReplayingMessageHandler handler;

        public FakeBigqueryService() : base(new Initializer
        {
            HttpClientFactory = new FakeHttpClientFactory(new ReplayingMessageHandler()),
            ApplicationName = "Fake",
            GZipEnabled = false
        })
        {
            handler = (ReplayingMessageHandler) HttpClient.MessageHandler.InnerHandler;
        }

        public void ExpectRequest<TResponse>(ClientServiceRequest<TResponse> request, TResponse response)
        {
            MaybeDisablePrettyPrint(request);
            string requestContent = SerializeObject(request);
            var httpRequest = request.CreateRequest();
            string responseContent = SerializeObject(response);            
            handler.ExpectRequest(httpRequest.RequestUri, httpRequest.Content?.ReadAsStringAsync()?.Result, responseContent);
        }

        public void ExpectRequest<TResponse>(ClientServiceRequest<TResponse> request, HttpStatusCode statusCode, RequestError error)
        {
            MaybeDisablePrettyPrint(request);
            string requestContent = SerializeObject(request);
            var httpRequest = request.CreateRequest();
            string responseContent = SerializeObject(new StandardResponse<object> { Error = error });
            var responseMessage = new HttpResponseMessage(statusCode) { Content = new StringContent(responseContent) };
            handler.ExpectRequest(httpRequest.RequestUri, httpRequest.Content?.ReadAsStringAsync()?.Result, responseMessage);
        }

        private void MaybeDisablePrettyPrint(dynamic request)
        {
            // Almost all our tests using FakeBigqueryService will have prettyPrint=false, and the requests will all
            // be derived from the generic BigqueryServiceBaseRequest which has a PrettyPrint property.
            // We use ??= so that if the request already has PrettyPrint set, we don't change that.
            request.PrettyPrint ??= false;
        }

        public void Verify() => handler.Verify();
    }
}
