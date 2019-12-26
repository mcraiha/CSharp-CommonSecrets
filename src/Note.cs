using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class Note
	{
		/// <summary>
		/// Note title as byte array
		/// </summary>
		public byte[] noteTitle { get; set; } = new byte[0];

		/// <summary>
		/// Note title key
		/// </summary>
		public static readonly string noteTitleKey = nameof(noteTitle);

		/// <summary>
		/// Note text as byte array
		/// </summary>
		public byte[] noteText { get; set; } = new byte[0];

		/// <summary>
		/// Note text key
		/// </summary>
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

		/// <summary>
		/// Default constructor for note
		/// </summary>
		/// <param name="newNoteTitle">Note title</param>
		/// <param name="newNoteText">Note text</param>
		public Note(string newNoteTitle, string newNoteText) : this (newNoteTitle, newNoteText, DateTimeOffset.UtcNow)
		{
			
		}

		public Note(string newNoteTitle, string newNoteText, DateTimeOffset time)
		{
			this.creationTime = time.ToUnixTimeSeconds();
			this.UpdateNote(newNoteTitle, newNoteText, time);
		}

		/// <summary>
		/// Update note
		/// </summary>
		/// <param name="updatedNoteTitle">New title</param>
		/// <param name="updatedNoteText">New text</param>
		public void UpdateNote(string updatedNoteTitle, string updatedNoteText)
		{
			this.UpdateNote(updatedNoteTitle, updatedNoteText, DateTimeOffset.UtcNow);
		}

		public void UpdateNote(string updatedNoteTitle, string updatedNoteText, DateTimeOffset time)
		{
			this.noteTitle = Encoding.UTF8.GetBytes(updatedNoteTitle);
			this.noteText = Encoding.UTF8.GetBytes(updatedNoteText);
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Get note title
		/// </summary>
		/// <returns>Title as string</returns>
		public string GetNoteTitle()
		{
			return System.Text.Encoding.UTF8.GetString(this.noteTitle);
		}

		/// <summary>
		/// Get note text
		/// </summary>
		/// <returns>Text as string</returns>
		public string GetNoteText()
		{
			return System.Text.Encoding.UTF8.GetString(this.noteText);
		}

		/// <summary>
		/// Get creation time
		/// </summary>
		/// <returns>Creation time as DateTimeOffset</returns>
		public DateTimeOffset GetCreationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.creationTime);
		}

		/// <summary>
		/// Get modification time
		/// </summary>
		/// <returns>Modification time as DateTimeOffset</returns>
		public DateTimeOffset GetModificationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.modificationTime);
		}

		/// <summary>
		/// Get checksum as hex
		/// </summary>
		/// <returns>Hex string</returns>
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