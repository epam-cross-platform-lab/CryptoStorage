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
using System.IO;
namespace Epam.X.CryptoStorage
{
    internal interface IStorageProvider
    {
        Stream GetWritingStream(string fileName);

        Stream GetReadingStream(string fileName);

        void WriteIv(string fileName, byte[] iv);

        byte[] ReadIv(string fileName);

        bool Contains(string fileName);

        void Delete(string fileName);

        void Clean();
    }
}
