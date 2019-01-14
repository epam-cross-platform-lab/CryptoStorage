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
using System;
using System.IO;
using JetBrains.Annotations;
using System.Text;
namespace Epam.X.CryptoStorage
{
    /// <summary>
    /// Crypto storage extensions.
    /// </summary>
    public static class CryptoStorageExtensions
    {
        /// <summary>
        /// Adds the bytes array into the CryptoStorage
        /// </summary>
        /// <param name="cryptoStorage">Crypto storage.</param>
        /// <param name="key">Unique key.</param>
        /// <param name="bytes">Byte arrays to be added.</param>
        /// <exception cref="InvalidOperationException">If key already exists in CryptoStorage.</exception>
        public static void AddBytes(
            this ICryptoStorage cryptoStorage, 
            [NotNull] string key, 
            [NotNull] byte[] bytes)
        {
            using (Stream stream = new MemoryStream(bytes.NotNullOrEmpty()))
            {
                cryptoStorage.Write(key.NotNullOrWhiteSpace(), stream);
            }
        }

        /// <summary>
        /// Gets the bytes array from CryptoStorage
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="cryptoStorage">Crypto storage.</param>
        /// <param name="key">Unique key.</param>
        /// <exception cref="InvalidOperationException">If key is not found in CryptoStorage.</exception>
        public static byte[] GetBytes(
            this ICryptoStorage cryptoStorage, 
            [NotNull] string key)
        {
            using(var stream = new MemoryStream())
            {
                cryptoStorage.Read(key.NotNullOrWhiteSpace(), stream);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// Adds the string value into CryptoStorage
        /// </summary>
        /// <param name="cryptoStorage">Crypto storage.</param>
        /// <param name="key">Unique key.</param>
        /// <param name="value">Value to be added.</param>
        /// <exception cref="InvalidOperationException">If key already exists in CryptoStorage.</exception>
        public static void AddString(
            this ICryptoStorage cryptoStorage, 
            [NotNull] string key, 
            [NotNull] string value)
        {
            var bytes = Encoding.UTF8.GetBytes(value.NotNullOrWhiteSpace());

            cryptoStorage.AddBytes(key.NotNullOrWhiteSpace(), bytes);
        }

        /// <summary>
        /// Gets string representation of value that corresponds to specified <paramref name="key"/>.
        /// <para/>If <paramref name="key" /> doesn't exist in CryptoStorage than <see cref="InvalidOperationException"></see> will be thrown.
        /// </summary>
        /// <returns>String representation of value that corresponds to <paramref name="key"/></returns>
        /// <param name="cryptoStorage">Crypto storage.</param>
        /// <param name="key">Unique key.</param>
        /// <exception cref="InvalidOperationException">If key is not found in CryptoStorage.</exception>
        public static string GetString(
            this ICryptoStorage cryptoStorage, 
            [NotNull] string key)
        {
            var bytes = cryptoStorage.GetBytes(key.NotNullOrWhiteSpace());

            return Encoding.UTF8.GetString(bytes);
        }
    }
}
