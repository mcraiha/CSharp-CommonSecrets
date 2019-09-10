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
			Assert.AreEqual(noteTitle1, cscDeserialized.notes[0].noteTitle);
			Assert.AreEqual(noteText1, cscDeserialized.notes[0].noteText);

			Assert.AreEqual(filename1, cscDeserialized.files[0].filename);
			CollectionAssert.AreEqual(file1Content, cscDeserialized.files[0].fileContent);
		}
	}
}