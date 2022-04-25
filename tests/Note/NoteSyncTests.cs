#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class NoteSyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			Note note1 = new Note();
			Note note2 = new Note("Some topic here", "Some text here, yes.");

			// Act

			// Assert
			Assert.IsNotNull(note1);
			Assert.IsNotNull(note2);
		}

		[Test]
		public void GetValuesTest()
		{
			// Arrange
			string title = "My shopping list";
			string text = "Cheese, cucumber, mayo, lettuce, tomato ...";
			Note note = new Note(title, text);

			// Act

			// Assert
			Assert.AreEqual(title, note.GetNoteTitle());
			Assert.AreEqual(text, note.GetNoteText());
		}

		[Test]
		public void GetCreationTimeTest()
		{
			// Arrange
			Note note1 = new Note("Some topic here", "Some text here, yes.");
			System.Threading.Thread.Sleep(1100);
			Note note2 = new Note("Another topic here", "Not so similar text");

			// Act
			DateTimeOffset noteCreationTime1 = note1.GetCreationTime();
			DateTimeOffset noteCreationTime2 = note2.GetCreationTime();

			// Assert
			Assert.IsTrue(noteCreationTime1 < noteCreationTime2, "Second creation time should happen later");
		}

		[Test]
		public void ModificationUpdateTimestampTest()
		{
			// Arrange
			Note note = new Note("Some topic here", "Some text here, yes.");

			// Act
			var modificationTime1 = note.GetModificationTime();
			System.Threading.Thread.Sleep(1100);
			note.UpdateNote(note.GetNoteTitle(), "Some text here, yes. part 2");
			var modificationTime2 = note.GetModificationTime();

			// Assert
			Assert.AreNotEqual(modificationTime1, modificationTime2);
		}
		
		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = new Note();
			Note note2 = new Note("Some topic here", "Some text here, yes.", dto);
			Note note3 = new Note("Some topic here", "Some text here, yes.", dto);

			// Act
			string checksum1 = note1.GetChecksumAsHex();
			string checksum2 = note2.GetChecksumAsHex();
			string checksum3 = note3.GetChecksumAsHex();

			string newContent = note3.GetNoteText() + "A";
			note3.UpdateNote(note3.GetNoteTitle(), newContent);
			string checksum4 = note3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ShallowCopyTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = new Note("Some topic here", "Some text here, yes.", dto);

			// Act
			Note note2 = note1.ShallowCopy();

			string checksum1 = note1.GetChecksumAsHex();
			string checksum2 = note2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(note2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			Note note1 = new Note("Some topic here", "Some text here, yes. And read more!", dto);

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
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			Note note1 = new Note("Some topic here", "Some text here, yes.");

			// Act
			string checksum1 = note1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(note1, Formatting.Indented);

			Note note2 = JsonConvert.DeserializeObject<Note>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, note2.GetChecksumAsHex());
		}

		[Test]
		public void CheckIfChecksumMatchesContentTest()
		{
			// Arrange
			Note note1 = new Note("Some topic here", "Some text here, yes.");

			// Act
			bool shouldBeTrue = note1.CheckIfChecksumMatchesContent();
			note1.checksum = note1.checksum.Remove(0, 1);
			bool shouldBeFalse = note1.CheckIfChecksumMatchesContent();

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM