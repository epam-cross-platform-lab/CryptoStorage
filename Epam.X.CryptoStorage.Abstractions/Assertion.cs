// =========================================================================
// Copyright 2019 EPAM Systems, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// =========================================================================
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    internal static class Assertion
    {
        [AssertionMethod]
        public static T AssertNotNull<T>(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] T value,
            [NotNull] string message,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0) where T : class
        {
            if (value != null) return value;

            throw new AssertionException($"Assertion '{message}' failed at {memberName}, {filePath}:{lineNumber}");
        }

        [AssertionMethod]
        public static T NotNull<T>(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] this T value,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0) where T : class
        {
            if (value != null) return value;

            throw new AssertionException($"Value of type '{typeof(T).FullName}' is null at {memberName}, {filePath}:{lineNumber}");
        }

        [AssertionMethod]
        public static string NotNullOrWhiteSpace(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] this string value,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (!string.IsNullOrWhiteSpace(value)) return value;

            throw new AssertionException($"Value of type '{typeof(string).FullName}' is null or empty at {memberName}, {filePath}:{lineNumber}");
        }

        [AssertionMethod]
        public static T[] NotNullOrEmpty<T>(
            [AssertionCondition(AssertionConditionType.IS_NOT_NULL)] this T[] value,
            [CallerMemberName] string memberName = null,
            [CallerFilePath] string filePath = null,
            [CallerLineNumber] int lineNumber = 0)
        {
            if (value != null && value.Any()) return value;

            throw new AssertionException($"Value of type '{typeof(T[]).FullName}' is null or empty at {memberName}, {filePath}:{lineNumber}");
        }
    }
}
