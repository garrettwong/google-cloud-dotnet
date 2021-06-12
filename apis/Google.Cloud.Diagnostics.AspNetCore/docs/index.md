{{title}}

`Google.Cloud.Diagnostics.AspNetCore` is an ASP.NET Core instrumentation library for Google Logging, Error Reporting and Tracing.
It allows for simple integration of Google observability components into ASP.NET Core 2.1+ applications with minimal code changes.

Note: ASP.NET Core 2.1 is the long-term support release of ASP.NET Core 2.x.

{{version}}

{{installation}}

{{auth}}

See [API Permissions for `entries.write`](https://cloud.google.com/logging/docs/access-control#api-permissions)
for the permissions needed for Logging and Error Reporting.

See [API Permissions for PatchTraces](https://cloud.google.com/trace/docs/iam#api_permissions)
for the permissions needed for Tracing.

# Note
The Google.Cloud.Diagnostics.AspNetCore package attempts to collect the filename and line number when
entries are collected. However, to be able to collect this information PDBs must be included with
the deployed code.

# Note
When running on environments that limit or disable CPU usage for background activities, for instance
[Google Cloud Run](https://cloud.google.com/run/docs/tips/general#avoiding_background_activities), take care
not to use the timed buffer options for any of Logging, Tracing or Error Reporting. Take into account
that the timed buffer is used for all of these components by default so you will need to explicitly
configure the buffers by using the `Google.Cloud.Diagnostics.AspNetCore.LoggerOptions`,
`Google.Cloud.Diagnostics.Common.TraceOptions` and `Google.Cloud.Diagnostics.Common.ErrorReportingOptions` classes.
Below you'll find examples of how to configure the buffers.

# Getting started

## Initializing Google Diagnostics

The easiest way to initialize Google Diagnostics services is using
the `UseGoogleDiagnostics` extentension method on `IWebHostBuilder`.
This configures Logging, Tracing and Error Reporting middleware.

If your application is runnng on Google App Engine, Google
Kubernetes Engine, Google Cloud Run or Google Compute Engine, you
don't need to provide a value for `ProjectId`, `Service` and
`Version` since they can be automatically obtained by the
`UseGoogleDiagnostics` method as far as they make sense for the
environment. (Not every environment has the concept of a "service"
or "version".) The values used will be the ones associated with the running application.

If your application is running outside of GCP, including when it
runs locally, then you'll need to provide the `ProjectId` of the
Google Cloud Project in which to store the diagnostic information as
well as the `Service` and `Version` with which to identify your
application.

{{sample:Diagnostics.UseGoogleDiagnostics}}

You can still initialize the separate components using the extension
methods below. This can be useful if you only need to use some of
the observability components.

Optional parameters on `UseGoogleDiagnostics` are also available to
specify options for each of the components (logging, tracing and
error reporting). This is typically useful for diagnosing problems,
as described below.

# Error Reporting

## Registering Error Reporting

{{sample:ErrorReporting.ReportUnhandledExceptions}}

## Log Exceptions

{{sample:ErrorReporting.LogExceptions}}

# Logging

## Initializing Logging

When configuring an `IWebHostBuilder` Logging can be initialized in
two different ways:

Using `ConfigureServices`:

{{sample:Logging.RegisterGoogleLogger2}}

Or using `ConfigureLogging`:

{{sample:Logging.RegisterGoogleLogger3}}

Note that this approach does not support custom services (such as
log entry label providers) being used as the service provider is
not available within `ConfigureLogging`.

Alternatively, logging can be configured within the application's
`Startup.Configure` method:

{{sample:Logging.RegisterGoogleLogger}}

## Log

{{sample:Logging.UseGoogleLogger}}

## Troubleshooting Logging

Sometimes it is neccessary to diagnose log operations. It might be that logging is failing or that
we simply cannot find where the logs are being stored in GCP. What follows are a couple of code samples
that can be useful to find out what might be wrong with logging operations.

### Propagating Exceptions

By default the Google Logger won't propagate any exceptions thrown during logging. This is to protect the
application from crashing if logging is not possible. But logging is an important aspect of most applications
so at times we need to know if it's failing and why. The following
example shows how to configure Google Logger so that it propagates exceptions thrown during logging.

{{sample:Logging.RegisterGoogleLoggerPropagateExceptions}}

The same `LoggerOptions` can be specified in any of the other ways
of registering logging.

### Finding out the URL where logs are written

Depending on where your code is running and the options you provided
for creating a Google Logger, it might be hard to find your logs in
the GCP Console. We have provided a way for you to obtain the URL
where your logs can be found.

As the following code sample shows, you only need to pass a
`System.IO.TextWriter` (typically `Console.Out` or `Console.Error`)
as part of the options when registering logging. When the
`GoogleLoggerProvider` is initialized, the URL where its logs can be
found will be written to the given text writer.

{{sample:Logging.RegisterGoogleLoggerWriteUrl}}

Please note that since this is a Google Logger diagnostics feature,
we don't respect settings for exception handling, i.e. we propagate
any exception thrown while writing the URL to the given text writer
so you know what might be happening. This feature should only be
activated as a one off, if you are having trouble trying to find
your logs in the GCP Console, and not as a permanent feature in
production code. To deactivate this feature simply stop passing a
`System.IO.TextWriter` as part of the options when creating a Google
Logger.

# Tracing

## Initializing Tracing

{{sample:Trace.RegisterGoogleTracer}}

## Troubleshooting Tracing

Just as with logging, trace is most easily diagnosed by removing
buffering and propagating exceptions immediately.

{{sample:Trace.Troubleshooting}}

The options can be specified wherever you are configuring tracing.

## Tracing in Controllers

To use the `IManagedTracer` in controllers you can either inject the singleton instance of 
`IManagedTracer` into the controller's constructor (see `TraceSamplesConstructorController`) or you
can inject the `IManagedTracer` into the action method using the `[FromServices]` attribute
(see `TraceSamplesMethodController`).

{{sample:Trace.TraceMVCConstructor}}

{{sample:Trace.TraceMVCMethod}}

## Manual Tracing

{{sample:Trace.UseTracer}}

{{sample:Trace.UseTracerRunIn}}

## Trace Outgoing HTTP Requests (recommended)

The [recommended way of creating HttpClient](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-2.1) in ASP.NET Core 2.0 and upwards is to use the
`System.Net.Http.IHttpClientFactory` defined in the Microsoft.Extensions.Http package.
The following example demonstrates how to register and use an HttpClient using Google Trace so that it traces
outgoing requests.

### Configuration

{{sample:Trace.ConfigureHttpClient}}

### Usage

{{sample:Trace.TraceOutgoingClientFactory}}

## Trace Outgoing HTTP Requests (alternative)

Alternatively, if you need to construct `HttpClient` objects manually,
`TraceHeaderPropagatingHandler` can be used to propagate trace
headers:

{{sample:Trace.TraceOutgoing}}

## Using Tracing with other than Google's own trace header

The `Google.Cloud.Diagnostics` packages have been coupled to
[Google's own trace header](https://cloud.google.com/trace/docs/setup#force-trace) from their
initial release up to, and including, version 4.2.0. Starting on version 4.3.0-beta01 and upwards
it is possible to consume and emit trace context information in a format other than Google's own
trace header.

The default behaviour of the libraries is still to consume and emit trace context information using
Google's trace header.

### Custom trace context for incoming HTTP requests

If the HTTP requests that your application handles contain trace context information in a custom format
you need to use dependency injection to register:

- A `Google.Cloud.Diagnostics.Common.ITraceContext`, which will probably be scoped, and that you can create
from information obtained from any other services available via dependency injection, including 
`Microsoft.AspNetCore.Http.IHttpContextAccessor`. The trace context will be obtained per request and used
as the parent context for all trace spans, either implicit or explicit, initiated from within the code handling
the request.
- A `System.Action<Micorosft.AspNetCore.Http.HttpResponse, Google.Cloud.Diagnostics.Common.ITraceContext>`
that will be used to set trace context information on the HTTP response to each request. This will be called
before returning a response with the updated (as modified by the request handling code) trace context information.

### Custom trace context for outgoing HTTP requests

If your application itself performs HTTP requests to other services and you want to propagate trace context
information in a format other than Google's trace header, you can use dependency injection to register a
`System.Action<System.Net.Http.HttpRequestMessage, Google.Cloud.Diagnostics.Common.ITraceContext>` that will be
used to set trace context information on outgoing HTTP requests. A few things to notice:

- The format in which you propagate trace context information to external requests doesn't have to be the same as
the format in which trace context information is received by your application. You might even be accepting Google's
trace header, but the service you are calling is not.
- The trace context information propagated to outgoing requests will be the information available at the time the 
request is made, which may or may not be the same as the information you received. For instance, your code may have
created several trace spans before making the outgoing request, in which case the span ID that will be propagated
is the one of the innermost span that remains open at the moment of sending the outgoing request.

### Custom trace context: example

The following is for demonstration purposes only. We assume that trace context information contains a trace ID only
that is propagated in a `custom_trace_id` header. This is of no use in the real world.

{{sample:Trace.CustomTraceContext}}

## Using Google Trace in applications not based in ASP.NET Core

If you want to use Google Cloud Trace in applications not based on ASP.NET Core you may use the
`Google.Cloud.Diagnostics.Common` package directly and .NET's dependency injection mechanism.
You can read more about .NET dependency injection in non ASP.NET Core apps in the
[Microsoft documentation](https://docs.microsoft.com/en-us/dotnet/core/extensions/dependency-injection-usage).

Note that this is useful for both installed applications and services that process incoming messages
other than HTTP requests, such as a service reacting to Pub/Sub messages.

Following you'll see a very simplified example of how you could set up Google Cloud Trace for these
types of applications.

- Configure Google Cloud Trace. You can set tracing options the same as you would do for ASP.NET Core apps.

{{sample:StandaloneTrace.Configure}}

- Build and start a `Microsoft.Extensions.Hosting.IHost`.

{{sample:StandaloneTrace.Start}}

- Create a tracing context when appropiate, for instance, when you receive a Pub/Sub message. You can create
an empty tracing context (with all null values) if there's none. The tracer will create a tracing context
depending on configuration options like QPS, etc.

{{sample:StandaloneTrace.IncomingContext}}

- Trace normally in your code

{{sample:StandaloneTrace.Trace}}

