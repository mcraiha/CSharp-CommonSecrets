using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class FileEntry
	{
		// Keep filename as byte array so that special characters won't cause issues
		public byte[] filename { get; set; } = new byte[0];
		public static readonly string filenameKey = nameof(filename);

		public byte[] fileContent { get; set; } = new byte[0];
		public static readonly string fileContentKey = nameof(fileContent);


		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string creationTimeKey = nameof(creationTime);

		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string modificationTimeKey = nameof(modificationTime);

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

		public FileEntry(string newFilename, byte[] newFileContent, DateTimeOffset time)
		{
			this.creationTime = time.ToUnixTimeSeconds();
			this.UpdateFileEntry(newFilename, newFileContent, time);
		}

		/// <summary>
		/// Update file entry
		/// </summary>
		/// <param name="updatedFilename">Filename</param>
		/// <param name="updatedFileContent">File content</param>
		public void UpdateFileEntry(string updatedFilename, byte[] updatedFileContent)
		{
			this.UpdateFileEntry(updatedFilename, updatedFileContent, DateTimeOffset.UtcNow);
		}

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