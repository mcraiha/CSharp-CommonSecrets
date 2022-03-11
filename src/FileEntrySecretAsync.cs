#if ASYNC_WITH_CUSTOM
// This file has async methods

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;
using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// FileEntrySecret stores one encrypted file
	/// </summary>
	public sealed partial class FileEntrySecret
	{
		#region Common getters

		/// <summary>
		/// Get FileEntry. Use this for situation where you want to convert secret -> non secret
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntry</returns>
		public async Task<FileEntry> GetFileEntryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dict = await this.GetFileEntryAsDictionaryAsync(derivedPassword, securityFunctions);

			/*FileEntry returnValue = new FileEntry((string)dict[FileEntry.filenameKey], (byte[])dict[FileEntry.fileContentKey]);

			returnValue.creationTime = ((DateTimeOffset)dict[FileEntry.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[FileEntry.modificationTimeKey]).ToUnixTimeSeconds();

			return returnValue;*/
			return new FileEntry();
		}

		/// <summary>
		/// Get filename
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Filename as string</returns>
		public async Task<string> GetFilenameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.filenameKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file content as byte array
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Content as byte array</returns>
		public async Task<byte[]> GetFileContentAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (byte[])await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.fileContentKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file content lenght in bytes
		/// </summary>
		/// <returns>Lenght in bytes</returns>
		public async Task<long> GetFileContentLengthInBytesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			byte[] fileContent = await GetFileContentAsync(derivedPassword, securityFunctions);
			return fileContent.LongLength;
		}

		/// <summary>
		/// Get file entry creation time
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>File entry creation time as DateTimeOffset</returns>
		public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.creationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file entry modification time
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>File entry modification time as DateTimeOffset</returns>
		public async Task<DateTimeOffset> GetModificationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.modificationTimeKey, deserializationSettings, securityFunctions);
		}

		private async Task<Dictionary<string, object>> GetFileEntryAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				throw passwordCheck.exception;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = await algorithm.DecryptBytesAsync(this.audalfData, derivedPassword, securityFunctions);

			var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

			if (!audalfCheck.valid)
			{
				throw audalfCheck.exception;
			}

			Dictionary<string, object> fileEntryAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return fileEntryAsDictionary;
		}

		#endregion // Common getters


		#region Common setters


		#endregion // Common setters


		#region Checksum

		/// <summary>
		/// Get checksum as hex
		/// </summary>
		/// <returns>Hex string</returns>
		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.keyIdentifier, this.audalfData, algorithm.GetSettingsAsBytes());
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}

		#endregion // Checksum
	}
}

#endif