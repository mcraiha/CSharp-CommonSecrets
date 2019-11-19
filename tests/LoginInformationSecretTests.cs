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
		public void SetLoginInformationTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, "does not matter", ska, derivedKey);

			string newTitle = "new and fresh!";

			// Act
			string loginInformationTitle1 = loginInformationSecret.GetTitle(derivedKey);
			loginInformationSecret.SetTitle(newTitle, derivedKey);
			string loginInformationTitle2 = loginInformationSecret.GetTitle(derivedKey);

			// Assert
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
			loginInformationSecret.SetURL(newURL, derivedKey);
			string loginInformationURL2 = loginInformationSecret.GetURL(derivedKey);

			// Assert
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
			loginInformationSecret.SetEmail(newEmail, derivedKey);
			string loginInformationEmail2 = loginInformationSecret.GetEmail(derivedKey);

			// Assert
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
			loginInformationSecret.SetUsername(newUsername, derivedKey);
			string loginInformationUsername2 = loginInformationSecret.GetUsername(derivedKey);

			// Assert
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
			loginInformationSecret.SetPassword(newPassword, derivedKey);
			string loginInformationPassword2 = loginInformationSecret.GetPassword(derivedKey);

			// Assert
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
			loginInformationSecret.SetNotes(newNotes, derivedKey);
			string loginInformationNotes2 = loginInformationSecret.GetNotes(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationNotes2));
			Assert.AreEqual(newNotes, loginInformationNotes2);
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
			loginInformationSecret.SetCreationTime(newCreationTime, derivedKey);
			DateTimeOffset loginInformationCreationTime2 = loginInformationSecret.GetCreationTime(derivedKey);

			// Assert
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
			loginInformationSecret.SetModificationTime(newModificationTime, derivedKey);
			DateTimeOffset loginInformationModificationTime2 = loginInformationSecret.GetModificationTime(derivedKey);

			// Assert
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
			loginInformationSecret.SetIcon(newIcon, derivedKey);
			byte[] loginInformationIcon2 = loginInformationSecret.GetIcon(derivedKey);

			// Assert
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
			loginInformationSecret.SetCategory(newCategory, derivedKey);
			string loginInformationCategory2 = loginInformationSecret.GetCategory(derivedKey);

			// Assert
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
			loginInformationSecret.SetTags(newTags, derivedKey);
			string loginInformationTags2 = loginInformationSecret.GetTags(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTags2));
			Assert.AreEqual(newTags, loginInformationTags2);
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