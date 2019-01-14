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
using System.Security.Cryptography;

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc />
    /// <summary>
    /// This class provides base implementation of ICryptoStorage interface.
    /// </summary>
    public abstract class CryptoStorageImplementationBase : ICryptoStorage
    {
        [NotNull] private readonly IStorageProvider _storageProvider;
        [NotNull] private readonly ICryptoProvider _cryptoProvider;
        [NotNull] private readonly IKeyProvider _keyProvider;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase" /> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        /// <param name="keyProvider">Key provider implementation that provides custom encryption keys.</param>
        protected CryptoStorageImplementationBase(
            [NotNull] string storageDirectory,
            [NotNull] IKeyProvider keyProvider) 
            : this(storageDirectory, 
                   keyProvider, 
                   new AesCryptoProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase"/> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        /// <param name="keyProvider">Key provider implementation that provides custom encryption keys.</param>
        /// <param name="cryptoProvider">Crypto provider implementation that provides custom encryption algorithms.</param>
        protected CryptoStorageImplementationBase(
            [NotNull] string storageDirectory,
            [NotNull] IKeyProvider keyProvider,
            [NotNull] ICryptoProvider cryptoProvider)
        {
            if (string.IsNullOrEmpty(storageDirectory))
                throw new ArgumentNullException(nameof(storageDirectory));

            if (keyProvider == null)
                throw new ArgumentNullException(nameof(keyProvider));

            if (cryptoProvider == null)
                throw new ArgumentNullException(nameof(cryptoProvider));

            _storageProvider = new StorageProvider(storageDirectory);
            _cryptoProvider = cryptoProvider;
            _keyProvider = keyProvider;
        }


        /// <summary>
        /// Writes all the data from <paramref name="inputStream"/> into CryptoStorage.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="key">Unique key.</param>
        /// <param name="inputStream">Input stream from which data will be taken.</param>
        /// <exception cref="InvalidOperationException">If key already exists in CryptoStorage.</exception>
        public void Write(string key, Stream inputStream)
        {
            if (Contains(key))
                throw new InvalidOperationException($"Key \"{key}\" already exists in CryptoStorage.");

            var iv = GenerateIv();
            _storageProvider.WriteIv(key, iv);

            using(var cryptoStream = new CryptoStream(
                inputStream, 
                _cryptoProvider.GetEncryptor(GetEncryptionKey(), iv), 
                CryptoStreamMode.Read))
            {
                using(var outputStream = _storageProvider.GetWritingStream(key))
                {
                    cryptoStream.CopyTo(outputStream);
                }
            }
        }

        /// <summary>
        /// Reads data from CryptoStorage and puts it into the <paramref name="outputStream"/>
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="key">Unique key.</param>
        /// <param name="outputStream">Output stream in which data will be written.</param>
        /// <exception cref="InvalidOperationException">If key is not found in CryptoStorage.</exception>
        public void Read(string key, Stream outputStream)
        {
            if (!Contains(key))
                throw new InvalidOperationException($"Key \"{key}\" doesn't exist in CryptoStorage.");

            var iv = _storageProvider.ReadIv(key);

            using (var cryptoStream = new CryptoStream(
                _storageProvider.GetReadingStream(key), 
                _cryptoProvider.GetDecryptor(GetEncryptionKey(), iv),
                CryptoStreamMode.Read))
            {
                cryptoStream.CopyTo(outputStream);
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Checks if specified <paramref name="key" /> exists in CryptoStorage
        /// </summary>
        /// <returns>TRUE - if key exists, otherwise - FALSE.</returns>
        /// <param name="key">Unique key.</param>
        public bool Contains([NotNull] string key)
        {
            return _storageProvider.Contains(key.NotNullOrWhiteSpace());
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes the specified <paramref name="key"/> and coresponding value from CryptoStorage.
        /// </summary>
        /// <param name="key">Unique key.</param>
        public void Delete([NotNull] string key)
        {
            _storageProvider.Delete(key.NotNullOrWhiteSpace());
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes all key-value pairs from CryptoStorage.
        /// </summary>
        public void Clean()
        {
            _storageProvider.Clean();
        }

        /// <inheritdoc />
        /// <summary>
        /// Releases all resource used by the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase" /> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose the specified disposing.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _keyProvider.Dispose();
            }
        }

        [NotNull]
        private byte[] GenerateIv()
        {
            var iv = new byte[_cryptoProvider.GetIvBytesLength()];
            new Random(DateTime.Now.Millisecond).NextBytes(iv);
            return iv;
        }

        [NotNull]
        private byte[] GetEncryptionKey()
        {
            var encryptionKey = _keyProvider.GetKey().NotNull();

            if (encryptionKey.Length < _cryptoProvider.GetKeyBytesLenght())
                throw new InvalidOperationException($"CryptoProvider needs key size = {_cryptoProvider.GetKeyBytesLenght()} bytes.");

            return encryptionKey;
        }
    }
}
