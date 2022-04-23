#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// FileEntry stores one plaintext (anyone can read) file
	/// </summary>
	public sealed partial class FileEntry
	{
		/// <summary>
		/// Create file entry, async
		/// </summary>
		/// <param name="newFilename">Filename</param>
		/// <param name="newFileContent">File content</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntry</returns>
		public static async Task<FileEntry> CreateFileEntryAsync(string newFilename, byte[] newFileContent, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateFileEntryAsync(newFilename, newFileContent, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Create file entry, async
		/// </summary>
		/// <param name="newFilename">Filename</param>
		/// <param name="newFileContent">File content</param>
		/// <param name="time">Creation time</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntry</returns>
		public static async Task<FileEntry> CreateFileEntryAsync(string newFilename, byte[] newFileContent, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			FileEntry fileEntry = new FileEntry();
			fileEntry.creationTime = time.ToUnixTimeSeconds();
			await fileEntry.UpdateFileEntryAsync(newFilename, newFileContent, time, securityFunctions);
			return fileEntry;
		}

		/// <summary>
		/// Update file entry, uses DateTimeOffset.UtcNow for modification timestamp, async
		/// </summary>
		/// <param name="updatedFilename">Filename</param>
		/// <param name="updatedFileContent">File content</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateFileEntryAsync(string updatedFilename, byte[] updatedFileContent, ISecurityAsyncFunctions securityFunctions)
		{
			await this.UpdateFileEntryAsync(updatedFilename, updatedFileContent, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Update file entry, use chosen time for modification time, async
		/// </summary>
		/// <param name="updatedFilename">Filename</param>
		/// <param name="updatedFileContent">File content</param>
		/// <param name="time">Modification time</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateFileEntryAsync(string updatedFilename, byte[] updatedFileContent, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			this.filename = Encoding.UTF8.GetBytes(updatedFilename);
			this.fileContent = updatedFileContent;
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		#region Checksum

		/// <summary>
		/// Check if checksum matches content, async
		/// </summary>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await this.CalculateHexChecksumAsync(securityFunctions);
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.filename, this.fileContent, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime));
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}

		#endregion // Checksum
	}
}

#endif // ASYNC_WITH_CUSTOM