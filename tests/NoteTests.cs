using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;

namespace Tests
{
	public class NoteTests
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
			Assert.AreEqual(title, note.noteTitle);
			CollectionAssert.AreEqual(text, note.noteText);
		}

		[Test]
		public void ModificationUpdateTimestampTest()
		{
			// Arrange
			Note note = new Note("Some topic here", "Some text here, yes.");

			// Act
			var modificationTime1 = note.modificationTime;
			note.UpdateNote(note.noteTitle, "Some text here, yes. part 2");
			var modificationTime2 = note.modificationTime;

			// Assert
			Assert.AreNotEqual(modificationTime1, modificationTime2);
		}
		
		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			Note note1 = new Note();
			Note note2 = new Note("Some topic here", "Some text here, yes.");
			Note note3 = new Note("Some topic here", "Some text here, yes.");

			// Act
			string checksum1 = note1.GetChecksumAsHex();
			string checksum2 = note2.GetChecksumAsHex();
			string checksum3 = note3.GetChecksumAsHex();

			string newContent = note3.noteText + "A";
			note3.UpdateNote(note3.noteTitle, newContent);
			string checksum4 = note3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}
	}
}