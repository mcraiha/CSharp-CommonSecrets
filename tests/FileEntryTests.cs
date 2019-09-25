using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class FileEntryTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			FileEntry fe1 = new FileEntry();
			FileEntry fe2 = new FileEntry("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."));

			// Act

			// Assert
			Assert.IsNotNull(fe1);
			Assert.IsNotNull(fe2);
		}

		[Test]
		public void GetValuesTest()
		{
			// Arrange
			string filename = "notnice.doc";
			byte[] contentForFile = Encoding.UTF8.GetBytes("👺 is evil!");
			FileEntry fe = new FileEntry(filename, contentForFile);

			// Act

			// Assert
			Assert.AreEqual(filename, fe.filename);
			CollectionAssert.AreEqual(contentForFile, fe.fileContent);
		}

		[Test]
		public void ModificationUpdateTimestampTest()
		{
			// Arrange
			FileEntry fe = new FileEntry("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."));

			// Act
			var modificationTime1 = fe.modificationTime;
			System.Threading.Thread.Sleep(1100);
			fe.UpdateFileEntry(fe.GetFilename(), new byte[] { });
			var modificationTime2 = fe.modificationTime;

			// Assert
			Assert.AreNotEqual(modificationTime1, modificationTime2);
		}
		
		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			FileEntry fe1 = new FileEntry();
			FileEntry fe2 = new FileEntry("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."));
			FileEntry fe3 = new FileEntry("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."));

			// Act
			string checksum1 = fe1.GetChecksumAsHex();
			string checksum2 = fe2.GetChecksumAsHex();
			string checksum3 = fe3.GetChecksumAsHex();

			byte[] newContent = new byte[fe3.fileContent.Length];
			Array.Copy(fe3.fileContent, newContent, fe3.fileContent.Length);
			newContent[0] = 127;
			fe3.UpdateFileEntry(fe3.GetFilename(), newContent);
			string checksum4 = fe3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			FileEntry fe1 = new FileEntry("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."));

			// Act
			string checksum1 = fe1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(fe1, Formatting.Indented);

			FileEntry fe2 = JsonConvert.DeserializeObject<FileEntry>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, fe2.GetChecksumAsHex());
		}
	}
}