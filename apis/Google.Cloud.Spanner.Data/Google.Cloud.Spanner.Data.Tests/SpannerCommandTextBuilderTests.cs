﻿// Copyright 2017 Google Inc. All Rights Reserved.
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

using System;
using Xunit;

namespace Google.Cloud.Spanner.Data.Tests
{
    public class SpannerCommandTextBuilderTests
    {
        [Theory]
        [InlineData("DROP DATABASE FOO")]
        [InlineData("DROP DATABASE FOO;")]
        [InlineData("DROP DATABASE  FOO")]
        [InlineData("DROP DATABASE      FOO")]
        [InlineData("DROP DATABASE  FOO     ;")]
        [InlineData("DROP DATABASE      FOO;;")]
        public void DropDatabasePositive(string ddlString)
        {
            SpannerCommandTextBuilder builder = SpannerCommandTextBuilder.FromCommandText(ddlString);
            Assert.True(builder.IsDropDatabaseCommand);
            Assert.Equal("FOO", builder.DatabaseToDrop);

            //via spannerconnection as well...
            using (var connection = new SpannerConnection("Data Source=projects/p/instances/i"))
            {
                var command = connection.CreateDdlCommand(ddlString);
                Assert.True(command.SpannerCommandTextBuilder.IsDropDatabaseCommand);
                Assert.Equal("FOO", command.SpannerCommandTextBuilder.DatabaseToDrop);
            }
        }

        [Fact]
        public void DropDatabase_UnrecognizedCommand()
        {
            Assert.Throws<ArgumentException>(() => SpannerCommandTextBuilder.FromCommandText("DROPDATABASE FOO;"));
        }

        [Fact]
        public void DropDatabase_TooManyValues()
        {
            // Arguably this should throw an ArgumentException on construction, but for the moment, we'll
            // stick with the v1.0 behavior.
            var builder = SpannerCommandTextBuilder.FromCommandText("DROP DATABASE FOO BAR;");
            Assert.Throws<InvalidOperationException>(() => builder.DatabaseToDrop.ToString());
        }

        [Theory]
        [InlineData("DROP DATABASEFOO")]
        [InlineData("DROP  DATABASE  FOO")]
        public void DropDatabaseUnrecognized(string ddlString)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(ddlString);
            Assert.False(builder.IsDropDatabaseCommand);
        }

        // https://cloud.google.com/spanner/docs/query-syntax
        [Theory]
        [InlineData("SELECT * FROM Albums")]
        [InlineData(" SELECT * FROM Albums")]
        [InlineData("\t\nSELECT * FROM Albums")]
        [InlineData("SELECT  * FROM Albums")]
        [InlineData("SELECT\t\n* FROM Albums")]
        [InlineData("WITH AlbumsView AS (SELECT * FROM Albums) select Title from AlbumsView")]
        [InlineData(" WITH AlbumsView AS (SELECT * FROM Albums) select Title from AlbumsView")]
        [InlineData("\t\nWITH AlbumsView AS (SELECT * FROM Albums) select Title from AlbumsView")]
        [InlineData(" WITH   AlbumsView AS (SELECT * FROM Albums) select Title from AlbumsView")]
        [InlineData("WITH\t\nAlbumsView AS (SELECT * FROM Albums) select Title from AlbumsView")]
        public void SelectCommand(string commandText)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(commandText);
            Assert.Equal(SpannerCommandType.Select, builder.SpannerCommandType);
        }

        // https://cloud.google.com/spanner/docs/dml-syntax#insert-statement
        [Theory]
        [InlineData("INSERT INTO Albums (AlbumId) VALUES (@id)")]
        [InlineData(" INSERT INTO Albums (AlbumId) VALUES (@id)")]
        [InlineData("\t\nINSERT\t\nINTO\t\nAlbums\t\n(AlbumId)t\nVALUESt\n(@id)")]
        [InlineData("INSERT  INTO  Albums  (AlbumId) VALUES (@id)")]
        [InlineData("INSERT\t\nINTO\t\nAlbums\t\n(AlbumId)t\nVALUESt\n(@id)")]
        [InlineData("INSERT Albums (AlbumId) VALUES (@id)")]
        [InlineData(" INSERT Albums (AlbumId) VALUES (@id)")]
        [InlineData("\t\nINSERT\t\nAlbums\t\n(AlbumId)t\nVALUESt\n(@id)")]
        [InlineData("INSERT  Albums  (AlbumId) VALUES (@id)")]
        [InlineData("INSERT\t\nAlbums\t\n(AlbumId)t\nVALUESt\n(@id)")]
        public void DmlInsertCommand(string commandText)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(commandText);
            Assert.Equal(SpannerCommandType.Dml, builder.SpannerCommandType);
        }

        // https://cloud.google.com/spanner/docs/dml-syntax#update-statement
        [Theory]
        [InlineData("UPDATE Albums SET Title=@title WHERE TRUE")]
        [InlineData(" UPDATE Albums SET Title=@title WHERE TRUE")]
        [InlineData("\t\nUPDATE\t\nAlbumst\nSETt\nTitle=@titlet\nWHEREt\nTRUE")]
        [InlineData("UPDATE  Albums  SET Title=@title WHERE TRUE")]
        [InlineData("UPDATE\t\nAlbums\t\nSETt\nTitle=@titlet\nWHEREt\nTRUE")]
        public void DmlUpdateCommand(string commandText)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(commandText);
            Assert.Equal(SpannerCommandType.Dml, builder.SpannerCommandType);
        }

        // https://cloud.google.com/spanner/docs/dml-syntax#delete-statement
        [Theory]
        [InlineData("DELETE FROM Albums WHERE TRUE")]
        [InlineData(" DELETE FROM Albums WHERE TRUE")]
        [InlineData("\t\nDELETE FROM Albums WHERE TRUE")]
        [InlineData("DELETE  FROM  Albums  WHERE  TRUE")]
        [InlineData("DELETE\t\nFROM\t\nAlbumst\nWHEREt\nTRUE")]
        [InlineData("DELETE Albums WHERE TRUE")]
        [InlineData(" DELETE Albums WHERE TRUE")]
        [InlineData("\t\nDELETE\t\nAlbumst\nWHEREt\nTRUE")]
        [InlineData("DELETE  Albums  WHERE TRUE")]
        [InlineData("DELETE\t\nAlbumst\nWHEREt\nTRUE")]
        public void DmlDeleteCommand(string commandText)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(commandText);
            Assert.Equal(SpannerCommandType.Dml, builder.SpannerCommandType);
        }

        // https://cloud.google.com/spanner/docs/data-definition-language
        [Theory]
        [InlineData("CREATE TABLE FOO")]
        [InlineData(" CREATE TABLE FOO")]
        [InlineData("CREATE\t\nTABLE\t\nFOO")]
        [InlineData("\t\nCREATE\t\nTABLE\t\nFOO")]
        [InlineData("DROP TABLE FOO")]
        [InlineData(" DROP TABLE FOO")]
        [InlineData("DROP\t\nTABLE\t\nFOO")]
        [InlineData("\t\nDROP\t\nTABLE\t\nFOO")]
        [InlineData("ALTER TABLE FOO")]
        [InlineData(" ALTER TABLE FOO")]
        [InlineData("ALTER\t\nTABLE\t\nFOO")]
        [InlineData("\t\nALTER\t\nTABLE\t\nFOO")]
        public void DdlCommand(string commandText)
        {
            var builder = SpannerCommandTextBuilder.FromCommandText(commandText);
            Assert.Equal(SpannerCommandType.Ddl, builder.SpannerCommandType);
        }
    }
}
