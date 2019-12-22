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

using Google.Api;
using Google.Cloud.ClientTesting;
using Google.Cloud.Diagnostics.Common;
using Google.Cloud.Diagnostics.Common.IntegrationTests;
using Google.Cloud.Logging.Type;
using Google.Protobuf.WellKnownTypes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Google.Cloud.Diagnostics.AspNetCore.IntegrationTests
{
    public class DiagnosticsWebHostTests
    {
        public DiagnosticsWebHostTests()
        {
            // The rate limiter instance is static and only set once.  If we do not reset it at the
            // beginning of each tests the qps will not change.  This is dependent on the tests not
            // running in parallel.
            RateLimiter.Reset();
        }

        [Fact]
        public void UseGoogleDiagnostics_ConfiguresServices()
        {
            var webHostBuilder = new WebHostBuilder()
                .Configure(app => { })
                .UseGoogleDiagnostics("tmp", "app", "1.0.0");

            using (var server = new TestServer(webHostBuilder))
            {
                var services = server.Host.Services;

                // Test Google diagnostics startup filter
                var startupFilters = services.GetServices<IStartupFilter>();
                Assert.NotNull(startupFilters.FirstOrDefault(r => r is GoogleDiagnosticsStartupFilter));

                // Test tracing
                Assert.NotNull(services.GetService<IHttpContextAccessor>());
                Assert.NotNull(services.GetService<IManagedTracer>());

                // Test exception logging
                Assert.NotNull(services.GetService<IExceptionLogger>());
                Assert.NotNull(services.GetService<IContextExceptionLogger>());
            }
        }

        [Fact]
        public async Task UseGoogleDiagnostics_ConfiguresComponents()
        {
            var testId = IdGenerator.FromDateTime();
            var startTime = DateTime.UtcNow;
            var resource = new MonitoredResource
            {
                Type = "global",
                Labels =
                {
                    { "project_id", TestEnvironment.GetTestProjectId() },
                    { "module_id", EntryData.Service },
                    { "version_id", EntryData.Version },
                    { "build_id", "some-build-id" }
                }
            };
            // We won't be able to detect the right monitored resource, so specify it explicitly.
            var loggerOptions = LoggerOptions.Create(monitoredResource: resource);
            var webHostBuilder = new WebHostBuilder()
                .ConfigureServices(services => services.AddMvcCore())
                .Configure(app => app.UseMvcWithDefaultRoute())
                .UseGoogleDiagnostics(TestEnvironment.GetTestProjectId(), EntryData.Service, EntryData.Version, loggerOptions);

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                await TestTrace(testId, startTime, client);
                await TestLogging(testId, startTime, client);
                await TestErrorReporting(testId, client);
            }
        }

        [Fact]
        public async Task UseGoogleDiagnostics_ConfiguresComponentsFromHostBuilderContext()
        {
            var testId = IdGenerator.FromDateTime();
            var startTime = DateTime.UtcNow;
            var configurationData = new Dictionary<string, string>
            {
                { "project_id", TestEnvironment.GetTestProjectId() },
                { "module_id", EntryData.Service },
                { "version_id", EntryData.Version }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(configurationData)
                .Build();

            var webHostBuilder = new WebHostBuilder()
                .UseConfiguration(configuration)
                .ConfigureServices(services => services.AddMvcCore())
                .Configure(app => app.UseMvcWithDefaultRoute())
                .UseGoogleDiagnostics(
                    ctx => ctx.Configuration["project_id"],
                    ctx => ctx.Configuration["module_id"],
                    ctx => ctx.Configuration["version_id"]
                );

            using (var server = new TestServer(webHostBuilder))
            using (var client = server.CreateClient())
            {
                await TestTrace(testId, startTime, client);
                await TestLogging(testId, startTime, client);
                await TestErrorReporting(testId, client);
            }
        }

        private static async Task TestLogging(string testId, DateTime startTime, HttpClient client)
        {
            var polling = new LogEntryPolling();
            await client.GetAsync($"/Main/Warning/{testId}");
            var results = polling.GetEntries(startTime, testId, 1, LogSeverity.Warning);

            Assert.Single(results);
            var result = results.Single();
            Assert.Single(result.Labels);
            var label = result.Labels.Single();
            Assert.Equal("trace_identifier", label.Key);
            Assert.NotEmpty(label.Value);
        }

        private static async Task TestTrace(string testId, DateTime startTime, HttpClient client)
        {
            var uri = $"/Trace/{nameof(TraceController.Trace)}/{testId}";
            var childSpanName = EntryData.GetMessage(nameof(TraceController.Trace), testId);

            var response = await client.GetAsync(uri);

            // Give the polling a little extra time to find the trace as
            // trace processing can sometimes take time and the default buffer is a
            // timed buffer.
            var polling = new TraceEntryPolling(TimeSpan.FromSeconds(20));
            var trace = polling.GetTrace(uri, Timestamp.FromDateTime(startTime));

            TraceEntryVerifiers.AssertParentChildSpan(trace, uri, childSpanName);
            TraceEntryVerifiers.AssertSpanLabelsContains(
                trace.Spans.First(s => s.Name == uri), TraceEntryData.HttpGetSuccessLabels);

            Assert.False(response.Headers.Contains(TraceHeaderContext.TraceHeader));
        }

        private static async Task TestErrorReporting(string testId, HttpClient client)
        {
            var polling = new ErrorEventEntryPolling();
            await Assert.ThrowsAsync<Exception>(() => client.GetAsync($"/ErrorReporting/{nameof(ErrorReportingController.ThrowsException)}/{testId}"));
            var errorEvent = ErrorEventEntryVerifiers.VerifySingle(polling, testId);
            ErrorEventEntryVerifiers.VerifyFullErrorEventLogged(errorEvent, testId, nameof(ErrorReportingController.ThrowsException));
        }
    }
}
