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

			string noteTitle1 = "some notes";
			string noteText1 = "words are so hard sometimes 😠";
			csc.notes.Add(new Note(noteTitle1, noteText1));

			string filename1 = "test.txt";
			byte[] file1Content = new byte[] { 1, 34, 46, 47,24, 33, 4};
			csc.files.Add(new FileEntry(filename1, file1Content));

			// Act
			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			CommonSecretsContainer cscDerialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(noteTitle1, cscDerialized.notes[0].noteTitle);
			Assert.AreEqual(noteText1, cscDerialized.notes[0].noteText);

			Assert.AreEqual(filename1, cscDerialized.files[0].filename);
			CollectionAssert.AreEqual(file1Content, cscDerialized.files[0].fileContent);
		}

		[Test]
		public void RoundTripComplexTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			int notesAmount = 17;

			for (int i = 0; i < notesAmount; i++)
			{
				csc.notes.Add(ContentGenerator.GenerateRandomNote());
			}

			//int notesSecretAmount = 11;

			// Act
			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			CommonSecretsContainer cscDerialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(notesAmount, csc.notes.Count);
			Assert.AreEqual(notesAmount, cscDerialized.notes.Count);
			for (int i = 0; i < notesAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreNotesEqual(csc.notes[i], cscDerialized.notes[i]));
			}
		}
	}
}