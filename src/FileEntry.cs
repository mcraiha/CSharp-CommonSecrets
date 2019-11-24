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

		public FileEntry(string newFilename, byte[] newFileContent) : this (newFilename, newFileContent, DateTimeOffset.UtcNow)
		{

		}

		public FileEntry(string newFilename, byte[] newFileContent, DateTimeOffset time)
		{
			this.creationTime = time.ToUnixTimeSeconds();
			this.UpdateFileEntry(newFilename, newFileContent, time);
		}

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

		public string GetFilename()
		{
			return System.Text.Encoding.UTF8.GetString(this.filename);
		}

		public byte[] GetFileContent()
		{
			return fileContent;
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