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
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc />
    /// <summary>
    /// Crypto storage implementation for iOS platform
    /// </summary>
    public class CryptoStorageImplementation : CryptoStorageImplementationBase
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase" /> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        public CryptoStorageImplementation([NotNull] string storageDirectory)
            : base(storageDirectory, new KeyProviderImplementation())
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase"/> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        /// <param name="cryptoProvider">Crypto provider implementation that provides custom encryption algorithms.</param>
        public CryptoStorageImplementation(
            [NotNull] string storageDirectory,
            [NotNull] ICryptoProvider cryptoProvider) 
            : base(storageDirectory, new KeyProviderImplementation(), cryptoProvider)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase"/> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        /// <param name="keyProvider">Key provider implementation that provides custom encryption keys.</param>
        public CryptoStorageImplementation(
            [NotNull] string storageDirectory,
            [NotNull] IKeyProvider keyProvider)
            : base(storageDirectory, keyProvider)
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Epam.CryptoStorage.Abstractions.CryptoStorageImplementationBase"/> class.
        /// </summary>
        /// <param name="storageDirectory">Path to a directory where files are stored.</param>
        /// <param name="keyProvider">Key provider implementation that provides custom encryption keys.</param>
        /// <param name="cryptoProvider">Crypto provider implementation that provides custom encryption algorithms.</param>
        public CryptoStorageImplementation(
            [NotNull] string storageDirectory,
            [NotNull] IKeyProvider keyProvider,
            [NotNull] ICryptoProvider cryptoProvider)
            : base(storageDirectory, keyProvider, cryptoProvider)
        {
        }
    }
}
