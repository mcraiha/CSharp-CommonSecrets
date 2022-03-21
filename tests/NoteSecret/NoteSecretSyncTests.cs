#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class NoteSecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 1, 1, 4, 5, 6, 7, 7, 7, 10, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf1, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ Note.noteTitleKey, "My shopping list for tomorrow"}
			};

			NoteSecret noteSecret = new NoteSecret(testDictionary, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(noteSecret);
			Assert.IsNotNull(noteSecret.audalfData);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xaa, 0xf5, 0xf6, 0xbb, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom and something";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);

			// Act
			NoteSecret noteSecretCopy = new NoteSecret(noteSecret);
			string noteTitle = noteSecretCopy.GetNoteTitle(derivedKey);

			// Assert
			Assert.IsFalse(string.IsNullOrEmpty(noteTitle));
			Assert.AreEqual(title, noteTitle);
			Assert.AreNotSame(noteSecret.audalfData, noteSecretCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(noteSecret.keyIdentifier, noteSecretCopy.keyIdentifier);
			Assert.AreNotSame(noteSecret.keyIdentifier, noteSecretCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(noteSecret.checksum, noteSecretCopy.checksum);
		}

		[Test]
		public void GetNoteTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);

			// Act
			Note noteCopy = noteSecret.GetNote(derivedKey);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreNotesEqual(note, noteCopy));
			Assert.AreEqual(note.creationTime, noteCopy.creationTime);
			Assert.AreEqual(note.modificationTime, noteCopy.modificationTime);
		}


		[Test]
		public void GetNoteTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string noteTitle = noteSecret.GetNoteTitle(derivedKey);

			// Assert
			Assert.AreEqual(title, noteTitle);
		}

		[Test]
		public void GetNoteTextTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string noteText = noteSecret.GetNoteText(derivedKey);

			// Assert
			Assert.AreEqual(text, noteText);
		}

		[Test]
		public void GetModificationTimeTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);
			
			// Act
			DateTimeOffset modificationTime1 = noteSecret.GetModificationTime(derivedKey);
			Thread.Sleep(1100);
			noteSecret.SetNoteTitle("1234567", derivedKey);
			DateTimeOffset modificationTime2 = noteSecret.GetModificationTime(derivedKey);

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

			string title = "Wishlist for holidays, eh";
			string text = "peace, happiness, freedom, faster";

			Note note = new Note(title, text);

			// Act
			NoteSecret noteSecret = new NoteSecret(note, keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, noteSecret.GetKeyIdentifier());
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

			string title = "Wishlist for holidays, eh";
			string text = "peace, happiness, freedom, faster";

			Note note = new Note(title, text);

			// Act
			NoteSecret noteSecret = new NoteSecret(note, keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(noteSecret.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(noteSecret.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(noteSecret.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(noteSecret.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetNoteTitleTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			string title = "Wishlist for holidays part IV";
			string text = "peace, happiness, freedom and something...";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", ska, derivedKey);

			string noteTitle1 = "future text that is happy and joyful for all holiday purposes...";

			// Act
			bool shouldBeTrue = noteSecret.SetNoteTitle(noteTitle1, derivedKey);
			string noteTitle2 = noteSecret.GetNoteTitle(derivedKey);
			bool shouldBeFalse = noteSecret.SetNoteTitle(noteTitle1,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(noteTitle2));
			Assert.AreEqual(noteTitle1, noteTitle2);
		}

		[Test]
		public void SetNoteTextTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };

			SymmetricKeyAlgorithm ska = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);

			string title = "Top three";
			string text = "Another long and super boring text for testing purposes.";

			Note note = new Note(title, text);

			NoteSecret noteSecret = new NoteSecret(note, "does not matter", ska, derivedKey);

			string noteText1 = "Not so boring but still bit something text for test case";

			// Act
			bool shouldBeTrue = noteSecret.SetNoteText(noteText1, derivedKey);
			string noteText2 = noteSecret.GetNoteText(derivedKey);
			bool shouldBeFalse = noteSecret.SetNoteText(noteText1,  new byte[] { 1, 2, 3 });

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
			Assert.IsFalse(string.IsNullOrEmpty(noteText2));
			Assert.AreEqual(noteText1, noteText2);
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret1 = new NoteSecret(note, "does not matter", skaAES_CTR, derivedKey);

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

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM