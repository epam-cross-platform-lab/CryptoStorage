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
using System.Threading;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    /// <summary>
    /// Provides functions for CryptoStorage creation.
    /// </summary>
    public class CrossCryptoStorageFactory
    {
        [NotNull] private static readonly Lazy<CrossCryptoStorageFactory> Implementation =
            new Lazy<CrossCryptoStorageFactory>(CreateCryptoStorageFactory, LazyThreadSafetyMode.PublicationOnly);

        private CrossCryptoStorageFactory()
        {
        }

        /// <summary>
        /// Current instance of <see cref="CrossCryptoStorageFactory"/>
        /// </summary>
        public static CrossCryptoStorageFactory Current
        {
            get
            {
                var instance = Implementation.Value;

                if (instance == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }

                return instance;
            }
        }

        /// <summary>
        /// Creates new instance of <see cref="ICryptoStorage"/>.
        /// </summary>
        /// <param name="storageDirectory">The directory path where files with encrypted data are stored.</param>
        /// <returns>New instance of <see cref="ICryptoStorage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageDirectory"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="storageDirectory"/> is a zero-length string or contains only white space,
        /// or contains one or more invalid characters.
        /// You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/> method.
        /// </exception>
        /// <remarks>
        /// <see cref="ICryptoStorage"/> puts encrypted data to files and saves them in <paramref name="storageDirectory"/> folder.
        /// <para/>AES encryption algorithm in CBC mode with 128 bits key length is used for data encryption by default. Encryption key is unique per device.
        /// <para/>iOS KeyChain or Android Keystore are used for encryption key protection.
        /// </remarks>
        public ICryptoStorage Create(string storageDirectory)
        {
            if (storageDirectory == null)
                throw new ArgumentNullException(nameof(storageDirectory));
            if (string.IsNullOrWhiteSpace(storageDirectory))
                throw new ArgumentException("Storage directory is a zero-length string or contains only white space.", nameof(storageDirectory));
            if (storageDirectory.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("Storage directory contains one or more invalid characters.", nameof(storageDirectory));

#if NETSTANDARD2_0
            return null;
#else
            return new CryptoStorageImplementation(storageDirectory);
#endif
        }

        /// <summary>
        /// Creates new instance of <see cref="ICryptoStorage"/>.
        /// </summary>
        /// <param name="storageDirectory">The directory path where files with encrypted data are stored.</param>
        /// <param name="keyProvider">Key provider that provides custom encryption keys.</param>
        /// <returns>New instance of <see cref="ICryptoStorage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageDirectory"/> or <paramref name="keyProvider"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="storageDirectory"/> is a zero-length string or contains only white space,
        /// or contains one or more invalid characters.
        /// You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/> method.
        /// </exception>
        /// <remarks>
        /// <see cref="ICryptoStorage"/> puts encrypted data to files and saves them in <paramref name="storageDirectory"/> folder.
        /// <para/>AES encryption algorithm in CBC mode with custom key is used for data encryption by default.
        /// <para/>iOS KeyChain or Android Keystore are used for encryption key protection.
        /// </remarks>
        public ICryptoStorage Create(string storageDirectory, IKeyProvider keyProvider)
        {
            if (storageDirectory == null)
                throw new ArgumentNullException(nameof(storageDirectory));
            if (string.IsNullOrWhiteSpace(storageDirectory))
                throw new ArgumentException("Storage directory is a zero-length string or contains only white space.", nameof(storageDirectory));
            if (storageDirectory.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("Storage directory contains one or more invalid characters.", nameof(storageDirectory));
            if (keyProvider == null)
                throw new ArgumentNullException(nameof(keyProvider));

#if NETSTANDARD2_0
            return null;
#else
            return new CryptoStorageImplementation(storageDirectory, keyProvider);
#endif
        }

        /// <summary>
        /// Creates new instance of <see cref="ICryptoStorage"/>.
        /// </summary>
        /// <param name="storageDirectory">The directory path where files with encrypted data are stored.</param>
        /// <param name="cryptoProvider">The cryptographic provider that provides custom encryption/decryption algorithm.</param>
        /// <returns>New instance of <see cref="ICryptoStorage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageDirectory"/> or <paramref name="cryptoProvider"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="storageDirectory"/> is a zero-length string or contains only white space,
        /// or contains one or more invalid characters.
        /// You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/> method.
        /// </exception>
        /// <remarks>
        /// <see cref="ICryptoStorage"/> puts encrypted data to files and saves them in <paramref name="storageDirectory"/> folder.
        /// <para/>Custom encryption algorithm with 128 bits key is used for data encryption by default. Encryption key is unique per device.
        /// <para/>iOS KeyChain or Android Keystore are used for encryption key protection.
        /// </remarks>
        public ICryptoStorage Create(string storageDirectory, ICryptoProvider cryptoProvider)
        {
            if (storageDirectory == null)
                throw new ArgumentNullException(nameof(storageDirectory));
            if (string.IsNullOrWhiteSpace(storageDirectory))
                throw new ArgumentException("Storage directory is a zero-length string or contains only white space.", nameof(storageDirectory));
            if (storageDirectory.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("Storage directory contains one or more invalid characters.", nameof(storageDirectory));
            if (cryptoProvider == null)
                throw new ArgumentNullException(nameof(cryptoProvider));

#if NETSTANDARD2_0
            return null;
#else
            return new CryptoStorageImplementation(storageDirectory, cryptoProvider);
#endif
        }

        /// <summary>
        /// Creates new instance of <see cref="ICryptoStorage"/>.
        /// </summary>
        /// <param name="storageDirectory">The directory path where files with encrypted data are stored.</param>
        /// <param name="keyProvider">Key provider that provides custom encryption keys.</param>
        /// <param name="cryptoProvider">The cryptographic provider that provides custom encryption/decryption algorithm.</param>
        /// <returns>New instance of <see cref="ICryptoStorage"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="storageDirectory"/> or <paramref name="keyProvider"/> or <paramref name="cryptoProvider"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="storageDirectory"/> is a zero-length string or contains only white space,
        /// or contains one or more invalid characters.
        /// You can query for invalid characters by using the <see cref="Path.GetInvalidPathChars"/> method.
        /// </exception>
        /// <remarks>
        /// <see cref="ICryptoStorage"/> puts encrypted data to files and saves them in <paramref name="storageDirectory"/> folder.
        /// <para/>Custom encryption algorithm with custom key is used for data encryption by default.
        /// <para/>iOS KeyChain or Android Keystore are used for encryption key protection.
        /// </remarks>
        public ICryptoStorage Create(string storageDirectory, IKeyProvider keyProvider, ICryptoProvider cryptoProvider)
        {
            if (storageDirectory == null)
                throw new ArgumentNullException(nameof(storageDirectory));
            if (string.IsNullOrWhiteSpace(storageDirectory))
                throw new ArgumentException("Storage directory is a zero-length string or contains only white space.", nameof(storageDirectory));
            if (storageDirectory.IndexOfAny(Path.GetInvalidPathChars()) != -1)
                throw new ArgumentException("Storage directory contains one or more invalid characters.", nameof(storageDirectory));
            if (keyProvider == null)
                throw new ArgumentNullException(nameof(keyProvider));
            if (cryptoProvider == null)
                throw new ArgumentNullException(nameof(cryptoProvider));

#if NETSTANDARD2_0
            return null;
#else
            return new CryptoStorageImplementation(storageDirectory, keyProvider, cryptoProvider);
#endif
        }

        private static CrossCryptoStorageFactory CreateCryptoStorageFactory()
        {
#if NETSTANDARD2_0
            return null;
#else
            return new CrossCryptoStorageFactory();
#endif
        }

        [NotNull]
        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException(
                "This functionality is not implemented in the portable version of this assembly. " +
                "You should reference the Epam.X.CryptoStorage NuGet package from your main application project in order to reference the platform-specific implementation.");
        }
    }
}
