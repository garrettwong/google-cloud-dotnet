# Frequently Asked Questions

## How can I use non-default credentials for gRPC-based APIs?

The libraries implement a *client builder* pattern to make
life considerably simpler when you wish to specify different
credentials or a different API endpoint.

The general pattern is to create a builder (using the same name as
the client type, with a suffix of `Builder`), populate any
propreties to override some aspect of the behavior, and then call
`Build`. For example:

```csharp
SpeechClient client = new SpeechClientBuilder
{
    // Populate properties here
}.Build();
```

In terms of credentials, there are four (mutually-exclusive)
properties you can use to specify credentials:

- `ChannelCredentials`: the gRPC credentials to use
- `CredentialsPath`: the path to a JSON file containing service account credentials
- `JsonCredentials`: a string (in JSON format) containing service account credentials
- `TokenAccessMethod`: a delegate used to provide access tokens

The final property is a delegate to avoid exposing a dependency on
`Google.Apis.Auth` in the API surface, but the intention of it is to
allow you to use any `ICredential` (e.g. a `GoogleCredential` or a
`UserCredential`) via a method group conversion. For example, to
create a `SpeechClient` with a `UserCredential`, you would write
code like this:

```csharp
UserCredential credential = ...;
SpeechClient client = new SpeechClientBuilder
{
    TokenAccessMethod = credential.GetAccessTokenForRequestAsync
}.Build();
```

## How can I trace gRPC issues?

For libraries that use gRPC, it can be very useful to hook into the
gRPC logging framework. There are two aspects to this:

- Setting environment variables
- Directing logs

