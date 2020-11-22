﻿// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     https://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using Google.Cloud.Tools.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Google.Cloud.Tools.ReleaseManager
{
    /// <summary>
    /// Release management tool, providing a single entry point to multiple common tasks
    /// when releasing  libraries.
    /// </summary>
    internal sealed class Program
    {
        private static int Main(string[] args)
        {
            try
            {
                var allCommands = typeof(Program).Assembly.GetTypes()
                    .Where(t => !t.IsAbstract && typeof(ICommand).IsAssignableFrom(t))
                    .Select(Activator.CreateInstance)
                    .Cast<ICommand>()
                    .OrderBy(c => c.Command)
                    .ToList();
                if (args.Length == 0)
                {
                    ShowUsage(allCommands);
                    return 0;
                }


                var commandName = args[0];
                var commandArgs = args.Skip(1).ToArray();

                var selectedCommand = allCommands.Where(c => c.Command == commandName).FirstOrDefault();
                if (selectedCommand is null)
                {
                    Console.WriteLine($"Unknown command: '{commandName}'");
                    ShowUsage(allCommands);
                    return 0;
                }
                selectedCommand.Execute(commandArgs);
                return 0;
            }
            catch (UserErrorException e)
            {
                Console.WriteLine($"Configuration error: {e.Message}");
                return 1;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Failed: {e}");
                return 1;
            }
        }

        private static void ShowUsage(IList<ICommand> commands)
        {
            Console.WriteLine($"{nameof(ReleaseManager)} commands:");
            Console.WriteLine("");
            foreach (var command in commands)
            {
                // The conditional aspect of ExpectedArguments makes this ugly to do with
                // string interpolation.
                var builder = new StringBuilder()
                    .Append(command.Command)
                    .Append(string.IsNullOrEmpty(command.ExpectedArguments) ? "" : " " + command.ExpectedArguments)
                    .Append(": ")
                    .Append(command.Description);
                Console.WriteLine(builder);
            }
        }
    }
}
