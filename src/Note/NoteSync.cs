#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// Note stores one plaintext (anyone can read) note. Basically a text file
/// </summary>
public sealed partial class Note
{
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
	/// Update note, uses DateTimeOffset.UtcNow for modification timestamp
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
	/// <param name="modificationTime">Modification time</param>
	public void UpdateNote(string updatedNoteTitle, string updatedNoteText, DateTimeOffset modificationTime)
	{
		this.noteTitle = Encoding.UTF8.GetBytes(updatedNoteTitle);
		this.noteText = Encoding.UTF8.GetBytes(updatedNoteText);
		this.modificationTime = modificationTime.ToUnixTimeSeconds();
		this.CalculateAndUpdateChecksum();
	}

	#region Checksum

	/// <summary>
	/// Check if checksum matches content
	/// </summary>
	/// <returns>True if matches; False otherwise</returns>
	public bool CheckIfChecksumMatchesContent()
	{
		return checksum == this.CalculateHexChecksum();
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

	#endregion // Checksum
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM