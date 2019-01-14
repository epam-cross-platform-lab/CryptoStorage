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
using Android.OS;
using JetBrains.Annotations;

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc cref="KeyProviderBase" />
    /// <summary>
    /// Key provider for Android platform
    /// </summary>
    public sealed class KeyProviderImplementation : KeyProviderBase
    {
        [NotNull] private readonly KeyProviderBase _keyProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:Epam.CryptoStorage.KeyProviderImplementation"/> class.
        /// </summary>
        public KeyProviderImplementation()
        {
            if (Build.VERSION.SdkInt < BuildVersionCodes.M)
            {
                _keyProvider = new KeyProviderBeforeMarshMallow();
            }
            else
            {
                _keyProvider = new KeyProviderAfterMarshMallow();
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Generates new encryption key.
        /// </summary>
        /// <returns>Byte array that represents the encryption key.</returns>
        public override byte[] GenerateKey()
        {
            return _keyProvider.GenerateKey();
        }
    }
}
