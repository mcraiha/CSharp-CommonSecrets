using System;
using System.Text;

namespace CSCommonSecrets
{
	/// <summary>
	/// FileEntry stores one plaintext (anyone can read) file
	/// </summary>
	public sealed class FileEntry
	{
		/// <summary>
		/// Filename as byte array so that special characters won't cause issues. For normal use case use GetFilename()
		/// </summary>
		public byte[] filename { get; set; } = new byte[0];

		/// <summary>
		/// Key for storing filename data to AUDALF
		/// </summary>
		public static readonly string filenameKey = nameof(filename);

		/// <summary>
		/// File content as byte array
		/// </summary>
		public byte[] fileContent { get; set; } = new byte[0];

		/// <summary>
		/// Key for storing file content (actual bytes of file) to AUDALF
		/// </summary>
		public static readonly string fileContentKey = nameof(fileContent);

		/// <summary>
		/// Creation time of file entry, in Unix seconds since epoch
		/// </summary>
		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing file entry creation time to AUDALF
		/// </summary>
		public static readonly string creationTimeKey = nameof(creationTime);

		/// <summary>
		/// Last modification time of file entry, in Unix seconds since epoch
		/// </summary>
		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing file entry last modification time to AUDALF
		/// </summary>
		public static readonly string modificationTimeKey = nameof(modificationTime);

		/// <summary>
		/// Calculated checksum of File entry
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public FileEntry()
		{
			
		}

		/// <summary>
		/// Default constructor for file entry
		/// </summary>
		/// <param name="newFilename">Filename</param>
		/// <param name="newFileContent">File content</param>
		public FileEntry(string newFilename, byte[] newFileContent) : this (newFilename, newFileContent, DateTimeOffset.UtcNow)
		{

		}

		/// <summary>
		/// Constructor with creation time override
		/// </summary>
		/// <param name="newFilename">Filename</param>
		/// <param name="newFileContent">File content</param>
		/// <param name="time">Creation time</param>
		public FileEntry(string newFilename, byte[] newFileContent, DateTimeOffset time)
		{
			this.creationTime = time.ToUnixTimeSeconds();
			this.UpdateFileEntry(newFilename, newFileContent, time);
		}

		/// <summary>
		/// Update file entry, use current DateTimeOffset.UtcNow for modification timestamp
		/// </summary>
		/// <param name="updatedFilename">Filename</param>
		/// <param name="updatedFileContent">File content</param>
		public void UpdateFileEntry(string updatedFilename, byte[] updatedFileContent)
		{
			this.UpdateFileEntry(updatedFilename, updatedFileContent, DateTimeOffset.UtcNow);
		}

		/// <summary>
		/// Update file entry, use chosen time for modification time
		/// </summary>
		/// <param name="updatedFilename">Filename</param>
		/// <param name="updatedFileContent">File content</param>
		/// <param name="time">Modification time</param>
		public void UpdateFileEntry(string updatedFilename, byte[] updatedFileContent, DateTimeOffset time)
		{
			this.filename = Encoding.UTF8.GetBytes(updatedFilename);
			this.fileContent = updatedFileContent;
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Get filename
		/// </summary>
		/// <returns>Filename as string</returns>
		public string GetFilename()
		{
			return System.Text.Encoding.UTF8.GetString(this.filename);
		}

		/// <summary>
		/// Get file content
		/// </summary>
		/// <returns>File content as byte array</returns>
		public byte[] GetFileContent()
		{
			return fileContent;
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

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.filename, this.fileContent, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime));
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}
}