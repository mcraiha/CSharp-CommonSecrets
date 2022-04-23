#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class NoteAsyncTests
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

			Note note1 = new Note();
			Note note2 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(note1);
			Assert.IsNotNull(note2);
		}

		[Test]
		public async Task GetValuesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string title = "My shopping list";
			string text = "Cheese, cucumber, mayo, lettuce, tomato ...";
			Note note = await Note.CreateNoteAsync(title, text, securityAsyncFunctions);

			// Act

			// Assert
			Assert.AreEqual(title, note.GetNoteTitle());
			Assert.AreEqual(text, note.GetNoteText());
		}

		[Test]
		public async Task ModificationUpdateTimestampAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Note note = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", securityAsyncFunctions);

			// Act
			var modificationTime1 = note.modificationTime;
			System.Threading.Thread.Sleep(1100);
			await note.UpdateNoteAsync(note.GetNoteTitle(), "Some text here, yes. part 2", securityAsyncFunctions);
			var modificationTime2 = note.modificationTime;

			// Assert
			Assert.AreNotEqual(modificationTime1, modificationTime2);
		}
		
		[Test]
		public async Task ChecksumChangesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = new Note();
			Note note2 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", dto, securityAsyncFunctions);
			Note note3 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", dto, securityAsyncFunctions);

			// Act
			string checksum1 = note1.GetChecksumAsHex();
			string checksum2 = note2.GetChecksumAsHex();
			string checksum3 = note3.GetChecksumAsHex();

			string newContent = note3.GetNoteText() + "A";
			await note3.UpdateNoteAsync(note3.GetNoteTitle(), newContent, securityAsyncFunctions);
			string checksum4 = note3.GetChecksumAsHex();

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

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", dto, securityAsyncFunctions);

			// Act
			Note note2 = note1.ShallowCopy();

			string checksum1 = note1.GetChecksumAsHex();
			string checksum2 = note2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(note2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes. And read more!", dto, securityAsyncFunctions);

			// Act
			Note note2 = new Note(note1);

			// Assert
			Assert.AreNotSame(note1.noteTitle, note2.noteTitle);
			CollectionAssert.AreEqual(note1.noteTitle, note2.noteTitle);

			Assert.AreNotSame(note1.noteText, note2.noteText);
			CollectionAssert.AreEqual(note1.noteText, note2.noteText);

			Assert.AreEqual(note1.modificationTime, note2.modificationTime);
			Assert.AreEqual(note1.creationTime, note2.creationTime);

			Assert.AreEqual(note1.checksum, note2.checksum);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Note note1 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", securityAsyncFunctions);

			// Act
			string checksum1 = note1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(note1, Formatting.Indented);

			Note note2 = JsonConvert.DeserializeObject<Note>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, note2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Note note1 = await Note.CreateNoteAsync("Some topic here", "Some text here, yes.", securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await note1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			note1.checksum = note1.checksum.Remove(0, 1);
			bool shouldBeFalse = await note1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM