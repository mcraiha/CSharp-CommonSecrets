using NUnit.Framework;
using CSCommonSecrets;
using System.Collections.Generic;

namespace Tests
{
	public class FileEntrySecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ FileEntry.filenameKey, "filename.txt"}
			};

			FileEntrySecret fes = new FileEntrySecret(testDictionary, skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(fes);
			Assert.IsNotNull(fes.audalfData);
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

			FileEntrySecret fes = new FileEntrySecret(fe, skaAES_CTR, derivedKey);

			// Act
			string rtFilename = System.Text.Encoding.UTF8.GetString(fes.GetFilenameUTF8Bytes(derivedKey));

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

			FileEntrySecret fes = new FileEntrySecret(fe, skaAES_CTR, derivedKey);

			// Act
			byte[] rtFileContent = fes.GetFileContent(derivedKey);

			// Assert
			CollectionAssert.AreEqual(fileContent, rtFileContent);
		}
	}
}