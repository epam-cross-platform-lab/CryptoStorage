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
using JetBrains.Annotations;
using System;

namespace Epam.X.CryptoStorage
{
    internal class StorageProvider : IStorageProvider
    {
        private const string FileExtension = "cst";
        private const string IvFileExtension = "iv";
        [NotNull] private readonly string _directory;

        public StorageProvider(string directory)
        {
            if (!Directory.Exists(directory.NotNullOrWhiteSpace()))
                throw new DirectoryNotFoundException($"Directory {directory} doesn't exist");

            _directory = directory;
        }

        public void WriteIv(string fileName, byte[] iv)
        {
            using(var stream = File.Open(GetIvFilePath(fileName), FileMode.OpenOrCreate))
            {
                stream.Write(iv, 0, iv.Length);
            }
        }

        public byte[] ReadIv(string fileName)
        {
            if (!File.Exists(GetIvFilePath(fileName)))
                throw new InvalidOperationException("File with initialization vector doesn't exist in CryptoStorage.");

            using (var stream = File.Open(GetIvFilePath(fileName), FileMode.Open))
            {
                var buff = new byte[stream.Length];

                stream.Read(buff, 0, buff.Length);

                return buff;
            }
        }

        public Stream GetWritingStream(string fileName)
        {
            return File.Open(GetFilePath(fileName), FileMode.OpenOrCreate).NotNull();
        }

        public Stream GetReadingStream(string fileName)
        {
            return File.Open(GetFilePath(fileName), FileMode.Open).NotNull();
        }

        public bool Contains(string fileName)
        {
            return File.Exists(GetFilePath(fileName)) &&
                   File.Exists(GetIvFilePath(fileName));
        }

        public void Delete(string fileName)
        {
            File.Delete(GetFilePath(fileName));
            File.Delete(GetIvFilePath(fileName));
        }

        public void Clean()
        {
            var files = Directory.EnumerateFiles(_directory, $"*.{FileExtension}").NotNull();
            foreach (var file in files)
            {
                File.Delete(file);
            }

            var ivs = Directory.EnumerateFiles(_directory, $"*.{IvFileExtension}").NotNull();
            foreach (var iv in ivs)
            {
                File.Delete(iv);
            }
        }
                
        private string GetFilePath(string key)
        {
            return Path.Combine(_directory, $"{key}.{FileExtension}");
        }

        private string GetIvFilePath(string key)
        {
            return Path.Combine(_directory, $"{key}.{IvFileExtension}");
        }
    }
}
