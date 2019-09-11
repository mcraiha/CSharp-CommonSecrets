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


		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string creationTimeKey = nameof(creationTime);

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string modificationTimeKey = nameof(modificationTime);

		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public FileEntry()
		{
			
		}

		public FileEntry(string newFilename, byte[] newFileContent)
		{
			this.UpdateFileEntry(newFilename, newFileContent);
		}

		public void UpdateFileEntry(string updatedFilename, byte[] updatedFileContent)
		{
			this.filename = Encoding.UTF8.GetBytes(updatedFilename);
			this.fileContent = updatedFileContent;
			this.modificationTime = DateTimeOffset.UtcNow;
			this.CalculateAndUpdateChecksum();
		}

		public string GetFilename()
		{
			return System.Text.Encoding.UTF8.GetString(this.filename);
		}


		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.filename, this.fileContent, BitConverter.GetBytes(this.creationTime.ToUnixTimeSeconds()),
														BitConverter.GetBytes(this.modificationTime.ToUnixTimeSeconds()));
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}
}