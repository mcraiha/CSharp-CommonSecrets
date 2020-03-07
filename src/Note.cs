using System;
using System.Text;

namespace CSCommonSecrets
{
	/// <summary>
	/// Note stores one plaintext (anyone can read) note. Basically a text file
	/// </summary>
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

		/// <summary>
		/// Creation time of note, in Unix seconds since epoch
		/// </summary>
		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing note creation time to AUDALF
		/// </summary>
		public static readonly string creationTimeKey = nameof(creationTime);

		/// <summary>
		/// Last modification time of note, in Unix seconds since epoch
		/// </summary>
		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing note last modification time to AUDALF
		/// </summary>
		public static readonly string modificationTimeKey = nameof(modificationTime);

		/// <summary>
		/// Calculated checksum of note
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public Note()
		{
			
		}

		/// <summary>
		/// Deep copy existing Note to new Note
		/// </summary>
		/// <param name="copyThis">Note to copy</param>
		public Note(Note copyThis)
		{
			this.noteTitle = new byte[copyThis.noteTitle.Length];
			Buffer.BlockCopy(copyThis.noteTitle, 0, this.noteTitle, 0, copyThis.noteTitle.Length);

			this.noteText = new byte[copyThis.noteText.Length];
			Buffer.BlockCopy(copyThis.noteText, 0, this.noteText, 0, copyThis.noteText.Length);

			this.creationTime = copyThis.creationTime;
			this.modificationTime = copyThis.modificationTime;
		}

		/// <summary>
		/// Default constructor for note
		/// </summary>
		/// <param name="newNoteTitle">Note title</param>
		/// <param name="newNoteText">Note text</param>
		public Note(string newNoteTitle, string newNoteText) : this (newNoteTitle, newNoteText, DateTimeOffset.UtcNow)
		{
			
		}

		/// <summary>
		/// Constructor with creation time override
		/// </summary>
		/// <param name="newNoteTitle">Note title</param>
		/// <param name="newNoteText">Note text</param>
		/// <param name="time">Creation time</param>
		public Note(string newNoteTitle, string newNoteText, DateTimeOffset time)
		{
			this.creationTime = time.ToUnixTimeSeconds();
			this.UpdateNote(newNoteTitle, newNoteText, time);
		}

		/// <summary>
		/// Create shallow copy, mostly for testing purposes
		/// </summary>
		/// <returns>Shallow copy of Note</returns>
		public Note ShallowCopy()
		{
			return (Note) this.MemberwiseClone();
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

		/// <summary>
		/// Update note, use chosen time for modification time
		/// </summary>
		/// <param name="updatedNoteTitle">New title</param>
		/// <param name="updatedNoteText">New text</param>
		/// <param name="time">Modification time</param>
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

		/// <summary>
		/// Check if checksum matches content
		/// </summary>
		/// <returns>True if matches; False otherwise</returns>
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