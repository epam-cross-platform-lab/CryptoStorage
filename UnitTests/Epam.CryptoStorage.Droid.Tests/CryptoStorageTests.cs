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
using System.Text;
using Epam.X.CryptoStorage;
using NUnit.Framework;
using System;
using System.IO;

#if __ANDROID_10__
namespace Epam.CryptoStorage.Droid.Tests
#elif __IOS__
namespace Epam.X.CryptoStorage.iOS.Tests.v2
#endif
{
    [TestFixture]
    public class CryptoStorageTests
    {
        private const string Key1 = "key_1";
        private const string Key2 = "key_2";
        private const string Key3 = "key_3";
        private const int BuffLen = 10 * 1024;

        private string _folder;
        private ICryptoStorage _storage;
        private string _value1;
        private string _value2;
        private string _value3;
        private byte[] _bytes1;
        private byte[] _bytes2;

        [TestFixtureSetUp]
        public void TestFixtureInit()
        {
            _folder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            _storage = CrossCryptoStorageFactory.Current.Create(_folder);

            var builder = new StringBuilder();
            _value1 = builder.Append('A', 5000).ToString();
            _value2 = builder.Append('B', 5000).ToString();
            _value3 = builder.Append('C', 5000).ToString();

            _bytes1 = new byte[BuffLen];
            for (var i = 0; i < BuffLen; i++)
                _bytes1[i] = 0xAB;

            _bytes2 = new byte[BuffLen];
            for (var i = 0; i < BuffLen; i++)
                _bytes1[i] = 0x83;
        }

        [Test]
        public void Contain_function_returns_true_if_key_exists()
        {
            _storage.Clean();

            _storage.AddString(Key1, _value1);
            var exists = _storage.Contains(Key1);
            Assert.True(exists);
        }

        [Test]
        public void Contains_function_returns_false_if_key_does_not_exist()
        {
            _storage.Clean();

            Assert.False(_storage.Contains(Key1));
            Assert.False(_storage.Contains(Key2));
            Assert.False(_storage.Contains(Key3));
        }

        [Test]
        public void Delete_function_removes_value_from_storage()
        {
            _storage.Clean();

            _storage.AddString(Key1, _value1);
            Assert.True(_storage.Contains(Key1));

            _storage.Delete(Key1);
            Assert.False(_storage.Contains(Key1));
        }

        [Test]
        public void Write_Read_To_Storage()
        {
            var buffer = new byte[1024*1024];
            for (var i = 0; i < buffer.Length; i++)
            {
                buffer[i] = 0xAA;
            }

            var inputStream = new MemoryStream(buffer);

            _storage.Clean();

            _storage.Write(Key1, inputStream);

            Assert.IsTrue(_storage.Contains(Key1));

            var outputStream = new MemoryStream();

            _storage.Read(Key1, outputStream);

            var buffer2 = outputStream.ToArray();

            Assert.IsTrue(buffer.Length == buffer2.Length);

            for (var i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(buffer[i], buffer2[i]);
            }
        }

        [Test]
        public void AddString_and_GetString_functions_check()
        {
            _storage.Clean();

            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key1));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key2));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key3));

            _storage.AddString(Key1, _value1);
            _storage.AddString(Key2, _value2);
            _storage.AddString(Key3, _value3);

            Assert.True(_storage.Contains(Key1));
            Assert.True(_storage.Contains(Key2));
            Assert.True(_storage.Contains(Key3));

            Assert.AreEqual(_value1, _storage.GetString(Key1));
            Assert.AreEqual(_value2, _storage.GetString(Key2));
            Assert.AreEqual(_value3, _storage.GetString(Key3));
        }

        [Test]
        public void AddBytes_and_GetBytes_functions_check()
        {
            _storage.Clean();

            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key1));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key2));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key3));

            _storage.AddBytes(Key1, _bytes1);
            _storage.AddBytes(Key2, _bytes2);

            Assert.True(_storage.Contains(Key1));
            Assert.True(_storage.Contains(Key2));

            var result1 = _storage.GetBytes(Key1);
            var result2 = _storage.GetBytes(Key2);

            for (var i = 0; i < BuffLen; i++)
            {
                Assert.AreEqual(result1[i], _bytes1[i]);
                Assert.AreEqual(result2[i], _bytes2[i]);
            }
        }

        [Test]
        public void GetString_function_throws_InvalidOperationException_if_key_does_not_exist()
        {
            _storage.Clean();

            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key1));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key2));
            Assert.Throws<InvalidOperationException>(() => _storage.GetString(Key3));
        }

        [Test]
        public void Add_function_throws_InvalidOperationException_if_key_already_exists()
        {
            _storage.Clean();

            Assert.False(_storage.Contains(Key1));
            Assert.False(_storage.Contains(Key2));
            Assert.False(_storage.Contains(Key3));

            _storage.AddString(Key1, _value1);
            Assert.Throws<InvalidOperationException>(() => _storage.AddString(Key1, _value3));


            _storage.AddBytes(Key2, _bytes2);
            Assert.Throws<InvalidOperationException>(() => _storage.AddBytes(Key2, _bytes2));
        }
    }
}
