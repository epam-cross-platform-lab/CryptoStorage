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
using Android.Security.Keystore;
using Java.Security;
using Javax.Crypto;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    internal sealed class KeyProviderAfterMarshMallow : KeyProviderBase
    {
        private const string KeystoreId = "AndroidKeyStore";
        private const string EncryptionKeyAlias = "HMACK_SHA256_KEY_ALIAS";

        private static readonly byte[] InitialVector =
        {
            0x3E, 0x07, 0xEF, 0xB0, 0xE9, 0x56, 0xBF, 0x6B, 0x70, 0x3B, 0xAC, 0x72, 0xFC, 0x7C, 0xEC, 0x37,
            0x3D, 0x47, 0xDF, 0x9D, 0x90, 0x47, 0xA7, 0xE4, 0x06, 0x17, 0x6A, 0x32, 0x25, 0x6E, 0x6D, 0x1E,
            0x31, 0x44, 0x8F, 0xE6, 0x2D, 0xDA, 0xF6, 0xCB, 0x56, 0xAD, 0x75, 0x2E, 0x34, 0x78, 0x86, 0xBE,
            0xB6, 0x11, 0x4F, 0x98, 0x9A, 0x71, 0x63, 0x4C, 0x3A, 0xF8, 0x9E, 0x09, 0xA0, 0x3C, 0x94, 0xB2,
            0x36, 0x83, 0x56, 0xA5, 0xD5, 0x8E, 0x97, 0xF2, 0xC9, 0xB1, 0x1B, 0x0D, 0xFA, 0x4E, 0x53, 0xCA,
            0x92, 0xF4, 0x86, 0xD8, 0x42, 0x50, 0x76, 0xC1, 0x51, 0x72, 0xE2, 0x49, 0x92, 0x82, 0x4C, 0x0C,
            0x04, 0x5A, 0xEC, 0x41, 0x37, 0x72, 0x37, 0x3B, 0x75, 0x45, 0x55, 0x72, 0x34, 0xFB, 0xBC, 0x26,
            0xF3, 0x48, 0x27, 0x87, 0x0E, 0x47, 0x5F, 0xC3, 0xE1, 0xF7, 0xB4, 0xA5, 0x3F, 0x22, 0x47, 0x28
        };

        [CanBeNull] private IKey _macKey;

        public KeyProviderAfterMarshMallow()
        {
            var ks = KeyStore.GetInstance(KeystoreId).NotNull();
            ks.Load(null);
            _macKey = ks.GetKey(EncryptionKeyAlias, null);
        }

        public override byte[] GenerateKey()
        {
            if (_macKey == null)
                _macKey = GenerateMacKey();

            var mac = Mac.GetInstance(KeyProperties.KeyAlgorithmHmacSha256).NotNull();
            mac.Init(_macKey);

            return mac.DoFinal(InitialVector);
        }
        
        [NotNull]
        private static IKey GenerateMacKey()
        {
            var generator = KeyGenerator.GetInstance(KeyProperties.KeyAlgorithmHmacSha256, KeystoreId).NotNull();
            var specification = new KeyGenParameterSpec.Builder(EncryptionKeyAlias, KeyStorePurpose.Sign).Build();
            generator.Init(specification);

            return generator.GenerateKey().NotNull();
        }
    }
}
