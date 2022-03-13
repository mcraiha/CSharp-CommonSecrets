#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Note stores one plaintext (anyone can read) note. Basically a text file
	/// </summary>
	public sealed partial class Note
	{
		/// <summary>
		/// Create note, async
		/// </summary>
		/// <param name="newNoteTitle">Note title</param>
		/// <param name="newNoteText">Note text</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note</returns>
		public static async Task<Note> CreateNoteAsync(string newNoteTitle, string newNoteText, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateNoteAsync(newNoteTitle, newNoteText, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Create note, async
		/// </summary>
		/// <param name="newNoteTitle">Note title</param>
		/// <param name="newNoteText">Note text</param>
		/// <param name="time">Creation time</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Note</returns>
		public static async Task<Note> CreateNoteAsync(string newNoteTitle, string newNoteText, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			Note note = new Note();
			note.creationTime = time.ToUnixTimeSeconds();
			await note.UpdateNoteAsync(newNoteTitle, newNoteText, time, securityFunctions);
			return note;
		}
		/*
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
		*/
		/// <summary>
		/// Update note, uses DateTimeOffset.UtcNow for modification timestamp, async
		/// </summary>
		/// <param name="updatedNoteTitle">New title</param>
		/// <param name="updatedNoteText">New text</param>
		public async Task UpdateNoteAsync(string updatedNoteTitle, string updatedNoteText, ISecurityAsyncFunctions securityFunctions)
		{
			await this.UpdateNoteAsync(updatedNoteTitle, updatedNoteText, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Update note, use chosen time for modification time
		/// </summary>
		/// <param name="updatedNoteTitle">New title</param>
		/// <param name="updatedNoteText">New text</param>
		/// <param name="modificationTime">Modification time</param>
		public async Task UpdateNoteAsync(string updatedNoteTitle, string updatedNoteText, DateTimeOffset modificationTime, ISecurityAsyncFunctions securityFunctions)
		{
			this.noteTitle = Encoding.UTF8.GetBytes(updatedNoteTitle);
			this.noteText = Encoding.UTF8.GetBytes(updatedNoteText);
			this.modificationTime = modificationTime.ToUnixTimeSeconds();
			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Check if checksum matches content
		/// </summary>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await CalculateHexChecksumAsync(securityFunctions);
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.noteTitle, this.noteText, BitConverter.GetBytes(this.creationTime),
														BitConverter.GetBytes(this.modificationTime));
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}
	}

}

#endif // ASYNC_WITH_CUSTOM