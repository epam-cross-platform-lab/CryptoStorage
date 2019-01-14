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
using System.IO.IsolatedStorage;
using Android.App;
using Android.Security;
using Java.Math;
using Java.Security;
using Java.Util;
using Javax.Crypto;
using Javax.Security.Auth.X500;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    internal sealed class KeyProviderBeforeMarshMallow : KeyProviderBase
    {
        private const string SymmetricKeyFileName = "CryptoStorage.key";
        private const string KeystoreId = "AndroidKeyStore";
        private const string EncryptionKeyAlias = "RSA_KEY_PAIR_ALIAS";
        private const string RsaAlgorithmDefinition = "RSA/ECB/PKCS1Padding";
        private const int KeyLength = 32;
        private const int Rsa2048BlockSize = 256;

        [CanBeNull] private readonly KeyStore.PrivateKeyEntry _keyEntry;

        public KeyProviderBeforeMarshMallow()
        {
            var ks = KeyStore.GetInstance(KeystoreId).NotNull();
            ks.Load(null);
            _keyEntry = (KeyStore.PrivateKeyEntry)ks.GetEntry(EncryptionKeyAlias, null);
        }

        public override byte[] GenerateKey()
        {
            byte[] key;

            // ReSharper disable once AssignNullToNotNullAttribute
            var isolatedStorage = IsolatedStorageFile.GetStore(IsolatedStorageScope.User | IsolatedStorageScope.Assembly, null, null);

            if (!isolatedStorage.FileExists(SymmetricKeyFileName))
            {
                key = GenerateKey(KeyLength);

                var publicKey = _keyEntry == null ? GenerateRsaKeys().Public : _keyEntry.Certificate?.PublicKey;
                var encrypted = Transform(key, publicKey.NotNull(), CipherMode.EncryptMode);

                using (var isolatedStream = new IsolatedStorageFileStream(SymmetricKeyFileName, FileMode.OpenOrCreate, isolatedStorage))
                {
                    using (var writer = new BinaryWriter(isolatedStream))
                    {
                        writer.Write(encrypted);
                    }
                }
            }
            else
            {
                if (_keyEntry == null)
                    throw new InvalidOperationException("Can't find RSA keys for decrypting Symmetrick encryption key.");

                var encrypted = new byte[Rsa2048BlockSize];
                using (var isolatedStream = new IsolatedStorageFileStream(SymmetricKeyFileName, FileMode.Open, isolatedStorage))
                {
                    using (var reader = new BinaryReader(isolatedStream))
                    {
                        reader.Read(encrypted, 0, Rsa2048BlockSize);
                    }
                }
                key = Transform(encrypted, _keyEntry.PrivateKey.NotNull(), CipherMode.DecryptMode);
            }

            return key;
        }

        [NotNull]
        private static KeyPair GenerateRsaKeys()
        {
#pragma warning disable 618
            var specification = new KeyPairGeneratorSpec
                .Builder(Application.Context)
                .SetSubject(new X500Principal("CN=EPAM.CryptoStorage, O=Android Authority"))
                .SetAlias(EncryptionKeyAlias)
                .SetSerialNumber(BigInteger.One)
                .SetStartDate(new Date(DateTime.Now.Ticks))
                .SetEndDate(new Date(DateTime.Now.AddYears(30).Ticks))
                .Build();
#pragma warning restore 618
            var generator = KeyPairGenerator.GetInstance("RSA", KeystoreId);
            generator.NotNull().Initialize(specification);

            return generator.GenerateKeyPair().NotNull();
        }

        [NotNull]
        private static byte[] GenerateKey(int keyLength)
        {
            var generator = new System.Random(DateTime.Now.Millisecond);
            var buff = new byte[keyLength];
            generator.NextBytes(buff);

            return buff;
        }

        [NotNull]
        private static byte[] Transform([NotNull] byte[] data, [NotNull] IKey rsaKey, CipherMode mode)
        {
            try
            {
                var cipher = Cipher.GetInstance(RsaAlgorithmDefinition).NotNull();
                cipher.Init(mode, rsaKey);

                return cipher.DoFinal(data);
            }
            catch (NoSuchAlgorithmException ex)
            {
                throw new InvalidOperationException("Android runtime doesn't have support for 'RSA' cipher (mandatory algorithm for runtime)", ex);
            }
            catch (NoSuchProviderException ex)
            {
                throw new InvalidOperationException("Android runtime doesn't have support for 'AndroidOpenSSL' provider (mandatory for runtime)", ex);
            }
            catch (NoSuchPaddingException ex)
            {
                throw new InvalidOperationException("Android runtime doesn't have support for 'PKCS1' padding", ex);
            }
        }
    }
}
