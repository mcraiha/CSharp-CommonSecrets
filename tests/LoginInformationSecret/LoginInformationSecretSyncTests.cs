#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class LoginInformationSecretSyncTests
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
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			LoginInformationSecret loginInformationSecretCopy = new LoginInformationSecret(loginInformationSecret);
			string loginInformationTitle = loginInformationSecretCopy.GetTitle(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle));
			Assert.AreEqual(loginInformation.title, loginInformationTitle);
			Assert.AreNotSame(loginInformationSecret.audalfData, loginInformationSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(loginInformationSecret.keyIdentifier, loginInformationSecretCopy.keyIdentifier);
			Assert.AreNotSame(loginInformationSecret.keyIdentifier, loginInformationSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(loginInformationSecret.checksum, loginInformationSecretCopy.checksum);
		}

		[Test]
		public void GetLoginInformationTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			LoginInformation loginInformationCopy = loginInformationSecret.GetLoginInformation(derivedKey);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreLoginInformationsEqual(loginInformation, loginInformationCopy));
			Assert.AreEqual(loginInformation.creationTime, loginInformationCopy.creationTime);
			Assert.AreEqual(loginInformation.modificationTime, loginInformationCopy.modificationTime);
		}

		[Test]
		public void GetWithInvalidKeyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			
			byte[] derivedKeyInvalid = Mutator.CreateMutatedByteArray(derivedKey);

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => loginInformationSecret.GetTitle(null));
			Assert.Throws<ArgumentException>(() => loginInformationSecret.GetTitle(derivedKeyInvalid));
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
			string loginInformationUrl = loginInformationSecret.GetURL(derivedKey);

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
		public void GetLoginInformatioMFATest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 231, 42, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfc, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			loginInformationModified.UpdateMFA("otpauth://totp/DRAGONFIER?secret=YOUR_DRAGON");

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformationModified, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string loginInformationMFA = loginInformationSecret.GetMFA(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationMFA));
			Assert.AreEqual(loginInformationModified.mfa, loginInformationMFA);
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
		public void GetKeyIdentifierTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x59, 0x15, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, loginInformationSecret.GetKeyIdentifier());
		}

		[Test]
		public void CanBeDecryptedWithDerivedPasswordTest()
		{
			byte[] derivedKey1 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(loginInformationSecret.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(loginInformationSecret.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(loginInformationSecret.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(loginInformationSecret.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetLoginInformationTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newTitle = "new and fresh!";

			// Act
			string loginInformationTitle1 = loginInformationSecret.GetTitle(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetTitle(newTitle, derivedKey);
			string loginInformationTitle2 = loginInformationSecret.GetTitle(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetTitle(newTitle, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle2));
			Assert.AreEqual(loginInformation.title, loginInformationTitle1);
			Assert.AreEqual(newTitle, loginInformationTitle2);
		}

		[Test]
		public void SetLoginInformationURLTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newURL = "https://newandbetter.future";

			// Act
			string loginInformationURL1 = loginInformationSecret.GetURL(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetURL(newURL, derivedKey);
			string loginInformationURL2 = loginInformationSecret.GetURL(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetURL(newURL, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationURL1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationURL2));
			Assert.AreEqual(loginInformation.url, loginInformationURL1);
			Assert.AreEqual(newURL, loginInformationURL2);
		}

		[Test]
		public void SetLoginInformationEmailTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newEmail = "tomorrow@newandbetter.future";

			// Act
			string loginInformationEmail1 = loginInformationSecret.GetEmail(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetEmail(newEmail, derivedKey);
			string loginInformationEmail2 = loginInformationSecret.GetEmail(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetEmail(newEmail, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail2));
			Assert.AreEqual(loginInformation.email, loginInformationEmail1);
			Assert.AreEqual(newEmail, loginInformationEmail2);
		}

		[Test]
		public void SetLoginInformationUsernameTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newUsername = "futuredragon";

			// Act
			string loginInformationUsername1 = loginInformationSecret.GetUsername(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetUsername(newUsername, derivedKey);
			string loginInformationUsername2 = loginInformationSecret.GetUsername(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetUsername(newUsername, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername2));
			Assert.AreEqual(loginInformation.username, loginInformationUsername1);
			Assert.AreEqual(newUsername, loginInformationUsername2);
		}

		[Test]
		public void SetLoginInformationPasswordTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newPassword = "ag#%23,.67Hngrewn";

			// Act
			string loginInformationPassword1 = loginInformationSecret.GetPassword(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetPassword(newPassword, derivedKey);
			string loginInformationPassword2 = loginInformationSecret.GetPassword(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetPassword(newPassword, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword2));
			Assert.AreEqual(loginInformation.password, loginInformationPassword1);
			Assert.AreEqual(newPassword, loginInformationPassword2);
		}

		[Test]
		public void SetLoginInformationNotesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newNotes = "future text that is happy and joyful for all purposes...";

			// Act
			bool shouldBeTrue = loginInformationSecret.SetNotes(newNotes, derivedKey);
			string loginInformationNotes2 = loginInformationSecret.GetNotes(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetNotes(newNotes, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationNotes2));
			Assert.AreEqual(newNotes, loginInformationNotes2);
		}

		[Test]
		public void SetLoginInformationMFATest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 15, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 196 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newMFA = "otpauth://totp/BIG_DRAGON?secret=YOUR_FIRE";

			// Act
			bool shouldBeTrue = loginInformationSecret.SetMFA(newMFA, derivedKey);
			string loginInformationMFA2 = loginInformationSecret.GetMFA(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetMFA(newMFA, new byte[] { 1, 2, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationMFA2));
			Assert.AreEqual(newMFA, loginInformationMFA2);
		}

		[Test]
		public void SetLoginInformationCreationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			DateTimeOffset newCreationTime = DateTimeOffset.UtcNow.AddDays(1); 

			// Act
			DateTimeOffset loginInformationCreationTime1 = loginInformationSecret.GetCreationTime(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetCreationTime(newCreationTime, derivedKey);
			DateTimeOffset loginInformationCreationTime2 = loginInformationSecret.GetCreationTime(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetCreationTime(newCreationTime, new byte[] { 1, 29, 3});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.AreEqual(loginInformation.creationTime, loginInformationCreationTime1.ToUnixTimeSeconds());
			Assert.AreEqual(newCreationTime.ToUnixTimeSeconds(), loginInformationCreationTime2.ToUnixTimeSeconds());
		}

		[Test]
		public void SetLoginInformationModificationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			DateTimeOffset newModificationTime = DateTimeOffset.UtcNow.AddDays(1); 

			// Act
			DateTimeOffset loginInformationModificationTime1 = loginInformationSecret.GetModificationTime(derivedKey);
			bool shouldBeTrue = loginInformationSecret.SetModificationTime(newModificationTime, derivedKey);
			DateTimeOffset loginInformationModificationTime2 = loginInformationSecret.GetModificationTime(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetModificationTime(newModificationTime, new byte[] { 1, 29, 3, 99, 134, 255, 0});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.AreEqual(loginInformation.modificationTime, loginInformationModificationTime1.ToUnixTimeSeconds());
			Assert.AreEqual(newModificationTime.ToUnixTimeSeconds(), loginInformationModificationTime2.ToUnixTimeSeconds());
		}

		[Test]
		public void SetLoginInformationIconTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			byte[] newIcon = new byte[] { 1, 2, 44, 55, 66, 33, 89, 23, 222, 111, 100, 99, 45, 127, 198, 255 };

			// Act
			bool shouldBeTrue = loginInformationSecret.SetIcon(newIcon, derivedKey);
			byte[] loginInformationIcon2 = loginInformationSecret.GetIcon(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetIcon(newIcon, new byte[] { 13, 129, 3, 99, 134, 255, 0});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			CollectionAssert.AreEqual(newIcon, loginInformationIcon2);
		}

		[Test]
		public void SetLoginInformationCategoryTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newCategory = "Finance";

			// Act
			bool shouldBeTrue = loginInformationSecret.SetCategory(newCategory, derivedKey);
			string loginInformationCategory2 = loginInformationSecret.GetCategory(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetCategory(newCategory, new byte[] { 13, 129, 0, 99, 134, 255, 0});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationCategory2));
			Assert.AreEqual(newCategory, loginInformationCategory2);
		}

		[Test]
		public void SetLoginInformationTagsTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newTags = "Finance; Home; Travel; Future";

			// Act
			bool shouldBeTrue = loginInformationSecret.SetTags(newTags, derivedKey);
			string loginInformationTags2 = loginInformationSecret.GetTags(derivedKey);
			bool shouldBeFalse = loginInformationSecret.SetTags(newTags, new byte[] { 13, 129, 0, 91, 194, 255, 0});

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTags2));
			Assert.AreEqual(newTags, loginInformationTags2);
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
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

		[Test]
		public void CheckIfChecksumMatchesContentTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformationSecret loginInformationSecret1 = new LoginInformationSecret(loginInformation, "does not matter", skaAES_CTR, derivedKey);

			// Act
			bool shouldBeTrue = loginInformationSecret1.CheckIfChecksumMatchesContent();
			loginInformationSecret1.checksum = loginInformationSecret1.checksum.Remove(0, 1);
			bool shouldBeFalse = loginInformationSecret1.CheckIfChecksumMatchesContent();

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM