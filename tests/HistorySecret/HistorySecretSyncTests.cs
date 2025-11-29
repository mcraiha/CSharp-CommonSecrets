#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class HistorySecretSyncTests
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

			HistorySecret historySecret = new HistorySecret(testDictionary, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(historySecret);
			Assert.IsNotNull(historySecret.audalfData);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 112, 120, 103, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0x13, 0xaa, 0xf5, 0x36, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";
			History history1 = new History(HistoryEventType.Create, description);

			HistorySecret historySecret = new HistorySecret(history1, "does not matter", skaAES_CTR, derivedKey);

			// Act
			HistorySecret historySecretCopy = new HistorySecret(historySecret);
			string descriptionInCopy = historySecretCopy.GetDescription(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(descriptionInCopy));
			Assert.AreEqual(description, descriptionInCopy);
			Assert.AreNotSame(historySecret.audalfData, historySecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(historySecret.keyIdentifier, historySecretCopy.keyIdentifier);
			Assert.AreNotSame(historySecret.keyIdentifier, historySecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(historySecret.checksum, historySecretCopy.checksum);
		}

		[Test]
		public void GetHistoryTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			History historyCopy = historySecret.GetHistory(derivedKey);

			HistorySecret historySecretInvalid = new HistorySecret(historySecret);
			historySecretInvalid.audalfData[0] = (byte)(255 - historySecretInvalid.audalfData[0]);

			// Assert
			Assert.AreEqual(history.occurenceTime, historyCopy.occurenceTime);
			Assert.Throws<ArgumentNullException>(() => historySecret.GetHistory(null));
			Assert.Throws<ArgumentException>(() => historySecretInvalid.GetHistory(derivedKey));
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

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.Throws<ArgumentNullException>(() => historySecret.GetDescription(null));
			Assert.Throws<ArgumentException>(() => historySecret.GetDescription(derivedKeyInvalid));
		}

		[Test]
		public void GetHistoryEventTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			HistoryEventType rtHistoryEventType = historySecret.GetEventType(derivedKey);

			// Assert
			Assert.AreEqual(historyEventType, rtHistoryEventType);
		}

		[Test]
		public void GetDescriptionTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string rtDescription = historySecret.GetDescription(derivedKey);

			// Assert
			Assert.AreEqual(description, rtDescription);
		}

		[Test]
		public void GetOccurenceTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			DateTimeOffset fileEntryCreationTime = historySecret.GetOccurenceTime(derivedKey);

			// Assert
			Assert.AreEqual(history.GetOccurenceTime(), fileEntryCreationTime);
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

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			// Act
			HistorySecret historySecret = new HistorySecret(history, keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, historySecret.GetKeyIdentifier());
		}

		[Test]
		public void CanBeDecryptedWithDerivedPasswordTest()
		{
			byte[] derivedKey1 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			// Act
			HistorySecret historySecret = new HistorySecret(history, keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(historySecret.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(historySecret.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(historySecret.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(historySecret.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetEventTypeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 98, 10, 11, 12, 131, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", ska, derivedKey);

			// Act
			bool shouldBeTrue = historySecret.SetEventType(historyEventType, derivedKey);
			HistoryEventType historyEventType2 = historySecret.GetEventType(derivedKey);
			bool shouldBeFalse = historySecret.SetEventType(historyEventType,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.AreEqual(historyEventType, historyEventType2);
		}

		[Test]
		public void SetDescriptionTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 98, 10, 11, 12, 131, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);

			HistorySecret historySecret = new HistorySecret(history, "does not matter", ska, derivedKey);

			// Act
			bool shouldBeTrue = historySecret.SetDescription(description, derivedKey);
			string description2 = historySecret.GetDescription(derivedKey);
			bool shouldBeFalse = historySecret.SetDescription(description,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			CollectionAssert.AreEqual(description, description2);
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);
			HistorySecret fes1 = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string checksum1 = fes1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(fes1, Formatting.Indented);

			HistorySecret fes2 = JsonConvert.DeserializeObject<HistorySecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, fes2.GetChecksumAsHex());
		}

		[Test]
		public void CheckIfChecksumMatchesContentTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			HistoryEventType historyEventType = HistoryEventType.Update;
			string description = "Some text here, yes.";

			History history = new History(historyEventType, description);
			HistorySecret fes1 = new HistorySecret(history, "does not matter", skaAES_CTR, derivedKey);

			// Act
			bool shouldBeTrue = fes1.CheckIfChecksumMatchesContent();
			fes1.checksum = fes1.checksum.Remove(0, 1);
			bool shouldBeFalse = fes1.CheckIfChecksumMatchesContent();

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM