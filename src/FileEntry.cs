using System;
using System.Text;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	public sealed class FileEntry
	{
		public string filename { get; set; } = string.Empty;
		public byte[] fileContent { get; set; } = new byte[0];

		private string checksum = string.Empty;

		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;

		public FileEntry()
		{
			
		}

		public FileEntry(string newFilename, byte[] newFileContent)
		{
			this.UpdateFileEntry(newFilename, newFileContent);
		}

		public void UpdateFileEntry(string updatedFilename, byte[] updatedFileContent)
		{
			this.filename = updatedFilename;
			this.fileContent = updatedFileContent;
			this.modificationTime = DateTimeOffset.UtcNow;
			this.CalculateAndUpdateChecksum();
		}

		private string CalculateBase64Checksum()
		{
			using (SHA256 mySHA256 = SHA256.Create())
			{
				string forCalculatingHash = $"{this.filename}{Convert.ToBase64String(this.fileContent)}{this.creationTime.ToUnixTimeSeconds()}{this.modificationTime.ToUnixTimeSeconds()}";
				byte[] checksumBytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(forCalculatingHash));
				return Convert.ToBase64String(checksumBytes);
			}
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateBase64Checksum();
		}
	}
}