The environment variables affecting gRPC are [listed in the gRPC
repository](https://github.com/grpc/grpc/blob/master/doc/environment_variables.md).
The important ones for diagnostics are `GRPC_TRACE` and
`GRPC_VERBOSITY`. For example, you might want to start off with
`GRPC_TRACE=all` and `GRPC_VERBOSITY=DEBUG` which will dump a *lot*
of information, then tweak them to reduce this to only useful
data... or start with one kind of tracing (e.g.
`GRPC_TRACE=call_error`) and add more as required.

By default, the gRPC logs will not be displayed anywhere. The
simplest way of seeing gRPC logs in many cases will be to send them
to the console:

```csharp
using Grpc.Core;
using Grpc.Core.Logging;
...
// Call this before you do any gRPC work
GrpcEnvironment.SetLogger(new ConsoleLogger());
```

Other `ILogger` implementations are available, or you can implement
it yourself to integrate with other systems - see the
[Grpc.Core.Logging](https://github.com/grpc/grpc/tree/master/src/csharp/Grpc.Core/Logging)
namespace for details.

## How can I trace requests and responses in REST-based APIs?

For libraries that use HTTP1.1 and REST, it can be useful to perfom request and response
logging. There are two aspects to this:

- Registering a global logger
- Configuring the events to log in a specific service

The underlying service is available via the `Service` property in each `XyzClient` class. Within
that service, you need to configure the `HttpClient`'s message handler. As a complete example,
here's a call to the Translation API, listing all the available languages, and logging the request
headers and the response body:

[!code-cs[](obj/snippets/root.Faq.txt#RestLogging)]

To log *all* events from the message handler, you can set the `LogEvents` property to
`~LogEventType.None`.

## How can I use emulators?

Some APIs (such as Datastore and PubSub) provide emulators in the
[Cloud SDK](https://cloud.google.com/sdk/). Client libraries in some
other languages automatically use emulators if specific environment
variables are set, but the Google Cloud Libraries for .NET
deliberately do not do this, to avoid accidentally using an emulator
when production was expected or vice versa.

Where emulators are directly supported by the libraries, the client
builder type has an `EmulatorDetection` property which can be set to
one of the following values:

- `None` (the default): Ignores the presence or absence of emulator configuration.
- `ProductionOnly`: Always connects to the production servers, but
   throws an exception if an emulator configuration is detected that would suggest connecting to
   an emulator is expected.
- `EmulatorOnly`: Always connect to the emulator, throwing an exception if no emulator
   configuration is detected.
- `EmulatorOrProduction`: Connect to the emulator if an emulator configuration is detected,
  or production otherwise. This is a convenient option, but risks damage to
  production databases or running up unexpected bills if tests are accidentally
  run in production due to the emulator configuration being absent unexpectedly.
  (Using separate projects for production and testing is a best practice for
  preventing the first issue, but may be unrealistic for small or hobby projects.)

Here emulator configuration presence is usually interpreted as
"appropriate environment variables being set", but it is possible
that in the future there will be other conventions for
configuring emulators.

If you need to connect to an emulator directly (for example because
it is not yet supported in the library for the API you're using),
simply use the appropriate client builder, set the endpoint to the
host and port the emulator is listening on, and set the credentials to
to `ChannelCredentials.Insecure`.

Example for PubSub:

[!code-cs[](obj/snippets/root.Faq.txt#Emulator)]

## Why aren't the gRPC native libraries being found?

The native libraries that gRPC relies on are present in
[Grpc.Core](https://www.nuget.org/packages/Grpc.Core/),
and the NuGet package has targets to copy them to appropriate output
directories. However, due to the way NuGet dependencies are
generated with .NET Core, you may find that with transitive
dependencies, the targets aren't executed.

We've set up our client libraries (e.g. `Google.Cloud.Datastore.V1`)
so that if you directly depend on any of them, everything should
work - but if your application only has transitive dependencies, you
could run into errors like this:

```text
Unhandled Exception: System.IO.FileNotFoundException:
  Error loading native library. Not found in any of the possible locations: [...]
   at Grpc.Core.Internal.UnmanagedLibrary.FirstValidLibraryPath(String[] libraryPathAlternatives)
   at Grpc.Core.Internal.UnmanagedLibrary..ctor(String[] libraryPathAlternatives)
   at Grpc.Core.Internal.NativeExtension.Load()
   at Grpc.Core.Internal.NativeExtension..ctor()
   at Grpc.Core.Internal.NativeExtension.Get()
   at Grpc.Core.GrpcEnvironment.GrpcNativeInit()
   at Grpc.Core.GrpcEnvironment..ctor()
   ...
```

In that case, the simplest fix is to add a direct dependency to
`Grpc.Core` from your application, which will ensure that the
native libraries are copied appropriately.

## How can I modify repeated fields and maps in protobuf messages?

The generated C# code for protobuf messages makes simple properties
read/write, but repeated fields and map fields are read-only. That
doesn't stop you from populating them, though: it just means you
can't change the property to refer to a *different* list or map.

Typically you'll populate this using a *collection initializer*
nested within an *object initializer*. As an example, let's look at
how we might create a `BatchAnnotateImagesRequest` message in the
Vision API. (This is just an easy-to-understand example; the
Google.Cloud.Vision.V1 package provides helper methods to avoid you
having to create batches yourself in most cases.)

The protobuf description looks like this:

```proto
// Multiple image annotation requests are batched into a single service call.
message BatchAnnotateImagesRequest {
  // Individual image annotation requests for this batch.
  repeated AnnotateImageRequest requests = 1;
}
```

In the generated C# code, the `Requests` property of
`BatchAnnotateImagesRequest` is read-only, but you can populate it
with a collection initializer:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoRepeatedField1)]

You don't *have* to use a collection initializer though, and
sometimes it would be inconvenient to do so. It's perfectly valid to
add to the repeated field after other initialization:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoRepeatedField2)]

Finally, it's worth being aware that `RepeatedField<T>` has an `Add`
overload accepting an `IEnumerable<T>`. This allows you to use a
collection initializer to copy items out of another collection, or a
LINQ query result:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoRepeatedField3)]

Likewise for map fields (which are significantly less common) you
can use collection initializers, or (from C# 6 onwards) the indexer
syntax within an object initializer. As an example of this, let's
consider the Scheduler V1 API, which contains a message like this:

```proto
message HttpTarget {
  // Other fields omitted

  // The user can specify HTTP request headers to send with the job's
  // HTTP request. (Further documentation omitted here.)
  map<string, string> headers = 3;
}
```

Again, the `Headers` property in the generated message is read-only,
but you can populate it with a collection initializer:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoMap1)]

Or an indexer in an object initializer:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoMap2)]

Or modify it after other initialization steps:

[!code-cs[](obj/snippets/root.Faq.txt#ProtoMap3)]

## Why is System.EntryPointNotFoundException being thrown?

While there are various *potential* causes for this, the most likely
cause is that you have a dependency on Grpc.Core 2.x, but you're
still depending on Google Cloud libraries that depend on Grpc.Core
1.x.

We have now released client libraries (some of them still in beta,
but most GA) which use GAX 3.x, which depends on Grpc.Core 2.x. If
you update to the latest version of the client library, that should
fix the issue.
