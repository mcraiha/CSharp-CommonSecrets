using NUnit.Framework;
using CSCommonSecrets;
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

			NoteSecret noteSecret = new NoteSecret(testDictionary, skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(noteSecret);
			Assert.IsNotNull(noteSecret.audalfData);
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

			NoteSecret noteSecret = new NoteSecret(note, skaAES_CTR, derivedKey);

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

			NoteSecret noteSecret = new NoteSecret(note, skaAES_CTR, derivedKey);

			// Act
			string noteText = noteSecret.GetNoteText(derivedKey);

			// Assert
			Assert.AreEqual(text, noteText);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string title = "Wishlist for holidays";
			string text = "peace, happiness, freedom";

			Note note = new Note(title, text);

			NoteSecret noteSecret1 = new NoteSecret(note, skaAES_CTR, derivedKey);

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