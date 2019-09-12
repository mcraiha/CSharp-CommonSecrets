using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class Note
	{
		public string noteTitle { get; set; } = string.Empty;
		public static readonly string noteTitleKey = nameof(noteTitle);

		public string noteText { get; set; } = string.Empty;
		public static readonly string noteTextKey = nameof(noteText);

		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string creationTimeKey = nameof(creationTime);

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string modificationTimeKey = nameof(modificationTime);

		public string checksum { get; private set; } = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public Note()
		{
			
		}

		public Note(string newNoteTitle, string newNoteText)
		{
			this.UpdateNote(newNoteTitle, newNoteText);
		}

		public void UpdateNote(string updatedNoteTitle, string updatedNoteText)
		{
			this.noteTitle = updatedNoteTitle;
			this.noteText = updatedNoteText;
			this.modificationTime = DateTimeOffset.UtcNow;
			this.CalculateAndUpdateChecksum();
		}

		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		public bool CheckIfChecksumMatchesContent()
		{
			return checksum == CalculateHexChecksum();
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(Encoding.UTF8.GetBytes(this.noteTitle), Encoding.UTF8.GetBytes(this.noteText), BitConverter.GetBytes(this.creationTime.ToUnixTimeSeconds()),
														BitConverter.GetBytes(this.modificationTime.ToUnixTimeSeconds()));
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}

}