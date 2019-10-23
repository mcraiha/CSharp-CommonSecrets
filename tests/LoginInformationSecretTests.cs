using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class LoginInformationSecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		private static readonly LoginInformation loginInformation = new LoginInformation("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1");

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 127, 1, 250, 4, 5, 6, 7, 13, 7, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ LoginInformation.titleKey, "Shopping site"}
			};

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(testDictionary, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(loginInformationSecret);
			Assert.IsNotNull(loginInformationSecret.audalfData);
		}

		[Test]
		public void GetLoginInformationTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationTitle = loginInformationSecret.GetTitle(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle));
			Assert.AreEqual(loginInformation.title, loginInformationTitle);
		}

		[Test]
		public void GetLoginInformationUrlTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 88, 9, 107, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 192, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationUrl = loginInformationSecret.GetUrl(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUrl));
			Assert.AreEqual(loginInformation.url, loginInformationUrl);
		}

		[Test]
		public void GetLoginInformatioEmailTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 8, 5, 6, 7, 88, 9, 107, 101, 12, 13, 104, 159, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0x22, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0x11, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationEmail = loginInformationSecret.GetEmail(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail));
			Assert.AreEqual(loginInformation.email, loginInformationEmail);
		}

		[Test]
		public void GetLoginInformationUsernameTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 66, 27, 83, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x30, 0x41, 0x5b, 0x63, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationUsername = loginInformationSecret.GetUsername(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername));
			Assert.AreEqual(loginInformation.username, loginInformationUsername);
		}

		[Test]
		public void GetLoginInformationPasswordTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 83, 9, 110, 211, 12, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationPassword = loginInformationSecret.GetPassword(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword));
			Assert.AreEqual(loginInformation.password, loginInformationPassword);
		}

		[Test]
		public void GetLoginInformationNotesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			loginInformationModified.UpdateNotes("Nice story about how I found the missing tapes of ...");

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationNotes = loginInformationSecret.GetNotes(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationNotes));
			Assert.AreEqual(loginInformationModified.notes, loginInformationNotes);
		}

		[Test]
		public void GetCreationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			DateTimeOffset loginInformationCreationTime = loginInformationSecret.GetCreationTime(derivedKey);

			// Assert
			Assert.AreEqual(loginInformation.GetCreationTime(), loginInformationCreationTime);
		}

		[Test]
		public void GetModificationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			Thread.Sleep(1100);
			loginInformationModified.UpdateNotes("Some text to here so modification time triggers");

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			DateTimeOffset loginInformationModificationTime = loginInformationSecret.GetModificationTime(derivedKey);

			// Assert
			Assert.IsTrue(loginInformationModified.modificationTime > loginInformationModified.creationTime);
			Assert.AreEqual(loginInformationModified.GetModificationTime(), loginInformationModificationTime);
		}

		[Test]
		public void GetLoginInformationIconTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 181, 229, 31, 44, 55, 61, 7, 8, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0x21, 0x3b, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Random rng = new Random(Seed: 1337);
			byte[] iconBytes = new byte[2048];
			rng.NextBytes(iconBytes);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			loginInformationModified.UpdateIcon(iconBytes);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			byte[] loginInformationIcon = loginInformationSecret.GetIcon(derivedKey);

			// Assert
			Assert.IsNotNull(loginInformationIcon);
			CollectionAssert.AreEqual(iconBytes, loginInformationIcon);
		}

		[Test]
		public void GetLoginInformationCategoryTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 91, 10, 21, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd5, 0xb5, 0x58, 0x59, 0x15, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			loginInformationModified.UpdateCategory("Shopping");

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationCategory = loginInformationSecret.GetCategory(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationCategory));
			Assert.AreEqual(loginInformationModified.category, loginInformationCategory);
		}

		[Test]
		public void GetLoginInformationTagsTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xfd, 0xaa, 0xc5, 0xd5, 0xb5, 0x58, 0x59, 0x15, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			loginInformationModified.UpdateTags("personal");

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationTags = loginInformationSecret.GetTags(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTags));
			Assert.AreEqual(loginInformationModified.tags, loginInformationTags);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 88, 9, 107, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 192, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret1 = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string checksum1 = loginInformationSecret1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(loginInformationSecret1, Formatting.Indented);

			LoginInformationSecret loginInformationSecret2 = JsonConvert.DeserializeObject<LoginInformationSecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, loginInformationSecret2.GetChecksumAsHex());
		}
	}
}