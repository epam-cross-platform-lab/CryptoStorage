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
using Foundation;
using Security;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc cref="KeyProviderBase" />
    /// <summary>
    /// Key provider implementation for iOS platform
    /// </summary>
    public class KeyProviderImplementation : KeyProviderBase
    {
        private const string KeyChainServiceId = "KeyChainForCryptoStorageService";
        private const string EncryptionKeyAlias = "ENCRYPTION_KEY_ALIAS";
        private const int KeyLength = 32;

        /// <inheritdoc />
        /// <summary>
        /// Generates new encryption key.
        /// </summary>
        /// <returns>Byte array that represents the encryption key.</returns>
        public override byte[] GenerateKey()
        {
            var key = GetByteValue(EncryptionKeyAlias);
            if (key != null)
                return key;

            key = new byte[KeyLength];
            var generator = new Random(DateTime.Now.Millisecond);
            generator.NextBytes(key);

            AddValue(EncryptionKeyAlias, key);

            return key;
        }

        private static void AddValue(string key, byte[] value)
        {
            var record = CreateSecRecord(key);
            record.ValueData = NSData.FromArray(value);

            var result = SecKeyChain.Add(record);

            if (result != SecStatusCode.Success)
                throw new InvalidOperationException($"Error adding record: {result}");
        }

        [CanBeNull]
        private static byte[] GetByteValue(string key)
        {
            var record = CreateSecRecord(key);
            var match = SecKeyChain.QueryAsRecord(record, out var resultCode);

            return resultCode == SecStatusCode.Success ? match.ValueData?.ToArray() : null;
        }

        [NotNull]
        private static SecRecord CreateSecRecord(string key)
        {
            return new SecRecord(SecKind.GenericPassword)
            {
                Account = key,
                Service = KeyChainServiceId,
                Label = key
            };
        }
    }
}
