# Version history

# Version 2.3.0, released 2020-11-19

- [Commit 532b6ae](https://github.com/googleapis/google-cloud-dotnet/commit/532b6ae):
  - feat: Update BigtableTableAdmin GetIamPolicy to include the additional binding for Backup.
  - feat: Change DeleteAppProfileRequest.ignore_warnings to REQUIRED.

# Version 2.2.0, released 2020-10-08

- [Commit 0790924](https://github.com/googleapis/google-cloud-dotnet/commit/0790924): fix: Add gRPC compatibility constructors
- [Commit 0ca05f5](https://github.com/googleapis/google-cloud-dotnet/commit/0ca05f5): chore: Regenerate all APIs using protoc 3.13 and Grpc.Tools 2.31
- [Commit 6bde7a3](https://github.com/googleapis/google-cloud-dotnet/commit/6bde7a3): docs: Regenerate all APIs with service comments in client documentation
- [Commit f83bdf1](https://github.com/googleapis/google-cloud-dotnet/commit/f83bdf1): fix: Regenerate all APIs with generator changes for timeouts
- [Commit 947a573](https://github.com/googleapis/google-cloud-dotnet/commit/947a573): docs: Regenerate all clients with more explicit documentation

# Version 2.1.0, released 2020-05-05

- [Commit 19d9a5e](https://github.com/googleapis/google-cloud-dotnet/commit/19d9a5e): fix: Add missing method_signature annotations for BigTable Admin Backup RPCs. The added method_signatures reflect method flattenings in the GAPIC v1 config.

# Version 2.0.0, released 2020-04-08

- [Commit 3d28ab3](https://github.com/googleapis/google-cloud-dotnet/commit/3d28ab3): Adds table backup functionality

# Version 2.0.0-beta02, released 2020-03-17

No API surface changes compared with 2.0.0-beta01, just dependency
and implementation changes.

# Version 2.0.0-beta01, released 2020-02-17

This is the first prerelease targeting GAX v3. Please see the [breaking changes
guide](https://googleapis.github.io/google-cloud-dotnet/docs/guides/breaking-gax2.html)
for details of changes to both GAX and code generation.

Note that snapshot-based RPCs are now exposed in the admin client,
despite being documented as alpha APIs. These were previously
disabled by configuration, but that configuration is not present
in the new generator. If these methods are ever removed, we'll
create a new major version.

# Version 1.1.0, released 2019-12-10

- [Commit 6ee4904](https://github.com/googleapis/google-cloud-dotnet/commit/6ee4904): Adds IAM support for table admin
- [Commit 9c69ad4](https://github.com/googleapis/google-cloud-dotnet/commit/9c69ad4): Add obsolete methods for backwards compatibility.
- [Commit 50658e2](https://github.com/googleapis/google-cloud-dotnet/commit/50658e2): Add format methods for resource names
- [Commit ee5c7dc](https://github.com/googleapis/google-cloud-dotnet/commit/ee5c7dc): Add client builders for simpler configuration

# Version 1.0.0, released 2019-05-23

Initial GA release.
