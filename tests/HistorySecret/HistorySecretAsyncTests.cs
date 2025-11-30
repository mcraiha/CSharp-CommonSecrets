#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class HistorySecretAsyncTests
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

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ History.descriptionTextKey, "Very boring description"u8.ToArray() }
			};

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(historySecret);
			Assert.IsNotNull(historySecret.audalfData);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 112, 120, 103, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0x13, 0xaa, 0xf5, 0x36, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			HistorySecret historySecretCopy = new HistorySecret(historySecret);
			string descriptionInCopy = await historySecretCopy.GetDescriptionAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(descriptionInCopy));
			Assert.AreEqual(description, descriptionInCopy);
			Assert.AreNotSame(historySecret.audalfData, historySecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(historySecret.keyIdentifier, historySecretCopy.keyIdentifier);
			Assert.AreNotSame(historySecret.keyIdentifier, historySecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(historySecret.checksum, historySecretCopy.checksum);
		}
		
		[Test]
		public async Task GetHistoryAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			History historyCopy = await historySecret.GetHistoryAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreHistoriesEqual(history, historyCopy));
			Assert.AreEqual(history.occurenceTime, historyCopy.occurenceTime);
		}

		[Test]
		public async Task GetWithInvalidKeyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			byte[] derivedKeyInvalid = Mutator.CreateMutatedByteArray(derivedKey);

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await historySecret.GetDescriptionAsync(null, securityAsyncFunctions));
			Assert.ThrowsAsync<ArgumentException>(async () => await historySecret.GetDescriptionAsync(derivedKeyInvalid, securityAsyncFunctions));
		}
		
		[Test]
		public async Task GetEventTypeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

            HistoryEventType eventType = HistoryEventType.Delete;
			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(eventType, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			HistoryEventType rtEventType = await historySecret.GetEventTypeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(eventType, rtEventType);
		}
		
		[Test]
		public async Task GetDescriptionAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string rtDescription = await historySecret.GetDescriptionAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(description, rtDescription);
		}
		
		
		[Test]
		public async Task GetOccurenceTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			DateTimeOffset historyOccurenceTime = await historySecret.GetOccurenceTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(history.GetOccurenceTime(), historyOccurenceTime);
		}
		
		[Test]
		public async Task GetKeyIdentifierAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 25, 138, 78, 83, 111, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0x13, 0xb5, 0x58, 0x59, 0x13, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			// Act
			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, keyIdentifier, skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(keyIdentifier, historySecret.GetKeyIdentifier());
		}
		
		[Test]
		public async Task CanBeDecryptedWithDerivedPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey1 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 36, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x95, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			// Act
			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, keyIdentifier, skaAES_CTR, derivedKey1, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(await historySecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey1, securityAsyncFunctions));
			Assert.IsFalse(await historySecret.CanBeDecryptedWithDerivedPasswordAsync(null, securityAsyncFunctions));
			Assert.IsFalse(await historySecret.CanBeDecryptedWithDerivedPasswordAsync(new byte[] {}, securityAsyncFunctions));
			Assert.IsFalse(await historySecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey2, securityAsyncFunctions));
		}
		
		[Test]
		public async Task SetDescriptionAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 98, 10, 11, 12, 131, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string desccriptionNew = "It was this instead";

			// Act
			bool shouldBeTrue = await historySecret.SetDescriptionAsync(desccriptionNew, derivedKey, securityAsyncFunctions);
			string description2 = await historySecret.GetDescriptionAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await historySecret.SetDescriptionAsync(desccriptionNew,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(description2));
			Assert.AreEqual(desccriptionNew, description2);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret1 = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string checksum1 = historySecret1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(historySecret1, Formatting.Indented);

			HistorySecret historySecret2 = JsonConvert.DeserializeObject<HistorySecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, historySecret2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string description = "Some text here, yes.";

			History history = await History.CreateHistoryAsync(HistoryEventType.Create, description, securityAsyncFunctions);

			HistorySecret historySecret1 = await HistorySecret.CreateHistorySecretAsync(history, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await historySecret1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			historySecret1.checksum = historySecret1.checksum.Remove(0, 1);
			bool shouldBeFalse = await historySecret1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM