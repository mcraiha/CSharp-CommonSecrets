#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class FileEntryAsyncTests
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

			FileEntry fe1 = new FileEntry();
			FileEntry fe2 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(fe1);
			Assert.IsNotNull(fe2);
		}

		[Test]
		public async Task GetValuesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string filename = "notnice.doc";
			byte[] contentForFile = Encoding.UTF8.GetBytes("👺 is evil!");
			FileEntry fe = await FileEntry.CreateFileEntryAsync(filename, contentForFile, securityAsyncFunctions);

			// Act

			// Assert
			Assert.AreEqual(filename, fe.GetFilename());
			Assert.AreEqual(contentForFile.LongLength, fe.GetFileContentLengthInBytes());
			CollectionAssert.AreEqual(contentForFile, fe.fileContent);
		}

		[Test]
		public async Task ModificationUpdateTimestampAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			FileEntry fe = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), securityAsyncFunctions);

			// Act
			var modificationTime1 = fe.modificationTime;
			System.Threading.Thread.Sleep(1100);
			await fe.UpdateFileEntryAsync(fe.GetFilename(), new byte[] { }, securityAsyncFunctions);
			var modificationTime2 = fe.modificationTime;

			// Assert
			Assert.AreNotEqual(modificationTime1, modificationTime2);
		}
		
		[Test]
		public async Task ChecksumChangesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			FileEntry fe1 = new FileEntry();
			FileEntry fe2 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), dto, securityAsyncFunctions);
			FileEntry fe3 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), dto, securityAsyncFunctions);

			// Act
			string checksum1 = fe1.GetChecksumAsHex();
			string checksum2 = fe2.GetChecksumAsHex();
			string checksum3 = fe3.GetChecksumAsHex();

			byte[] newContent = new byte[fe3.fileContent.Length];
			Array.Copy(fe3.fileContent, newContent, fe3.fileContent.Length);
			newContent[0] = 127;
			await fe3.UpdateFileEntryAsync(fe3.GetFilename(), newContent, securityAsyncFunctions);
			string checksum4 = fe3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public async Task ShallowCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			FileEntry fe1 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), securityAsyncFunctions);

			// Act
			FileEntry fe2 = fe1.ShallowCopy();

			string checksum1 = fe1.GetChecksumAsHex();
			string checksum2 = fe2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(fe2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string filename = "sometext.txt";
			string fileContent = "Some text here, yes. And even more";
			FileEntry fe1 = await FileEntry.CreateFileEntryAsync(filename, Encoding.UTF8.GetBytes(fileContent), securityAsyncFunctions);

			// Act
			FileEntry fe2 = new FileEntry(fe1);

			// Assert
			Assert.AreNotSame(fe1.filename, fe2.filename);
			CollectionAssert.AreEqual(fe1.filename, fe2.filename);

			Assert.AreNotSame(fe1.fileContent, fe2.fileContent);
			CollectionAssert.AreEqual(fe1.fileContent, fe2.fileContent);

			Assert.AreEqual(fe1.modificationTime, fe2.modificationTime);
			Assert.AreEqual(fe1.creationTime, fe2.creationTime);

			Assert.AreEqual(fe1.checksum, fe2.checksum);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			FileEntry fe1 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), securityAsyncFunctions);

			// Act
			string checksum1 = fe1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(fe1, Formatting.Indented);

			FileEntry fe2 = JsonConvert.DeserializeObject<FileEntry>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, fe2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			FileEntry fe1 = await FileEntry.CreateFileEntryAsync("sometext.txt", Encoding.UTF8.GetBytes("Some text here, yes."), securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await fe1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			fe1.checksum = fe1.checksum.Remove(0, 1);
			bool shouldBeFalse = await fe1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM