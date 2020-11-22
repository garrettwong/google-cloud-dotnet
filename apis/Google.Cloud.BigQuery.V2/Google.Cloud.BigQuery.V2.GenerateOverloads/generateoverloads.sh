#!/bin/bash

declare -r CLIENT=../Google.Cloud.BigQuery.V2/BigQueryClient

dotnet run -- Methods/DatasetCrud.xml $CLIENT.DatasetCrud.cs
dotnet run -- Methods/DatasetLabels.xml $CLIENT.DatasetLabels.cs
dotnet run -- Methods/InsertData.xml $CLIENT.InsertData.cs
dotnet run -- Methods/JobCrud.xml $CLIENT.JobCrud.cs
dotnet run -- Methods/ModelCrud.xml $CLIENT.ModelCrud.cs
dotnet run -- Methods/Queries.xml $CLIENT.Queries.cs
dotnet run -- Methods/RoutineCrud.xml $CLIENT.RoutineCrud.cs
dotnet run -- Methods/ServiceAccount.xml $CLIENT.ServiceAccount.cs
dotnet run -- Methods/TableCrud.xml $CLIENT.TableCrud.cs
