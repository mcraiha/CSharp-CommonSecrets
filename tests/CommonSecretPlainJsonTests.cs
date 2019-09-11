using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class CommonSecretPlainJsonTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			// Act
			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			// Assert
			Assert.IsNotNull(json);
			Assert.IsTrue(json.Contains("version"));
		}

		[Test]
		public void RoundTripTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string loginTitle1 = "Some nice website";
			string loginUrl1 = "https://hopefullynobodybuysthisdomain.com";
			string loginUsername1 = "superniceuser";
			string loginPassword1 = "dragon77!"; 

			string noteTitle1 = "some notes";
			string noteText1 = "words are so hard sometimes 😠";

			string filename1 = "test.txt";
			byte[] file1Content = new byte[] { 1, 34, 46, 47, 24, 33, 4};
			
			// Act
			csc.loginInformations.Add(new LoginInformation(loginTitle1, loginUrl1, loginUsername1, loginPassword1));
			csc.notes.Add(new Note(noteTitle1, noteText1));
			csc.files.Add(new FileEntry(filename1, file1Content));

			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);
			Console.WriteLine(json);

			CommonSecretsContainer cscDeserialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(loginTitle1, cscDeserialized.loginInformations[0].title);
			Assert.AreEqual(loginUrl1, cscDeserialized.loginInformations[0].url);
			Assert.AreEqual(loginUsername1, cscDeserialized.loginInformations[0].username);
			Assert.AreEqual(loginPassword1, cscDeserialized.loginInformations[0].password);

			Assert.AreEqual(noteTitle1, cscDeserialized.notes[0].noteTitle);
			Assert.AreEqual(noteText1, cscDeserialized.notes[0].noteText);

			Assert.AreEqual(filename1, cscDeserialized.files[0].filename);
			CollectionAssert.AreEqual(file1Content, cscDeserialized.files[0].fileContent);
		}

		[Test]
		public void RoundTripComplexTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string password = "dragon667";
			byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			SettingsAES_CTR settingsAES_CTR1 = new SettingsAES_CTR(initialCounter1);
			SymmetricKeyAlgorithm skaAES = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR1);

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry("does not matter");

			int notesAmount = 17;
			int notesSecretAmount = 11;

			// Act
			byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

			for (int i = 0; i < notesAmount; i++)
			{
				csc.notes.Add(ContentGenerator.GenerateRandomNote());
			}

			for (int i = 0; i < notesSecretAmount; i++)
			{
				csc.noteSecrets.Add(new NoteSecret(ContentGenerator.GenerateRandomNote(), skaAES, derivedPassword));
			}

			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			CommonSecretsContainer cscDeserialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(notesAmount, csc.notes.Count);
			Assert.AreEqual(notesAmount, cscDeserialized.notes.Count);
			for (int i = 0; i < notesAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreNotesEqual(csc.notes[i], cscDeserialized.notes[i]));
			}

			Assert.AreEqual(notesSecretAmount, csc.noteSecrets.Count);
			Assert.AreEqual(notesSecretAmount, cscDeserialized.noteSecrets.Count);
			for (int i = 0; i < notesSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreNotesSecretEqual(csc.noteSecrets[i], cscDeserialized.noteSecrets[i]));
			}
		}
	}
}