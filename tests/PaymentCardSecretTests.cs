using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class PaymentCardSecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 1, 1, 4, 5, 6, 7, 6, 7, 10, 11, 12, 13, 4, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ PaymentCard.titleKey, "Super cool card"}
			};

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(testDictionary, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(paymentCardSecret);
			Assert.IsNotNull(paymentCardSecret.audalfData);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Bank of  Dragon";
			PaymentCard paymentCard = new PaymentCard(title, "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			PaymentCardSecret paymentCardSecretCopy = new PaymentCardSecret(paymentCardSecret);
			string paymentCardTitle = paymentCardSecretCopy.GetTitle(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(paymentCardTitle));
			Assert.AreEqual(title, paymentCardTitle);
			Assert.AreNotSame(paymentCardSecret.audalfData, paymentCardSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(paymentCardSecret.keyIdentifier, paymentCardSecretCopy.keyIdentifier);
			Assert.AreNotSame(paymentCardSecret.keyIdentifier, paymentCardSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(paymentCardSecret.checksum, paymentCardSecretCopy.checksum);
		}

		[Test]
		public void GetPaymentCardTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			PaymentCard paymentCardCopy = paymentCardSecret.GetPaymentCard(derivedKey);

			// Assert
			Assert.IsTrue(ComparisonHelper.ArePaymentCardsEqual(paymentCard, paymentCardCopy));
			Assert.AreEqual(paymentCard.creationTime, paymentCardCopy.creationTime);
			Assert.AreEqual(paymentCard.modificationTime, paymentCardCopy.modificationTime);
		}


		[Test]
		public void GetPaymentCardTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Bank of  Dragon";
			PaymentCard paymentCard = new PaymentCard(title, "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardTitle = paymentCardSecret.GetTitle(derivedKey);

			// Assert
			Assert.AreEqual(title, paymentCardTitle);
		}

		[Test]
		public void GetPaymentCardNameOnCardTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string nameOnCard = "Cool Dragon";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", nameOnCard, "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardNameOnCard = paymentCardSecret.GetNameOnCard(derivedKey);

			// Assert
			Assert.AreEqual(nameOnCard, paymentCardNameOnCard);
		}

		[Test]
		public void GetPaymentCardCardTypeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string cardType = "Debit";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", cardType, "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardCardType = paymentCardSecret.GetCardType(derivedKey);

			// Assert
			Assert.AreEqual(cardType, paymentCardCardType);
		}

		[Test]
		public void GetPaymentCardNumberTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 51, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string number = "0000000000001234";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", number, "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardNumber = paymentCardSecret.GetNumber(derivedKey);

			// Assert
			Assert.AreEqual(number, paymentCardNumber);
		}

		[Test]
		public void GetPaymentCardSecurityCodeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 98, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string securityCode = "111";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", securityCode, "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardSecurityCode = paymentCardSecret.GetSecurityCode(derivedKey);

			// Assert
			Assert.AreEqual(securityCode, paymentCardSecurityCode);
		}

		[Test]
		public void GetPaymentCardStartDateTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 200, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string startDate = "11/20";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", startDate, "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardStartDate = paymentCardSecret.GetStartDate(derivedKey);

			// Assert
			Assert.AreEqual(startDate, paymentCardStartDate);
		}

		[Test]
		public void GetPaymentCardExpirationDateTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 100, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string expirationDate = "05/33";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", expirationDate, "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardStartDate = paymentCardSecret.GetExpirationDate(derivedKey);

			// Assert
			Assert.AreEqual(expirationDate, paymentCardStartDate);
		}

		[Test]
		public void GetPaymentCardNotesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 100, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string notes = "Super cool card I have here";
			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", notes);

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string paymentCardNotes = paymentCardSecret.GetNotes(derivedKey);

			// Assert
			Assert.AreEqual(notes, paymentCardNotes);
		}

		[Test]
		public void GetModificationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 15, 6, 7, 8, 9, 10, 11, 112, 139, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);
			
			// Act
			DateTimeOffset modificationTime1 = paymentCardSecret.GetModificationTime(derivedKey);
			Thread.Sleep(1100);
			paymentCardSecret.SetTitle("1234567", derivedKey);
			DateTimeOffset modificationTime2 = paymentCardSecret.GetModificationTime(derivedKey);

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
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

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act
			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, paymentCardSecret.GetKeyIdentifier());
		}

		[Test]
		public void CanBeDecryptedWithDerivedPassword()
		{
			byte[] derivedKey1 = new byte[16] { 11, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 11, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act
			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(paymentCardSecret.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(paymentCardSecret.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(paymentCardSecret.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(paymentCardSecret.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetPaymentCardTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string title = "future text that is happy and joyful for all holiday purposes...";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetTitle(title, derivedKey);
			string titleAfterModification = paymentCardSecret.GetTitle(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetTitle(title, new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(titleAfterModification));
			Assert.AreEqual(title, titleAfterModification);
		}

		[Test]
		public void SetPaymentCardNameOnCardTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string nameOnCard = "Da coolest dragon";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetNameOnCard(nameOnCard, derivedKey);
			string nameOnCardAfterModification = paymentCardSecret.GetNameOnCard(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetNameOnCard(nameOnCard,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(nameOnCardAfterModification));
			Assert.AreEqual(nameOnCard, nameOnCardAfterModification);
		}

		[Test]
		public void SetPaymentCardCardTypeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string cardType = "Credit!";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetCardType(cardType, derivedKey);
			string cardTypeAfterModification = paymentCardSecret.GetCardType(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetCardType(cardType,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(cardTypeAfterModification));
			Assert.AreEqual(cardType, cardTypeAfterModification);
		}

		[Test]
		public void SetPaymentCardNumberTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string number = "1234500000001234";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetNumber(number, derivedKey);
			string numberAfterModification = paymentCardSecret.GetNumber(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetNumber(number, new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(numberAfterModification));
			Assert.AreEqual(number, numberAfterModification);
		}

		[Test]
		public void SetPaymentCardSecurityCodeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string securityCode = "987";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetSecurityCode(securityCode, derivedKey);
			string securityCodeAfterModification = paymentCardSecret.GetSecurityCode(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetSecurityCode(securityCode,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(securityCodeAfterModification));
			Assert.AreEqual(securityCode, securityCodeAfterModification);
		}

		[Test]
		public void SetPaymentCardStartDateTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string startDate = "12/22";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetStartDate(startDate, derivedKey);
			string startDateAfterModification = paymentCardSecret.GetStartDate(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetStartDate(startDate,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(startDateAfterModification));
			Assert.AreEqual(startDate, startDateAfterModification);
		}

		[Test]
		public void SetPaymentCardExpirationDateTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string expirationDate = "06/36";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetExpirationDate(expirationDate, derivedKey);
			string expirationDateAfterModification = paymentCardSecret.GetExpirationDate(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetExpirationDate(expirationDate,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(expirationDateAfterModification));
			Assert.AreEqual(expirationDate, expirationDateAfterModification);
		}

		[Test]
		public void SetPaymentCardNotesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", ska, derivedKey);

			string notes = "Yet another non useful note";

			// Act
			bool shouldBeTrue = paymentCardSecret.SetNotes(notes, derivedKey);
			string notesAfterModification = paymentCardSecret.GetNotes(derivedKey);
			bool shouldBeFalse = paymentCardSecret.SetNotes(notes, new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(notesAfterModification));
			Assert.AreEqual(notes, notesAfterModification);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			PaymentCard paymentCard = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			PaymentCardSecret paymentCardSecret = new PaymentCardSecret(paymentCard, "does not matter", skaAES_CTR, derivedKey);

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
