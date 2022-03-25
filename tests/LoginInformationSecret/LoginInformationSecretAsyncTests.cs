#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class LoginInformationSecretAsyncTests
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

			byte[] derivedKey = new byte[16] { 127, 1, 250, 4, 5, 6, 7, 13, 7, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ LoginInformation.titleKey, "Shopping site"}
			};

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(loginInformationSecret);
			Assert.IsNotNull(loginInformationSecret.audalfData);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			LoginInformationSecret loginInformationSecretCopy = new LoginInformationSecret(loginInformationSecret);
			string loginInformationTitle = await loginInformationSecretCopy.GetTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle));
			Assert.AreEqual(loginInformation.title, loginInformationTitle);
			Assert.AreNotSame(loginInformationSecret.audalfData, loginInformationSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(loginInformationSecret.keyIdentifier, loginInformationSecretCopy.keyIdentifier);
			Assert.AreNotSame(loginInformationSecret.keyIdentifier, loginInformationSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(loginInformationSecret.checksum, loginInformationSecretCopy.checksum);
		}

		[Test]
		public async Task GetLoginInformationAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			LoginInformation loginInformationCopy = await loginInformationSecret.GetLoginInformationAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreLoginInformationsEqual(loginInformation, loginInformationCopy));
			Assert.AreEqual(loginInformation.creationTime, loginInformationCopy.creationTime);
			Assert.AreEqual(loginInformation.modificationTime, loginInformationCopy.modificationTime);
		}

		[Test]
		public async Task GetLoginInformationTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationTitle = await loginInformationSecret.GetTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle));
			Assert.AreEqual(loginInformation.title, loginInformationTitle);
		}

		[Test]
		public async Task GetLoginInformationUrlAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 88, 9, 107, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 192, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationUrl = await loginInformationSecret.GetURLAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUrl));
			Assert.AreEqual(loginInformation.url, loginInformationUrl);
		}

		[Test]
		public async Task GetLoginInformatioEmailAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 8, 5, 6, 7, 88, 9, 107, 101, 12, 13, 104, 159, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0x22, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0x11, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationEmail = await loginInformationSecret.GetEmailAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail));
			Assert.AreEqual(loginInformation.email, loginInformationEmail);
		}

		[Test]
		public async Task GetLoginInformationUsernameAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 66, 27, 83, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x30, 0x41, 0x5b, 0x63, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationUsername = await loginInformationSecret.GetUsernameAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername));
			Assert.AreEqual(loginInformation.username, loginInformationUsername);
		}

		[Test]
		public async Task GetLoginInformationPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 83, 9, 110, 211, 12, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationPassword = await loginInformationSecret.GetPasswordAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword));
			Assert.AreEqual(loginInformation.password, loginInformationPassword);
		}

		[Test]
		public async Task GetLoginInformationNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);
			
			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await loginInformationModified.UpdateNotesAsync("Nice story about how I found the missing tapes of ...", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationNotes = await loginInformationSecret.GetNotesAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationNotes));
			Assert.AreEqual(loginInformationModified.notes, loginInformationNotes);
		}

		[Test]
		public async Task GetLoginInformatioMFAAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 231, 42, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfc, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await loginInformationModified.UpdateMFAAsync("otpauth://totp/DRAGONFIER?secret=YOUR_DRAGON", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationMFA = await loginInformationSecret.GetMFAAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationMFA));
			Assert.AreEqual(loginInformationModified.mfa, loginInformationMFA);
		}

		[Test]
		public async Task GetCreationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			DateTimeOffset loginInformationCreationTime = await loginInformationSecret.GetCreationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(loginInformation.GetCreationTime(), loginInformationCreationTime);
		}

		[Test]
		public async Task GetModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await Task.Delay(1100);
			await loginInformationModified.UpdateNotesAsync("Some text to here so modification time triggers", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			DateTimeOffset loginInformationModificationTime = await loginInformationSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(loginInformationModified.modificationTime > loginInformationModified.creationTime);
			Assert.AreEqual(loginInformationModified.GetModificationTime(), loginInformationModificationTime);
		}

		[Test]
		public async Task GetLoginInformationIconAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 181, 229, 31, 44, 55, 61, 7, 8, 9, 110, 211, 128, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0x21, 0x3b, 0xf3, 0xaa, 0xc5, 0xd6, 0xbb, 0xf8, 0x19, 0x11, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Random rng = new Random(Seed: 1337);
			byte[] iconBytes = new byte[2048];
			rng.NextBytes(iconBytes);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await loginInformationModified.UpdateIconAsync(iconBytes, securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			byte[] loginInformationIcon = await loginInformationSecret.GetIconAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsNotNull(loginInformationIcon);
			CollectionAssert.AreEqual(iconBytes, loginInformationIcon);
		}

		[Test]
		public async Task GetLoginInformationCategoryAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 68, 78, 83, 91, 10, 21, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xc5, 0xd5, 0xb5, 0x58, 0x59, 0x15, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await loginInformationModified.UpdateCategoryAsync("Shopping", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationCategory = await loginInformationSecret.GetCategoryAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationCategory));
			Assert.AreEqual(loginInformationModified.category, loginInformationCategory);
		}

		[Test]
		public async Task GetLoginInformationTagsAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xfd, 0xaa, 0xc5, 0xd5, 0xb5, 0x58, 0x59, 0x15, 0xfb, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);
			LoginInformation loginInformationModified = loginInformation.ShallowCopy();
			await loginInformationModified.UpdateTagsAsync("personal", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformationModified, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string loginInformationTags = await loginInformationSecret.GetTagsAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTags));
			Assert.AreEqual(loginInformationModified.tags, loginInformationTags);
		}

		[Test]
		public async Task GetKeyIdentifierAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x59, 0x15, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			// Act
			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(keyIdentifier, loginInformationSecret.GetKeyIdentifier());
		}

		[Test]
		public async Task CanBeDecryptedWithDerivedPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey1 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			// Act
			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, keyIdentifier, skaAES_CTR, derivedKey1, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(await loginInformationSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey1, securityAsyncFunctions));
			Assert.IsFalse(await loginInformationSecret.CanBeDecryptedWithDerivedPasswordAsync(null, securityAsyncFunctions));
			Assert.IsFalse(await loginInformationSecret.CanBeDecryptedWithDerivedPasswordAsync(new byte[] {}, securityAsyncFunctions));
			Assert.IsFalse(await loginInformationSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey2, securityAsyncFunctions));
		}

		[Test]
		public async Task SetLoginInformationTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newTitle = "new and fresh!";

			// Act
			string loginInformationTitle1 = await loginInformationSecret.GetTitleAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetTitleAsync(newTitle, derivedKey, securityAsyncFunctions);
			string loginInformationTitle2 = await loginInformationSecret.GetTitleAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetTitleAsync(newTitle, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTitle2));
			Assert.AreEqual(loginInformation.title, loginInformationTitle1);
			Assert.AreEqual(newTitle, loginInformationTitle2);
		}

		[Test]
		public async Task SetLoginInformationURLAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newURL = "https://newandbetter.future";

			// Act
			string loginInformationURL1 = await loginInformationSecret.GetURLAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetURLAsync(newURL, derivedKey, securityAsyncFunctions);
			string loginInformationURL2 = await loginInformationSecret.GetURLAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetURLAsync(newURL, new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationURL1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationURL2));
			Assert.AreEqual(loginInformation.url, loginInformationURL1);
			Assert.AreEqual(newURL, loginInformationURL2);
		}

		[Test]
		public async Task SetLoginInformationEmailAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newEmail = "tomorrow@newandbetter.future";

			// Act
			string loginInformationEmail1 = await loginInformationSecret.GetEmailAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetEmailAsync(newEmail, derivedKey, securityAsyncFunctions);
			string loginInformationEmail2 = await loginInformationSecret.GetEmailAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetEmailAsync(newEmail, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationEmail2));
			Assert.AreEqual(loginInformation.email, loginInformationEmail1);
			Assert.AreEqual(newEmail, loginInformationEmail2);
		}

		[Test]
		public async Task SetLoginInformationUsernameAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newUsername = "futuredragon";

			// Act
			string loginInformationUsername1 = await loginInformationSecret.GetUsernameAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetUsernameAsync(newUsername, derivedKey, securityAsyncFunctions);
			string loginInformationUsername2 = await loginInformationSecret.GetUsernameAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetUsernameAsync(newUsername, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationUsername2));
			Assert.AreEqual(loginInformation.username, loginInformationUsername1);
			Assert.AreEqual(newUsername, loginInformationUsername2);
		}

		[Test]
		public async Task SetLoginInformationPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newPassword = "ag#%23,.67Hngrewn";

			// Act
			string loginInformationPassword1 = await loginInformationSecret.GetPasswordAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetPasswordAsync(newPassword, derivedKey, securityAsyncFunctions);
			string loginInformationPassword2 = await loginInformationSecret.GetPasswordAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetPasswordAsync(newPassword, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword1));
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationPassword2));
			Assert.AreEqual(loginInformation.password, loginInformationPassword1);
			Assert.AreEqual(newPassword, loginInformationPassword2);
		}

		[Test]
		public async Task SetLoginInformationNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newNotes = "future text that is happy and joyful for all purposes...";

			// Act
			bool shouldBeTrue = await loginInformationSecret.SetNotesAsync(newNotes, derivedKey, securityAsyncFunctions);
			string loginInformationNotes2 = await loginInformationSecret.GetNotesAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetNotesAsync(newNotes, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationNotes2));
			Assert.AreEqual(newNotes, loginInformationNotes2);
		}

		[Test]
		public async Task SetLoginInformationMFAAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 15, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 196 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newMFA = "otpauth://totp/BIG_DRAGON?secret=YOUR_FIRE";

			// Act
			bool shouldBeTrue = await loginInformationSecret.SetMFAAsync(newMFA, derivedKey, securityAsyncFunctions);
			string loginInformationMFA2 = await loginInformationSecret.GetMFAAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetMFAAsync(newMFA, new byte[] { 1, 2, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationMFA2));
			Assert.AreEqual(newMFA, loginInformationMFA2);
		}

		[Test]
		public async Task SetLoginInformationCreationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			DateTimeOffset newCreationTime = DateTimeOffset.UtcNow.AddDays(1); 

			// Act
			DateTimeOffset loginInformationCreationTime1 = await loginInformationSecret.GetCreationTimeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetCreationTimeAsync(newCreationTime, derivedKey, securityAsyncFunctions);
			DateTimeOffset loginInformationCreationTime2 = await loginInformationSecret.GetCreationTimeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetCreationTimeAsync(newCreationTime, new byte[] { 1, 29, 3}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.AreEqual(loginInformation.creationTime, loginInformationCreationTime1.ToUnixTimeSeconds());
			Assert.AreEqual(newCreationTime.ToUnixTimeSeconds(), loginInformationCreationTime2.ToUnixTimeSeconds());
		}

		[Test]
		public async Task SetLoginInformationModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			DateTimeOffset newModificationTime = DateTimeOffset.UtcNow.AddDays(1); 

			// Act
			DateTimeOffset loginInformationModificationTime1 = await loginInformationSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeTrue = await loginInformationSecret.SetModificationTimeAsync(newModificationTime, derivedKey, securityAsyncFunctions);
			DateTimeOffset loginInformationModificationTime2 = await loginInformationSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetModificationTimeAsync(newModificationTime, new byte[] { 1, 29, 3, 99, 134, 255, 0 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.AreEqual(loginInformation.modificationTime, loginInformationModificationTime1.ToUnixTimeSeconds());
			Assert.AreEqual(newModificationTime.ToUnixTimeSeconds(), loginInformationModificationTime2.ToUnixTimeSeconds());
		}

		[Test]
		public async Task SetLoginInformationIconAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			byte[] newIcon = new byte[] { 1, 2, 44, 55, 66, 33, 89, 23, 222, 111, 100, 99, 45, 127, 198, 255 };

			// Act
			bool shouldBeTrue = await loginInformationSecret.SetIconAsync(newIcon, derivedKey, securityAsyncFunctions);
			byte[] loginInformationIcon2 = await loginInformationSecret.GetIconAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetIconAsync(newIcon, new byte[] { 13, 129, 3, 99, 134, 255, 0}, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			CollectionAssert.AreEqual(newIcon, loginInformationIcon2);
		}

		[Test]
		public async Task SetLoginInformationCategoryAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newCategory = "Finance";

			// Act
			bool shouldBeTrue = await loginInformationSecret.SetCategoryAsync(newCategory, derivedKey, securityAsyncFunctions);
			string loginInformationCategory2 = await loginInformationSecret.GetCategoryAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetCategoryAsync(newCategory, new byte[] { 13, 129, 0, 99, 134, 255, 0 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationCategory2));
			Assert.AreEqual(newCategory, loginInformationCategory2);
		}

		[Test]
		public async Task SetLoginInformationTagsAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string newTags = "Finance; Home; Travel; Future";

			// Act
			bool shouldBeTrue = await loginInformationSecret.SetTagsAsync(newTags, derivedKey, securityAsyncFunctions);
			string loginInformationTags2 = await loginInformationSecret.GetTagsAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await loginInformationSecret.SetTagsAsync(newTags, new byte[] { 13, 129, 0, 91, 194, 255, 0 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(loginInformationTags2));
			Assert.AreEqual(newTags, loginInformationTags2);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 88, 9, 107, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xcc, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 192, settingsAES_CTR);

			LoginInformation loginInformation = await LoginInformation.CreateLoginInformationAsync("Wishlist for holidays", "https://example.com", "someone@noexistent.com", "dragon6", "password1", securityAsyncFunctions);

			LoginInformationSecret loginInformationSecret1 = await LoginInformationSecret.CreateLoginInformationSecretAsync(loginInformation, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

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

#endif // ASYNC_WITH_CUSTOM