using NUnit.Framework;
using CSCommonSecrets;
using System.Collections.Generic;

namespace Tests
{
	public class LoginInformationSecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

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

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(testDictionary, skaAES_CTR, derivedKey);

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

			string title = "Wishlist for holidays";
			string url = "https://example.com";
			string username = "dragon6";
			string password = "password1";

			LoginInformation loginInformation = new LoginInformation(title, url, username, password);

			LoginInformationSecret loginInformationSecret = new LoginInformationSecret(loginInformation, skaAES_CTR, derivedKey);


			// Act
			string loginInformationTitle = loginInformationSecret.GetNoteTitle(derivedKey);

			// Assert
			Assert.AreEqual(title, loginInformationTitle);
		}
	}
}