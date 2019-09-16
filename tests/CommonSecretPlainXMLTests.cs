using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.IO;
using System.Text;
using System.Xml;  
using System.Xml.Serialization; 

namespace Tests
{
	public class CommonSecretPlainXMLTests
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
			string xml = null;

			var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
			var stringWriter = new StringWriter();
			using (var writer = XmlWriter.Create(stringWriter))
			{
				xmlserializer.Serialize(writer, csc);
				xml = stringWriter.ToString();
			}

			// Assert
			Assert.IsNotNull(xml);
			Assert.IsTrue(xml.Contains("version"));
		}

		[Test]
		public void RoundTripTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string noteTitle1 = "some notes";
			string noteText1 = "words are so hard sometimes 😠";
			csc.notes.Add(new Note(noteTitle1, noteText1));

			string filename1 = "test.txt";
			byte[] file1Content = new byte[] { 1, 34, 46, 47, 24, 33, 4};
			csc.files.Add(new FileEntry(filename1, file1Content));

			// Act
			string xml = null;
			var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
			var stringWriter = new StringWriter();
			using (var writer = XmlWriter.Create(stringWriter))
			{
				xmlserializer.Serialize(writer, csc);
				xml = stringWriter.ToString();
			}

			CommonSecretsContainer cscDeserialized = null;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
			{
				cscDeserialized = (CommonSecretsContainer)xmlserializer.Deserialize(reader);
			}

			// Assert
			Assert.AreEqual(noteTitle1, System.Text.Encoding.UTF8.GetString(cscDeserialized.notes[0].noteTitle));
			Assert.AreEqual(noteText1, System.Text.Encoding.UTF8.GetString(cscDeserialized.notes[0].noteText));

			Assert.AreEqual(filename1, System.Text.Encoding.UTF8.GetString(cscDeserialized.files[0].filename));
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

			string xml = null;
			var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
			var stringWriter = new StringWriter();
			using (var writer = XmlWriter.Create(stringWriter))
			{
				xmlserializer.Serialize(writer, csc);
				xml = stringWriter.ToString();
			}

			CommonSecretsContainer cscDeserialized = null;
			using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
			{
				cscDeserialized = (CommonSecretsContainer)xmlserializer.Deserialize(reader);
			}

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