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

namespace Epam.X.CryptoStorage
{
    /// <inheritdoc />
    /// <summary>
    /// CryptoStorage interface
    /// </summary>
    public interface ICryptoStorage : IDisposable
    {
        /// <summary>
        /// Writes all the data from <paramref name="inputStream"/> into CryptoStorage.
        /// </summary>
        /// <returns>The write.</returns>
        /// <param name="key">Unique key.</param>
        /// <param name="inputStream">Input stream from which data will be taken.</param>
        /// <exception cref="InvalidOperationException">If key already exists in CryptoStorage.</exception>
        void Write(string key, Stream inputStream);

        /// <summary>
        /// Reads data from CryptoStorage and puts it into the <paramref name="outputStream"/>
        /// </summary>
        /// <returns>The read.</returns>
        /// <param name="key">Unique key.</param>
        /// <param name="outputStream">Output stream in which data will be written.</param>
        /// <exception cref="InvalidOperationException">If key is not found in CryptoStorage.</exception>
        void Read(string key, Stream outputStream);

        /// <summary>
        /// Checks if specified <paramref name="key" /> exists in CryptoStorage
        /// </summary>
        /// <returns>TRUE - if key exists, otherwise - FALSE.</returns>
        /// <param name="key">Unique key.</param>
        bool Contains(string key);

        /// <summary>
        /// Deletes the specified <paramref name="key"/> and coresponding value from CryptoStorage.
        /// </summary>
        /// <param name="key">Unique key.</param>
        void Delete(string key);

        /// <summary>
        /// Deletes all key-value pairs from CryptoStorage.
        /// </summary>
        void Clean();
    }
}
