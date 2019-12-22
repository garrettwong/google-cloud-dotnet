﻿// Copyright 2019 Google LLC
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

using Google.Cloud.Tools.VersionCompat.Utils;
using Mono.Cecil;
using System.Collections.Generic;
using System.Linq;

namespace Google.Cloud.Tools.VersionCompat.CecilUtils
{
    /// <summary>
    /// MethodDefinition comparer that compares methods in the same way that the C#
    /// compiler considers methods to be the same. I.e. if the C# compiler allows two
    /// methods to be defined in the same class, then this comparer will consider them
    /// separate methods; conversly if the C# compiler considers them the same, so it's
    /// an error to define them both in the same class, then this comparer will consider
    /// the two methods the same.
    /// E.g. `void A(long)` and `void A(int)` are different;
    /// but `void A(ref int)` and `void A(out int) are the same.
    /// </summary>
    internal class SameMethodComparer : IEqualityComparer<MethodDefinition>
    {
        public static SameMethodComparer Instance { get; } = new SameMethodComparer();
        private SameMethodComparer() { }
        public bool Equals(MethodDefinition x, MethodDefinition y)
        {
            if (x is null && y is null)
            {
                return true;
            }
            if (x is null || y is null)
            {
                return false;
            }
            if (x.Name != y.Name)
            {
                return false;
            }
            if (x.GenericParameters.Count != y.GenericParameters.Count)
            {
                return false;
            }
            if (x.Parameters.Count != y.Parameters.Count)
            {
                return false;
            }
            foreach (var (xp, yp) in x.Parameters.Zip(y.Parameters))
            {
                if (!SameTypeComparer.Instance.Equals(xp.ParameterType, yp.ParameterType))
                {
                    return false;
                }
            }
            // Don't compare name, default values, in/out/ref, etc... as these do not change the method signature.
            return true;
        }
        public int GetHashCode(MethodDefinition obj) => obj?.Name.GetHashCode() ?? 0;
    }
}
