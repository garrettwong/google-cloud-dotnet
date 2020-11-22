# Version history

# Version 2.1.0, released 2020-08-03

- [Commit 330b04e](https://github.com/googleapis/google-cloud-dotnet/commit/330b04e): Fix: PubSub methods will now be retried appropriately. Fixes [issue 5225](https://github.com/googleapis/google-cloud-dotnet/issues/5225)
- [Commit 0cd128c](https://github.com/googleapis/google-cloud-dotnet/commit/0cd128c): docs: Remove experimental warning for ordering keys properties. ([issue 5219](https://github.com/googleapis/google-cloud-dotnet/issues/5219))
- [Commit 6bde7a3](https://github.com/googleapis/google-cloud-dotnet/commit/6bde7a3): docs: Regenerate all APIs with service comments in client documentation
- [Commit 6165e07](https://github.com/googleapis/google-cloud-dotnet/commit/6165e07): feat: Add support for server-side streaming pull flow control ([issue 5119](https://github.com/googleapis/google-cloud-dotnet/issues/5119))
- [Commit 2c5f3c1](https://github.com/googleapis/google-cloud-dotnet/commit/2c5f3c1): feat: Add flow control settings for StreamingPullRequest to pubsub.proto
- [Commit b5500f5](https://github.com/googleapis/google-cloud-dotnet/commit/b5500f5): docs: Add a link to Pub/Sub filtering language public documentation to pubsub.proto
- [Commit ac924f2](https://github.com/googleapis/google-cloud-dotnet/commit/ac924f2): feat: Add "detached" bool to Subscription
- [Commit f3eeca0](https://github.com/googleapis/google-cloud-dotnet/commit/f3eeca0): docs: Add comment for MessageStoragePolicy message
- [Commit 1dae64f](https://github.com/googleapis/google-cloud-dotnet/commit/1dae64f): fix: Use correct resource type for DetachSubscriptionRequest
- [Commit 5f5b8aa](https://github.com/googleapis/google-cloud-dotnet/commit/5f5b8aa): feat: DetachSubscription RPC
- [Commit 947a573](https://github.com/googleapis/google-cloud-dotnet/commit/947a573): docs: Regenerate all clients with more explicit documentation
- [Commit 777b926](https://github.com/googleapis/google-cloud-dotnet/commit/777b926): docs: Removing the experimental tag from dead letter policy related fields.
- [Commit 8cd3929](https://github.com/googleapis/google-cloud-dotnet/commit/8cd3929): docs: Removing experimental tag from DeadLetterPolicy for Cloud Pub/Sub.

# Version 2.0.0, released 2020-04-09

- [Commit 26de65c](https://github.com/googleapis/google-cloud-dotnet/commit/26de65c): Fix: Fix comment around default AckDeadline
- [Commit b872180](https://github.com/googleapis/google-cloud-dotnet/commit/b872180): docs: treat a dummy example URL as a string literal instead of a link
- [Commit ab949d1](https://github.com/googleapis/google-cloud-dotnet/commit/ab949d1): Feature: experimental Subscription.Filter property

First GA release targeting GAX 3.0.0.

# Version 2.0.0-beta02, released 2020-03-18

- [Commit 2096b6d](https://github.com/googleapis/google-cloud-dotnet/commit/2096b6d): Feature: Subscription.RetryPolicy
- [Commit e4226b7](https://github.com/googleapis/google-cloud-dotnet/commit/e4226b7):
  - Regenerate Google.Cloud.PubSub.V1 ([issue 4515](https://github.com/googleapis/google-cloud-dotnet/issues/4515))
  - PullRequest.ReturnImmediately is now obsolete
  - ListTopicSnapshots methods have new overloads accepting a topic name
  - GetSnapshot methods have new overloads accepting a snapshot name

Additionally, dependencies have been updated to target GAX 3.0.0.

# Version 2.0.0-beta01, released 2020-02-18

This is the first prerelease targeting GAX v3. Please see the [breaking changes
guide](https://googleapis.github.io/google-cloud-dotnet/docs/guides/breaking-gax2.html)
for details of changes to both GAX and code generation.

Additional significant changes in this release:

- [Commit 173b019](https://github.com/googleapis/google-cloud-dotnet/commit/173b019): Dead-letter queue support in subscriber client

# Version 1.2.0-beta01, released 2020-01-06

- [Commit d859592](https://github.com/googleapis/google-cloud-dotnet/commit/d859592): Fully enable ordering-keys ([issue 3921](https://github.com/googleapis/google-cloud-dotnet/issues/3921))
- [Commit e13ab00](https://github.com/googleapis/google-cloud-dotnet/commit/e13ab00): Update default settings; add maximum total lease extension ([issue 3920](https://github.com/googleapis/google-cloud-dotnet/issues/3920))

# Version 1.1.0, released 2019-12-10

Note that support is present in the code for ordering keys, but it's currently disabled; it will be enabled in a future release.

- [Commit 5742d91](https://github.com/googleapis/google-cloud-dotnet/commit/5742d91): Adds ReceivedMessage.DeliveryAttempt
- [Commit 1784804](https://github.com/googleapis/google-cloud-dotnet/commit/1784804): Adds dead letter policy
- [Commit 50658e2](https://github.com/googleapis/google-cloud-dotnet/commit/50658e2): Add format method for all resource name types
- [Commit c3af927](https://github.com/googleapis/google-cloud-dotnet/commit/c3af927): Fix error in handling non-ordered batch errors ([issue 3133](https://github.com/googleapis/google-cloud-dotnet/issues/3133))
- [Commit 7f24b13](https://github.com/googleapis/google-cloud-dotnet/commit/7f24b13): Minor fix for memory leak risk ([issue 3120](https://github.com/googleapis/google-cloud-dotnet/issues/3120))
- [Commit afa4a96](https://github.com/googleapis/google-cloud-dotnet/commit/afa4a96): Pubsub client ordering-keys ([issue 3099](https://github.com/googleapis/google-cloud-dotnet/issues/3099))
- [Commit ee5c7dc](https://github.com/googleapis/google-cloud-dotnet/commit/ee5c7dc): Add client builders for simplified configuration
- [Commit 0609777](https://github.com/googleapis/google-cloud-dotnet/commit/0609777): Dispose correctly, and properly ignore cancellation ([issue 3083](https://github.com/googleapis/google-cloud-dotnet/issues/3083))
- [Commit 397623e](https://github.com/googleapis/google-cloud-dotnet/commit/397623e): Adds KMS support
- [Commit 1424e89](https://github.com/googleapis/google-cloud-dotnet/commit/1424e89): Adds method overloads accepting strings for methods accepting resource names
- [Commit ddc2de2](https://github.com/googleapis/google-cloud-dotnet/commit/ddc2de2): Adds ordering keys in the underlying API, but no specific support
- [Commit 5a18d38](https://github.com/googleapis/google-cloud-dotnet/commit/5a18d38): Adds OidcToken and AuthenticationMethod
- [Commit e664cad](https://github.com/googleapis/google-cloud-dotnet/commit/e664cad): Create pubsub channels with correct unique-per-channel id ([issue 2798](https://github.com/googleapis/google-cloud-dotnet/issues/2798))
- [Commit 61cd808](https://github.com/googleapis/google-cloud-dotnet/commit/61cd808): Avoid unobserved InvalidOperationException in AsyncSingleRecvQueue ([issue 2785](https://github.com/googleapis/google-cloud-dotnet/issues/2785)). Fixes [issue 2763](https://github.com/googleapis/google-cloud-dotnet/issues/2763)
- [Commit 1d0c50c](https://github.com/googleapis/google-cloud-dotnet/commit/1d0c50c): Adds ExpirationPolicy

# Version 1.0.0, released 2018-10-08

Initial GA release.
