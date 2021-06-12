#!/bin/bash

set -e

# Undo the changes made in pregeneration.sh
git -C $GOOGLEAPIS_DISCOVERY checkout google/cloud/compute/v1

# Fix up Grpc.Core parts
# We have a hand-written piece at the moment for the adapter.
sed -i 's/using gaxgrpccore = Google.Api.Gax.Grpc.GrpcCore;//g' Google.Cloud.Compute.V1/*.g.cs
sed -i 's/=> gaxgrpccore::GrpcCoreAdapter.Instance/=> ComputeRestAdapter.ComputeAdapter/g' Google.Cloud.Compute.V1/*.g.cs
