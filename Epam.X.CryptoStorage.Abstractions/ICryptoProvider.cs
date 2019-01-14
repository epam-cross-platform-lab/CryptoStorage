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
namespace Epam.X.CryptoStorage
{
    /// <summary>
    /// Interface definition of CryptoProvider.
    /// </summary>
    public interface ICryptoProvider
    {
        /// <summary>
        /// Gets the length of the Initialization vector
        /// </summary>
        /// <returns>The initialization vector length.</returns>
        int GetIvBytesLength();

        /// <summary>
        /// Returns key length of encryption algorithm
        /// </summary>
        /// <returns>The key lenght.</returns>
        int GetKeyBytesLenght();

        /// <summary>
        /// Creates the encryptor.
        /// </summary>
        /// <returns>The encryptor.</returns>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Initialization vector.</param>
        ICryptoTransform GetEncryptor(byte[] key, byte[] iv);

        /// <summary>
        /// Creates the decryptor.
        /// </summary>
        /// <returns>The decryptor.</returns>
        /// <param name="key">Encryption key.</param>
        /// <param name="iv">Initialization vector.</param>
        ICryptoTransform GetDecryptor(byte[] key, byte[] iv);
    }
}
