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
using System.Security.Cryptography;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    internal sealed class AesCryptoProvider : ICryptoProvider
    {
        private const int IvLength = 128;
        private const int KeyLength = 128;
        [NotNull] private readonly Aes _aes;

        public AesCryptoProvider()
        {
            _aes = CreateAes();
        }

        public int GetIvBytesLength() => IvLength/8;

        public int GetKeyBytesLenght() => KeyLength/8;


        public ICryptoTransform GetEncryptor(byte[] key, byte[] iv)
        {
            return _aes.CreateEncryptor(key, iv);
        }

        public ICryptoTransform GetDecryptor(byte[] key, byte[] iv)
        {
            return _aes.CreateDecryptor(key, iv);
        }

        [NotNull]
        private static Aes CreateAes()
        {
            var aes = Aes.Create().NotNull();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.KeySize = KeyLength;
            return aes;
        }
    }
}
