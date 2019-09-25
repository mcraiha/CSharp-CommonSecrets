using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class Note
	{
		public byte[] noteTitle { get; set; } = new byte[0];
		public static readonly string noteTitleKey = nameof(noteTitle);

		public byte[] noteText { get; set; } = new byte[0];
		public static readonly string noteTextKey = nameof(noteText);

		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string creationTimeKey = nameof(creationTime);

		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string modificationTimeKey = nameof(modificationTime);

		public string checksum { get; set; } = string.Empty;

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
			this.noteTitle = Encoding.UTF8.GetBytes(updatedNoteTitle);
			this.noteText = Encoding.UTF8.GetBytes(updatedNoteText);
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			this.CalculateAndUpdateChecksum();
		}

		public string GetNoteTitle()
		{
			return System.Text.Encoding.UTF8.GetString(this.noteTitle);
		}

		public string GetNoteText()
		{
			return System.Text.Encoding.UTF8.GetString(this.noteText);
		}

		public DateTimeOffset GetCreationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.creationTime);
		}

		public DateTimeOffset GetModificationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.modificationTime);
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
			return ChecksumHelper.CalculateHexChecksum(this.noteTitle, this.noteText, BitConverter.GetBytes(this.creationTime),
														BitConverter.GetBytes(this.modificationTime));
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}

}