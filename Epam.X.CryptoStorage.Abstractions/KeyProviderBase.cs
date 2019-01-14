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
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc cref="IKeyProvider" />
    /// <summary>
    /// Defines members for providing encryption key.
    /// </summary>
    public abstract class KeyProviderBase : IKeyProvider
    {
        [CanBeNull] private byte[] _key;

        /// <summary>
        ///  Gets the newly generated encryption key or existing one if it was already generated.
        /// </summary>
        /// <returns>Byte array that represents encryption key.</returns>
        [NotNull]
        public byte[] GetKey()
        {
            return _key ?? (_key = GenerateKey().NotNull());
        }

        /// <summary>
        /// Generates new encryption key.
        /// </summary>
        /// <returns>Byte array that represents the encryption key.</returns>
        [NotNull]
        public abstract byte[] GenerateKey();

        /// <inheritdoc />
        /// <summary>
        /// Releases all resource used by the <see cref="T:Epam.X.CryptoStorage.KeyProvider" /> object.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose current instance of <see cref="KeyProviderBase"/> by cleaning encryption key.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> disposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                CleanupKeyMemory();
            }
        }

        private void CleanupKeyMemory()
        {
            if (_key == null) return;

            for (var i = 0; i < _key.Length; i++)
            {
                _key[i] = 0xFF;
            }

            _key = null;
        }
    }
}
