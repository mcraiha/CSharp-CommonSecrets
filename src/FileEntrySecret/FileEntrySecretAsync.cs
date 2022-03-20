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
		/// <summary>
		/// Create FileEntrySecret, async
		/// </summary>
		/// <param name="fileEntry">File entry</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntrySecret</returns>
		public static async Task<FileEntrySecret> CreateFileEntrySecretAsync(FileEntry fileEntry, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ FileEntry.filenameKey, fileEntry.GetFilename() },
				{ FileEntry.fileContentKey, fileEntry.GetFileContent() },
				{ FileEntry.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(fileEntry.creationTime) },
				{ FileEntry.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(fileEntry.modificationTime) },
			};

			return await CreateFileEntrySecretAsync(dictionaryForAUDALF, keyIdentifier, algorithm, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Create FileEntrySecret, async
		/// </summary>
		/// <param name="fileEntryAsDictionary"></param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntrySecret</returns>
		public static async Task<FileEntrySecret> CreateFileEntrySecretAsync(Dictionary<string, object> fileEntryAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			FileEntrySecret fileEntrySecret = new FileEntrySecret();

			fileEntrySecret.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			fileEntrySecret.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			fileEntrySecret.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

			// Calculate new checksum
			await fileEntrySecret.CalculateAndUpdateChecksumAsync(securityFunctions);

			return fileEntrySecret;
		}

		#region Common getters

		/// <summary>
		/// Get FileEntry. Use this for situation where you want to convert secret -> non secret, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>FileEntry</returns>
		public async Task<FileEntry> GetFileEntryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Dictionary<string, object> dict = await this.GetFileEntryAsDictionaryAsync(derivedPassword, securityFunctions);

			FileEntry returnValue = await FileEntry.CreateFileEntryAsync((string)dict[FileEntry.filenameKey], (byte[])dict[FileEntry.fileContentKey], securityFunctions);

			returnValue.creationTime = ((DateTimeOffset)dict[FileEntry.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[FileEntry.modificationTimeKey]).ToUnixTimeSeconds();

			return returnValue;
		}

		/// <summary>
		/// Get filename, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Filename as string</returns>
		public async Task<string> GetFilenameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.filenameKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file content as byte array, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Content as byte array</returns>
		public async Task<byte[]> GetFileContentAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (byte[])await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.fileContentKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file content lenght in bytes, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Lenght in bytes</returns>
		public async Task<long> GetFileContentLengthInBytesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			byte[] fileContent = await GetFileContentAsync(derivedPassword, securityFunctions);
			return fileContent.LongLength;
		}

		/// <summary>
		/// Get file entry creation time, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>File entry creation time as DateTimeOffset</returns>
		public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, FileEntry.creationTimeKey, deserializationSettings, securityFunctions);
		}

		/// <summary>
		/// Get file entry modification time, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
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

		/// <summary>
		/// Can the content be decrypted with given derived password, async
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if can be; False otherwise</returns>
		public async Task<bool> CanBeDecryptedWithDerivedPasswordAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				return false;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = await algorithm.DecryptBytesAsync(this.audalfData, derivedPassword, securityFunctions);

			var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

			if (!audalfCheck.valid)
			{
				return false;
			}

			return true;
		}

		#endregion // Common getters


		#region Common setters

		/// <summary>
		/// Set filename, async
		/// </summary>
		/// <param name="newFilename">New filename</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set was success; False otherwise</returns>
		public async Task<bool> SetFilenameAsync(string newFilename, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(FileEntry.filenameKey, newFilename, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		/// <summary>
		/// Set file content, async
		/// </summary>
		/// <param name="newFileContent">New file content</param>
		/// <param name="derivedPassword">Derived password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if set was success; False otherwise</returns>
		public async Task<bool> SetFileContentAsync(byte[] newFileContent, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.GenericSetAsync(FileEntry.fileContentKey, newFileContent, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
		}

		private async Task<bool> GenericSetAsync(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
		{
			try 
			{
				Dictionary<string, object> fileEntryAsDictionary = await this.GetFileEntryAsDictionaryAsync(derivedPassword, securityFunctions);
				// Update wanted value
				fileEntryAsDictionary[key] = value;
				// Update modification time
				fileEntryAsDictionary[FileEntry.modificationTimeKey] = modificationTime;

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

				// Encrypt the AUDALF payload with given algorithm
				this.audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions);

				// Calculate new checksum
				await this.CalculateAndUpdateChecksumAsync(securityFunctions);

				return true;
			}
			catch
			{
				return false;
			}
		}

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