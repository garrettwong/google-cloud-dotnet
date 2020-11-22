# Version history

# Version 2.0.0-beta03, released 2020-11-18

- [Commit 0ca05f5](https://github.com/googleapis/google-cloud-dotnet/commit/0ca05f5): chore: Regenerate all APIs using protoc 3.13 and Grpc.Tools 2.31
- [Commit 6bde7a3](https://github.com/googleapis/google-cloud-dotnet/commit/6bde7a3): docs: Regenerate all APIs with service comments in client documentation
- [Commit f83bdf1](https://github.com/googleapis/google-cloud-dotnet/commit/f83bdf1): fix: Apply timeouts to RPCs without retry
- [Commit 947a573](https://github.com/googleapis/google-cloud-dotnet/commit/947a573): docs: Regenerate all clients with more explicit documentation
- [Commit 7cf6cd8](https://github.com/googleapis/google-cloud-dotnet/commit/7cf6cd8): feat: Add speech adaptation, phrase sets and custom classes.

# Version 2.0.0-beta02, released 2020-03-19

No API surface changes compared with 2.0.0-beta01, just dependency
and implementation changes.

# Version 2.0.0-beta01, released 2020-02-18

This is the first prerelease targeting GAX v3. Please see the [breaking changes
guide](https://googleapis.github.io/google-cloud-dotnet/docs/guides/breaking-gax2.html)
for details of changes to both GAX and code generation.

# Version 1.0.0-beta05, released 2019-12-16

- [Commit 3ac2779](https://github.com/googleapis/google-cloud-dotnet/commit/3ac2779): Regenerate without retry for streaming calls. Fixes [issue 3902](https://github.com/googleapis/google-cloud-dotnet/issues/3902).

# Version 1.0.0-beta04, released 2019-12-09

- Some retry settings have been removed
- Added Speaker Diariazation
- Added RecognitionMetadata.ObfuscatedId

# Version 1.0.0-beta03, released 2019-06-10

- Added AudioEncoding.Mp3
- Added client builders for simpler configuration
- Added SpeechContext.Boost
- Added StreamingRecognitionResult.ResultEndTime

# Version 1.0.0-beta02, released 2018-08-02

- Added various language codes

# Version 1.0.0-beta01, released 2018-07-17

First beta release
