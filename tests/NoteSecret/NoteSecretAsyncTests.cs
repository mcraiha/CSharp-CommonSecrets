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
	public class NoteSecretAsyncTests
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

			byte[] derivedKey = new byte[16] { 1, 1, 1, 4, 5, 6, 7, 7, 7, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ Note.noteTitleKey, "My shopping list for tomorrow"}
			};

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(noteSecret);
			Assert.IsNotNull(noteSecret.audalfData);
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

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom and something";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			NoteSecret noteSecretCopy = new NoteSecret(noteSecret);
			string noteTitle = await noteSecretCopy.GetNoteTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(noteTitle));
			Assert.AreEqual(title, noteTitle);
			Assert.AreNotSame(noteSecret.audalfData, noteSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(noteSecret.keyIdentifier, noteSecretCopy.keyIdentifier);
			Assert.AreNotSame(noteSecret.keyIdentifier, noteSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(noteSecret.checksum, noteSecretCopy.checksum);
		}

		[Test]
		public async Task GetNoteAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			Note noteCopy = await noteSecret.GetNoteAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreNotesEqual(note, noteCopy));
			Assert.AreEqual(note.creationTime, noteCopy.creationTime);
			Assert.AreEqual(note.modificationTime, noteCopy.modificationTime);
		}


		[Test]
		public async Task GetNoteTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string noteTitle = await noteSecret.GetNoteTitleAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(title, noteTitle);
		}

		[Test]
		public async Task GetNoteTextAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string noteText = await noteSecret.GetNoteTextAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(text, noteText);
		}

		[Test]
		public async Task GetModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);
			
			// Act
			DateTimeOffset modificationTime1 = await noteSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);
			await Task.Delay(1100);
			await noteSecret.SetNoteTitleAsync("1234567", derivedKey, securityAsyncFunctions);
			DateTimeOffset modificationTime2 = await noteSecret.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);

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

			string title = "Wishlist for holidays, eh";
			string text = "peace, happiness, freedom, faster";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			// Act
			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(keyIdentifier, noteSecret.GetKeyIdentifier());
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

			string title = "Wishlist for holidays, eh";
			string text = "peace, happiness, freedom, faster";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			// Act
			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, keyIdentifier, skaAES_CTR, derivedKey1, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(await noteSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey1, securityAsyncFunctions));
			Assert.IsFalse(await noteSecret.CanBeDecryptedWithDerivedPasswordAsync(null, securityAsyncFunctions));
			Assert.IsFalse(await noteSecret.CanBeDecryptedWithDerivedPasswordAsync(new byte[] {}, securityAsyncFunctions));
			Assert.IsFalse(await noteSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey2, securityAsyncFunctions));
		}

		[Test]
		public async Task SetNoteTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			string title = "Wishlist for holidays part IV";
			string text = "peace, happiness, freedom and something...";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string noteTitle1 = "future text that is happy and joyful for all holiday purposes...";

			// Act
			bool shouldBeTrue = await noteSecret.SetNoteTitleAsync(noteTitle1, derivedKey, securityAsyncFunctions);
			string noteTitle2 = await noteSecret.GetNoteTitleAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await noteSecret.SetNoteTitleAsync(noteTitle1,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(noteTitle2));
			Assert.AreEqual(noteTitle1, noteTitle2);
		}

		[Test]
		public async Task SetNoteTextAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR, securityAsyncFunctions);

			string title = "Top three";
			string text = "Another long and super boring text for testing purposes.";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", ska, derivedKey, securityAsyncFunctions);

			string noteText1 = "Not so boring but still bit something text for test case";

			// Act
			bool shouldBeTrue = await noteSecret.SetNoteTextAsync(noteText1, derivedKey, securityAsyncFunctions);
			string noteText2 = await noteSecret.GetNoteTextAsync(derivedKey, securityAsyncFunctions);
			bool shouldBeFalse = await noteSecret.SetNoteTextAsync(noteText1,  new byte[] { 1, 2, 3 }, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(noteText2));
			Assert.AreEqual(noteText1, noteText2);
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

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			NoteSecret noteSecret1 = await NoteSecret.CreateNoteSecretAsync(note, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string checksum1 = noteSecret1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(noteSecret1, Formatting.Indented);

			NoteSecret noteSecret2 = JsonConvert.DeserializeObject<NoteSecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, noteSecret2.GetChecksumAsHex());
		}
	}
}

#endif // ASYNC_WITH_CUSTOM