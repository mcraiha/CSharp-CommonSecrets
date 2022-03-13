using System;
using System.Text;

namespace CSCommonSecrets
{
	/// <summary>
	/// Note stores one plaintext (anyone can read) note. Basically a text file
	/// </summary>
	public sealed partial class Note
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

			this.checksum = copyThis.checksum;
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
	}

}