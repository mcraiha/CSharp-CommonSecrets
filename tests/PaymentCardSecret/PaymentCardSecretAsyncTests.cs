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
	public class PaymentCardSecretAsyncTests
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

			byte[] derivedKey = new byte[16] { 1, 1, 1, 4, 5, 6, 7, 6, 7, 10, 11, 12, 13, 4, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ PaymentCard.titleKey, "Super cool card"}
			};

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(paymentCardSecret);
			Assert.IsNotNull(paymentCardSecret.audalfData);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Bank of  Dragon";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync(title, "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			PaymentCardSecret paymentCardSecretCopy = new PaymentCardSecret(paymentCardSecret);
			string paymentCardTitle = await paymentCardSecretCopy.GetTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(paymentCardTitle));
			Assert.AreEqual(title, paymentCardTitle);
			Assert.AreNotSame(paymentCardSecret.audalfData, paymentCardSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(paymentCardSecret.keyIdentifier, paymentCardSecretCopy.keyIdentifier);
			Assert.AreNotSame(paymentCardSecret.keyIdentifier, paymentCardSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(paymentCardSecret.checksum, paymentCardSecretCopy.checksum);
		}

		[Test]
		public async Task GetPaymentCardAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			PaymentCard paymentCardCopy = await paymentCardSecret.GetPaymentCardAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(ComparisonHelper.ArePaymentCardsEqual(paymentCard, paymentCardCopy));
			Assert.AreEqual(paymentCard.creationTime, paymentCardCopy.creationTime);
			Assert.AreEqual(paymentCard.modificationTime, paymentCardCopy.modificationTime);
		}


		[Test]
		public async Task GetPaymentCardTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Bank of  Dragon";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync(title, "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardTitle = await paymentCardSecret.GetTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(title, paymentCardTitle);
		}

		[Test]
		public async Task GetPaymentCardNameOnCardAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string nameOnCard = "Cool Dragon";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", nameOnCard, "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardNameOnCard = await paymentCardSecret.GetNameOnCardAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(nameOnCard, paymentCardNameOnCard);
		}

		[Test]
		public async Task GetPaymentCardCardTypeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string cardType = "Debit";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", cardType, "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardCardType = await paymentCardSecret.GetCardTypeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(cardType, paymentCardCardType);
		}

		[Test]
		public async Task GetPaymentCardNumberAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 51, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string number = "0000000000001234";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", number, "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardNumber = await paymentCardSecret.GetNumberAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(number, paymentCardNumber);
		}

		[Test]
		public async Task GetPaymentCardSecurityCodeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 98, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string securityCode = "111";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", securityCode, "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardSecurityCode = await paymentCardSecret.GetSecurityCodeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(securityCode, paymentCardSecurityCode);
		}

		[Test]
		public async Task GetPaymentCardStartDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 200, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string startDate = "11/20";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", startDate, "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardStartDate = await paymentCardSecret.GetStartDateAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(startDate, paymentCardStartDate);
		}

		[Test]
		public async Task GetPaymentCardExpirationDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 100, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string expirationDate = "05/33";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", expirationDate, "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardStartDate = await paymentCardSecret.GetExpirationDateAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(expirationDate, paymentCardStartDate);
		}

		[Test]
		public async Task GetPaymentCardNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 100, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string notes = "Super cool card I have here";
			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", notes, securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string paymentCardNotes = await paymentCardSecret.GetNotesAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(notes, paymentCardNotes);
		}

		[Test]
		public async Task GetCreationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			DateTimeOffset paymentCardCreationTime = await paymentCardSecret.GetCreationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(paymentCard.GetCreationTime(), paymentCardCreationTime);
		}

		[Test]
		public async Task GetModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 15, 6, 7, 8, 9, 10, 11, 112, 139, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);
			
			// Act
			DateTimeOffset modificationTime1 = await paymentCardSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);
			await Task.Delay(1100);
			await paymentCardSecret.SetTitleAsync("1234567", derivedKey, securityAsyncFunctions);
			DateTimeOffset modificationTime2 = await paymentCardSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public async Task GetKeyIdentifierAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x59, 0x15, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(keyIdentifier, paymentCardSecret.GetKeyIdentifier());
		}

		[Test]
		public async Task CanBeDecryptedWithDerivedPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey1 = new byte[16] { 11, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 11, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, keyIdentifier, skaAES_CTR, derivedKey1, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(await paymentCardSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey1, securityAsyncFunctions));
			Assert.IsFalse(await paymentCardSecret.CanBeDecryptedWithDerivedPasswordAsync(null, securityAsyncFunctions));
			Assert.IsFalse(await paymentCardSecret.CanBeDecryptedWithDerivedPasswordAsync(new byte[] {}, securityAsyncFunctions));
			Assert.IsFalse(await paymentCardSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey2, securityAsyncFunctions));
		}

		[Test]
		public async Task SetPaymentCardTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string title = "future text that is happy and joyful for all holiday purposes...";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetTitleAsync(title, derivedKey, securityAsyncFunctions);
			string titleAfterModification = await paymentCardSecret.GetTitleAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetTitleAsync(title, new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(titleAfterModification));
			Assert.AreEqual(title, titleAfterModification);
		}

		[Test]
		public async Task SetPaymentCardNameOnCardAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string nameOnCard = "Da coolest dragon";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetNameOnCardAsync(nameOnCard, derivedKey, securityAsyncFunctions);
			string nameOnCardAfterModification = await paymentCardSecret.GetNameOnCardAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetNameOnCardAsync(nameOnCard,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(nameOnCardAfterModification));
			Assert.AreEqual(nameOnCard, nameOnCardAfterModification);
		}

		[Test]
		public async Task SetPaymentCardCardTypeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string cardType = "Credit!";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetCardTypeAsync(cardType, derivedKey, securityAsyncFunctions);
			string cardTypeAfterModification = await paymentCardSecret.GetCardTypeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetCardTypeAsync(cardType,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(cardTypeAfterModification));
			Assert.AreEqual(cardType, cardTypeAfterModification);
		}

		[Test]
		public async Task SetPaymentCardNumberAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string number = "1234500000001234";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetNumberAsync(number, derivedKey, securityAsyncFunctions);
			string numberAfterModification = await paymentCardSecret.GetNumberAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetNumberAsync(number, new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(numberAfterModification));
			Assert.AreEqual(number, numberAfterModification);
		}

		[Test]
		public async Task SetPaymentCardSecurityCodeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string securityCode = "987";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetSecurityCodeAsync(securityCode, derivedKey, securityAsyncFunctions);
			string securityCodeAfterModification = await paymentCardSecret.GetSecurityCodeAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetSecurityCodeAsync(securityCode,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(securityCodeAfterModification));
			Assert.AreEqual(securityCode, securityCodeAfterModification);
		}

		[Test]
		public async Task SetPaymentCardStartDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string startDate = "12/22";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetStartDateAsync(startDate, derivedKey, securityAsyncFunctions);
			string startDateAfterModification = await paymentCardSecret.GetStartDateAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetStartDateAsync(startDate,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(startDateAfterModification));
			Assert.AreEqual(startDate, startDateAfterModification);
		}

		[Test]
		public async Task SetPaymentCardExpirationDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string expirationDate = "06/36";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetExpirationDateAsync(expirationDate, derivedKey, securityAsyncFunctions);
			string expirationDateAfterModification = await paymentCardSecret.GetExpirationDateAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetExpirationDateAsync(expirationDate,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(expirationDateAfterModification));
			Assert.AreEqual(expirationDate, expirationDateAfterModification);
		}

		[Test]
		public async Task SetPaymentCardNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string notes = "Yet another non useful note";

			// Act
			bool shouldBeTrue = await paymentCardSecret.SetNotesAsync(notes, derivedKey, securityAsyncFunctions);
			string notesAfterModification = await paymentCardSecret.GetNotesAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await paymentCardSecret.SetNotesAsync(notes, new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(notesAfterModification));
			Assert.AreEqual(notes, notesAfterModification);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			PaymentCardSecret paymentCardSecret = await PaymentCardSecret.CreatePaymentCardSecretAsync(paymentCard, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string checksum1 = paymentCardSecret.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(paymentCardSecret, Formatting.Indented);

			PaymentCardSecret paymentCardSecret2 = JsonConvert.DeserializeObject<PaymentCardSecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, paymentCardSecret2.GetChecksumAsHex());
		}
	}
}

#endif // ASYNC_WITH_CUSTOM