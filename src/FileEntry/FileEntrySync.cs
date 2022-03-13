#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;

namespace CSCommonSecrets
{
	/// <summary>
	/// FileEntry stores one plaintext (anyone can read) file
	/// </summary>
	public sealed partial class FileEntry
	{
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
		/// Update file entry, uses DateTimeOffset.UtcNow for modification timestamp
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

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM