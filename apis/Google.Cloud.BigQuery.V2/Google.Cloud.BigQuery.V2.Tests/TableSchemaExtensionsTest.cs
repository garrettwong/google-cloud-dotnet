﻿// Copyright 2016 Google Inc. All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using Google.Apis.Bigquery.v2.Data;
using Xunit;

namespace Google.Cloud.BigQuery.V2.Tests
{
    public class TableSchemaExtensionsTest
    {
        [Fact]
        public void IndexFieldNames()
        {
            var schema = new TableSchema
            {
                Fields = new List<TableFieldSchema>
                {
                    new TableFieldSchema { Name = "foo" },
                    new TableFieldSchema { Name = "bar" }
                }
            };
            var expected = new Dictionary<string, int> { { "foo", 0 }, { "bar", 1 } };
            Assert.Equal(expected, schema.IndexFieldNames());
        }

        [Fact]
        public void IndexFieldNames_EmptySchema()
        {
            var schema = new TableSchema();
            Assert.Empty(schema.IndexFieldNames());
        }

        [Fact]
        public void BuildSelectedFields_NullSchema()
        {
            TableSchema schema = null;
            Assert.Null(schema.BuildSelectedFields());
        }

        [Fact]
        public void BuildSelectedFields_EmptySchema()
        {
            var schema = new TableSchema();
            Assert.Equal(string.Empty, schema.BuildSelectedFields());
        }

        [Fact]
        public void BuildSelectedFields_OneField()
        {
            var schema = new TableSchema
            {
                Fields = new List<TableFieldSchema>
                {
                    new TableFieldSchema { Name = "foo" },
                }
            };

            Assert.Equal("foo", schema.BuildSelectedFields());
        }

        [Fact]
        public void BuildSelectedFields_OneLevel()
        {
            var schema = new TableSchema
            {
                Fields = new List<TableFieldSchema>
                {
                    new TableFieldSchema { Name = "foo" },
                    new TableFieldSchema { Name = "bar" },
                }
            };

            Assert.Equal("foo,bar", schema.BuildSelectedFields());
        }

        [Fact]
        public void BuildSelectedFields_Nested()
        {
            var schema = new TableSchema
            {
                Fields = new List<TableFieldSchema>
                {
                    new TableFieldSchema
                    {
                        Name = "l0_f0",
                        Fields = new List<TableFieldSchema>
                        {
                            new TableFieldSchema { Name = "l1_f0"},
                            new TableFieldSchema
                            {
                                Name = "l1_f1",
                                Fields = new List<TableFieldSchema>
                                {
                                    new TableFieldSchema { Name = "l2_f0" },
                                    new TableFieldSchema { Name = "l2_f1" }
                                }
                            }
                        }
                    },
                    new TableFieldSchema { Name = "l0_f1" },
                    new TableFieldSchema
                    {
                        Name = "l0_f2",
                        Fields = new List<TableFieldSchema>
                        {
                            new TableFieldSchema { Name = "l1_f2"},
                            new TableFieldSchema { Name = "l1_f3"}
                        }
                    },

                }
            };

            Assert.Equal("l0_f0.l1_f0,l0_f0.l1_f1.l2_f0,l0_f0.l1_f1.l2_f1,l0_f1,l0_f2.l1_f2,l0_f2.l1_f3", schema.BuildSelectedFields());
        }
    }
}
