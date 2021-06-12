# Version history

# Version 1.1.0, released 2021-04-29

- [Commit f4e1ad2](https://github.com/googleapis/google-cloud-dotnet/commit/f4e1ad2): feat: Add fields for Pub/Sub triggers
- [Commit f3ab04e](https://github.com/googleapis/google-cloud-dotnet/commit/f3ab04e):
  - fix: Specify `build` as the body of a `CreateBuild` call. The Cloud Build API has always assumed this, but now we are actually specifying it.
  - feat: Add `ReceiveTriggerWebhook` for webhooks activating specific triggers.
  - docs: Update field docs on required-ness behavior and fix typos.
  - docs: Add `$PROJECT_NUMBER` as a substitution variable.
  - feat: Add `SecretManager`-related resources and messages for corresponding integration.
  - docs: Clarify lifetime/expiration behavior around `ListBuilds` page tokens.
  - feat: Add `COMMENTS_ENABLED_FOR_EXTERNAL_CONTRIBUTORS_ONLY` for corresponding comment control behavior with triggered builds.
  - feat: Add `E2_HIGHCPU_8` and `E2_HIGHCPU_32` machine types.

# Version 1.0.0, released 2021-03-02

No API surface changes since 1.0.0-beta01.

# Version 1.0.0-beta01, released 2021-01-06

Initial release.
