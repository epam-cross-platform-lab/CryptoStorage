# Epam.X.CryptoStorage Plugin

This component provides encryption and decryption of sensitive data. All data is stored in a folder, that must be specified 
on creation step. In order to store new scope of data you need to specify "Key" for each data scope. The reason is that package works as Dictionary.
Your "Key" value will be used as FileName and your sensitive data will be used as File content. 

## Cloning Instruction
```
git clone https://github.com/epam-xamarin-lab/CryptoStorage.git
```

## Current Status

This package is released and considered as stable.

## Authors

* Aliaksei Safonau (EPAM Systems), https://github.com/Aleksey7151
* Yauhen Papou (EPAM Systems), https://github.com/ypapou

## Dependencies

No external dependencies.

## Requirements
* Android 4.3+
* iOS 9.3+

## Quick Start
This package is constructed in a Cross way, so in any project (platform-independent or any platform-specific) you can just write:

```C#
// ==================================================== //
//                      IMPORTANT                       //
// IN ORDER TO KEEP ENCRYPTION KEY IN SECRET YOU MUST   //
// CALL "DISPOSE" FUNCTION AFTER USING CRYPTOSTORAGE.   //
// ==================================================== //

// ================== CREATION
// here we specify storage directory. Here we use default CryptoProvider (AES with 128 bit key), and default KeyProvider (KeyChain of Keystore)
using(ICryptoStorage storage = CryptoStorageFactory.Create("path to storage directory"))
{
	// adds string to storage
	storage.Add("key_1", "some_string_value");

	// adds byte array to storage
	storage.Add("key_2", new byte[1024*1024]);
} 

// also we can specify our custom CryptoProvider (MyCryptoProvider)
using(ICryptoStorage storage = CryptoStorageFactory.Create("path to storage directory", new MyCryptoProvider()))
{
	// gets string from storage
	string stringValue = storage.GetString("key_1");

	// gets string from storage, if key doesn't exist returns default value
	string stringValue = storage.GetString("key_1", "default value");

	// gets byte array from storage
	byte[] bytes = storage.GetBytes("key_2");
}

// also we can specify our custom KeyProvider (MyKeyProvider)
using(ICryptoStorage storage = CryptoStorageFactory.Create("path to storage directory", new MyKeyProvider()))
{
	bool exists = storage.Contains("key_2"); 
}

// also we can also specify both custom CryptoProvider and KeyProvider.
using(ICryptoStorage storage = CryptoStorageFactory.Create("path to storage directory",  new MyCryptoProvider(), new MyKeyProvider()))
{
	// Removes key-value pair from CryptoStorage.
	storage.Delete("key_1");

	// Removes all key-value pairs from CryptoStorage.
	storage.Clear();
}



```

## Solution Structure
* Platform independent code: .NET Standart 2.0 library.
* Android specific code: Android class library.
* iOS specific code: iOS class library.

## Installation
You need to add the **Epam.X.CryptoStorage** package to your iOS / Android projects and cross-platform projects

## Cryptographic algorithms for data protection

|             |     iOS / Android     |
|:-----------:|:---------------------:|
|  Algorithm  | AES                   |
|  Key length | 128 bit 		      |
|   Mode      | CBC                   |
|  Padding    | PCKS7                 | 



## Encryption key Protection mechanisms

|  iOS  |  Android (<6.0) | Android (>=6.0) | 
|:-----:|:---------------:|:---------------:|
| Encryption key (for AES) is stored in iOS KeyChain| Data encryption key (for AES) is stored in System.IO.IsolatedStorage in encrypted form. RSA 2048 is used for key protection. RSA key-pair is generated in Android Keystore. In that case Private key can not be obtained. And only this library is able to decrypt data encryption key. This is controlled by the Android OS. | Data encryption key (for AES) is formed using HMacSHA256 and constant data, that is defined in library. For each device unique HMac key is generated in Android Keystore. That means, that each device forms unique data encryption key. HMac key can not be obtained. And only this library can use HMac key in HMacSHA256 algorithm. |


## Package dependencies
None.

## Perfomance
v1 = "AAAA...(50000 times)...AAAA"
v2 = "BBBB...(50000 times)...BBBB"


|     Device \ User Story        | GetString(k1)|GetString(k1)->GetString(k2)  |  Add(k1, v2)  |  Add(k1, v1) --> Add(k2, v2)  | Contains(k1) | Delete(k1) |
|--------------------------------|--------------|------------------------------|---------------|-------------------------------|--------------|------------|
|  Samsung Galaxy S6             | 123 msec     | 123 msec -> 3.2 msec         |   123 msec    |   123 msec   -->  3.5 msec    |   15 msec    |  7 msec    |
|  iPhone 6S                     | 21 msec      |  21 msec -> 1 msec           |   24 msec     |     24 msec  -->  1.5 msec    |    8 msec    |  5 msec    |

