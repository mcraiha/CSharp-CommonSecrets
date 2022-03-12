#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class FileEntrySecretAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task ConstructorAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ FileEntry.filenameKey, "filename.txt" }
			};

			FileEntrySecret fes = await FileEntrySecret.CreateFileEntryAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(fes);
			Assert.IsNotNull(fes.audalfData);
		}
/*
		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 112, 120, 103, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0x13, 0xaa, 0xf5, 0x36, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nic2e.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0, 23, 34, 33, 22, 222, 111 };

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			FileEntrySecret fesCopy = new FileEntrySecret(fes);
			string filenameInCopy = fesCopy.GetFilename(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(filenameInCopy));
			Assert.AreEqual(filename, filenameInCopy);
			Assert.AreNotSame(fes.audalfData, fesCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(fes.keyIdentifier, fesCopy.keyIdentifier);
			Assert.AreNotSame(fes.keyIdentifier, fesCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(fes.checksum, fesCopy.checksum);
		}

		[Test]
		public void GetFileEntryTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "ni12ce.pdf";
			byte[] fileContent = new byte[] { 1, 2, 32, 11, 2, byte.MaxValue, 0, 0, 2, 34, 45, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			FileEntry feCopy = fes.GetFileEntry(derivedKey);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreFileEntriesEqual(fe, feCopy));
			Assert.AreEqual(fe.creationTime, feCopy.creationTime);
			Assert.AreEqual(fe.modificationTime, feCopy.modificationTime);
		}

		[Test]
		public void GetFilenameTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string rtFilename = fes.GetFilename(derivedKey);

			// Assert
			Assert.AreEqual(filename, rtFilename);
		}

		[Test]
		public void GetFileContentTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			byte[] rtFileContent = fes.GetFileContent(derivedKey);

			// Assert
			CollectionAssert.AreEqual(fileContent, rtFileContent);
		}

		[Test]
		public void GetFileContentLengthInBytes()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x40, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0x38, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice232fwf.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0, 4, 5, 6, 7, 8, 9, 33, 44, 55, 66, 77};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			long fileContentLength = fes.GetFileContentLengthInBytes(derivedKey);

			// Assert
			Assert.AreEqual(fileContent.LongLength, fileContentLength);
		}

		[Test]
		public void GetCreationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			DateTimeOffset fileEntryCreationTime = fes.GetCreationTime(derivedKey);

			// Assert
			Assert.AreEqual(fe.GetCreationTime(), fileEntryCreationTime);
		}

		[Test]
		public void GetModificationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			DateTimeOffset fileEntryModificationTime1 = fes.GetModificationTime(derivedKey);
			Thread.Sleep(1100);
			fes.SetFilename("much_nicer.pdf", derivedKey);
			DateTimeOffset fileEntryModificationTime2 = fes.GetModificationTime(derivedKey);

			// Assert
			Assert.Greater(fileEntryModificationTime2, fileEntryModificationTime1);
		}

		[Test]
		public void GetKeyIdentifierTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 25, 138, 78, 83, 111, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0x13, 0xb5, 0x58, 0x59, 0x13, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			// Act
			FileEntrySecret fes = new FileEntrySecret(fe, keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, fes.GetKeyIdentifier());
		}

		[Test]
		public void CanBeDecryptedWithDerivedPassword()
		{
			byte[] derivedKey1 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 100, 222, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			// Act
			FileEntrySecret fes = new FileEntrySecret(fe, keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(fes.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(fes.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(fes.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(fes.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetFilenameTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 98, 10, 11, 12, 131, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			string filename = "backup.zip";
			byte[] fileContent = Encoding.UTF8.GetBytes("peace, happiness, freedom and something...");

			FileEntry fileEntry = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fileEntry, "does not matter", ska, derivedKey);

			string filename1 = "another_backup.zip";

			// Act
			bool shouldBeTrue = fes.SetFilename(filename1, derivedKey);
			string filename2 = fes.GetFilename(derivedKey);
			bool shouldBeFalse = fes.SetFilename(filename1,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(filename2));
			Assert.AreEqual(filename1, filename2);
		}

		[Test]
		public void SetFileContentTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 98, 10, 11, 12, 131, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			string filename = "backup.zip";
			byte[] fileContent = Encoding.UTF8.GetBytes("peace, happiness, freedom and something...");

			FileEntry fileEntry = new FileEntry(filename, fileContent);

			FileEntrySecret fes = new FileEntrySecret(fileEntry, "does not matter", ska, derivedKey);

			byte[] fileContent1 = Encoding.UTF8.GetBytes("this is a wrong backup for this situation");

			// Act
			bool shouldBeTrue = fes.SetFileContent(fileContent1, derivedKey);
			byte[] fileContent2 = fes.GetFileContent(derivedKey);
			bool shouldBeFalse = fes.SetFileContent(fileContent1,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			CollectionAssert.AreEqual(fileContent1, fileContent2);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string filename = "nice.pdf";
			byte[] fileContent = new byte[] { 1, 2, 3, 1, 2, byte.MaxValue, 0, 0, 0, 0, 0, 0};

			FileEntry fe = new FileEntry(filename, fileContent);

			FileEntrySecret fes1 = new FileEntrySecret(fe, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string checksum1 = fes1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(fes1, Formatting.Indented);

			FileEntrySecret fes2 = JsonConvert.DeserializeObject<FileEntrySecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, fes2.GetChecksumAsHex());
		}

		*/
	}
}

#endif // ASYNC_WITH_CUSTOM