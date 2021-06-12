#!/bin/bash

set -e

# Undo the changes made in pregeneration.sh
git -C $GOOGLEAPIS checkout google/cloud/accessapproval/v1/

# Fix up the generated client to use the right gRPC types
sed -i s/AccessApprovalService.AccessApprovalServiceClient/AccessApproval.AccessApprovalClient/g Google.Cloud.AccessApproval.V1/AccessApprovalServiceClient.g.cs

# Fix up unit test classes
sed -i s/AccessApprovalService.AccessApprovalServiceClient/AccessApproval.AccessApprovalClient/g Google.Cloud.AccessApproval.V1.Tests/AccessApprovalServiceClientTest.g.cs
