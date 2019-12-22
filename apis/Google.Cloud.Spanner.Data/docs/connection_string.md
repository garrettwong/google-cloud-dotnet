# Connection string options

Spanner connection strings support the following options:

## Data Source

The project, instance and optionally database to use. The format is
`projects/{project}/instances/{instance}/databases/{database}`, with
the `databases/{database}` part being optional.

Examples:

- Data Source=projects/my_project/instances/my_instance
- Data Source=projects/my_project/instances/my_instance/databases/my_database

## CredentialFile

A path to a JSON service account credential file to use. If this is
not specified, the application default credentials will be used.
(These are provided automatically when running on Google Cloud
Platform, or a JSON service account credential file can be specified
with the `GOOGLE_APPLICATION_CREDENTIALS` environment variable.)

Example:

- CredentialFile=/path/to/credentials.json

## EnableGetSchemaTable

When set to true (and when targeting .NET 4.5 or .NET Standard 2.0;
the method doesn't exist in .NET Standard 1.x),
[SpannerDataReader.GetSchemaTable()](obj/api/Google.Cloud.Spanner.Data.SpannerDataReader.yml#Google_Cloud_Spanner_Data_SpannerDataReader_GetSchemaTable)
will return available schema information for the query in the form
of a `DataTable`. See the method documentation for details of the columns
returned.

Example:

- EnableGetSchemaTable=true

## UseClrDefaultForNull

By default, null values returned by Spanner are handled in the
following manner:

"Top-level" fields are always converted to `DBNull.Value`. This
means that calling `reader.GetString(0)` for example will throw an
`InvalidCastException` if the value is null. This is compatible
with other ADO.NET providers.

Array and structure elements use the null value for the target type
where possible (reference types and nullable value types), or throw
an `InvalidCastException` if the target type is a non-nullable value
type. For example, if an array field has values `[1, NULL, 2]` in Spanner,
then requesting that field with `reader.GetFieldValue<int[]>("field")` will
throw an exception, whereas `reader.GetFieldValue<int?[]>("field")`
will return a nullable integer array where the second element is a
null value.

When `UseClrDefaultForNull` is set to true, null values returned by
Spanner will be converted into the default CLR value for the
requested type. This was the behavior for Google.Cloud.Spanner.Data
v1.0.

Example:

- UseClrDefaultForNull=true

## Timeout

The default timeout used for `SpannerCommand.CommandTimeout`,
`SpannerTransaction.CommitTimeout`, and other Spanner network
operations. Defaults to 60 seconds.

Example:

- Timeout=30

## AllowImmediateTimeouts

When set to False, a timeout of 0 means an indefinite timeout.
When set to True, a timeout of 0 means an immediate timeout.
Defaults to False.

Example:

- AllowImmediateTimeouts=true

## MaximumGrpcChannels

The maximum number of gRPC channels to use per connection using the
same settings. Defaults to 4. This setting rarely needs to be
modified.

Example:

- MaximumGrpcChannels=8

## MaxConcurrentStreamsLowWatermark

The maximum number of concurrent streams per gRPC channel before
using a new channel. Defaults to 20. This setting rarely needs to be
modified.

Example:

- MaxConcurrentStreamsLowWatermark=30

## Host and Port

These control the Spanner service to connect to. These are primarily
available for testing purposes.

Examples:

- Host=alternative-spanner.googleapis.com; Port=1443
