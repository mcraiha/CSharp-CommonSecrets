#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// FileEntrySecret stores one encrypted file
/// </summary>
public sealed partial class FileEntrySecret
{
	/// <summary>
	/// Default constructor for FileEntrySecret
	/// </summary>
	/// <param name="fileEntry">File entry to encrypt</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public FileEntrySecret(FileEntry fileEntry, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
		{
			{ FileEntry.filenameKey, fileEntry.GetFilename() },
			{ FileEntry.fileContentKey, fileEntry.GetFileContent() },
			{ FileEntry.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(fileEntry.creationTime) },
			{ FileEntry.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(fileEntry.modificationTime) },
		};

		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Constructor for custom dictionary, use this only if you know what you are doing
	/// </summary>
	/// <param name="fileEntryAsDictionary">Dictionary containing file entry keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public FileEntrySecret(Dictionary<string, object> fileEntryAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	#region Common getters

	/// <summary>
	/// Get FileEntry. Use this for situation where you want to convert secret -> non secret
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>FileEntry</returns>
	public FileEntry GetFileEntry(byte[] derivedPassword)
	{
		Dictionary<string, object> dict = this.GetFileEntryAsDictionary(derivedPassword);
		FileEntry returnValue = new FileEntry((string)dict[FileEntry.filenameKey], (byte[])dict[FileEntry.fileContentKey]);

		returnValue.creationTime = ((DateTimeOffset)dict[FileEntry.creationTimeKey]).ToUnixTimeSeconds();
		returnValue.modificationTime = ((DateTimeOffset)dict[FileEntry.modificationTimeKey]).ToUnixTimeSeconds();

		return returnValue;
	}

	/// <summary>
	/// Get filename
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Filename as string</returns>
	public string GetFilename(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, FileEntry.filenameKey, deserializationSettings);
	}

	/// <summary>
	/// Get file content as byte array
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Content as byte array</returns>
	public byte[] GetFileContent(byte[] derivedPassword)
	{
		return (byte[])Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, FileEntry.fileContentKey, deserializationSettings);
	}

	/// <summary>
	/// Get file content lenght in bytes
	/// </summary>
	/// <returns>Lenght in bytes</returns>
	public long GetFileContentLengthInBytes(byte[] derivedPassword)
	{
		return GetFileContent(derivedPassword).LongLength;
	}

	/// <summary>
	/// Get file entry creation time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>File entry creation time as DateTimeOffset</returns>
	public DateTimeOffset GetCreationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, FileEntry.creationTimeKey, deserializationSettings);
	}

	/// <summary>
	/// Get file entry modification time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>File entry modification time as DateTimeOffset</returns>
	public DateTimeOffset GetModificationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, FileEntry.modificationTimeKey, deserializationSettings);
	}

	private Dictionary<string, object> GetFileEntryAsDictionary(byte[] derivedPassword)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			throw passwordCheck.exception;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = algorithm.DecryptBytes(this.audalfData, derivedPassword);

		var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

		if (!audalfCheck.valid)
		{
			throw audalfCheck.exception;
		}

		Dictionary<string, object> fileEntryAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return fileEntryAsDictionary;
	}

	/// <summary>
	/// Can the content be decrypted with given derived password
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if can be; False otherwise</returns>
	public bool CanBeDecryptedWithDerivedPassword(byte[] derivedPassword)
	{
		var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

		if (!passwordCheck.valid)
		{
			return false;
		}

		// Try to decrypt the binary
		byte[] decryptedAUDALF = algorithm.DecryptBytes(this.audalfData, derivedPassword);

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
	/// Set filename
	/// </summary>
	/// <param name="newFilename">New filename</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetFilename(string newFilename, byte[] derivedPassword)
	{
		return this.GenericSet(FileEntry.filenameKey, newFilename, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Set file content
	/// </summary>
	/// <param name="newFileContent">New file content</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set was success; False otherwise</returns>
	public bool SetFileContent(byte[] newFileContent, byte[] derivedPassword)
	{
		return this.GenericSet(FileEntry.fileContentKey, newFileContent, DateTimeOffset.UtcNow, derivedPassword);
	}

	private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
	{
		try
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			// Update wanted value
			fileEntryAsDictionary[key] = value;
			// Update modification time
			fileEntryAsDictionary[FileEntry.modificationTimeKey] = modificationTime;

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();

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
	/// Check if checksum matches content
	/// </summary>
	/// <returns>True if matches; False otherwise</returns>
	public bool CheckIfChecksumMatchesContent()
	{
		return checksum == this.CalculateHexChecksum();
	}

	private string CalculateHexChecksum()
	{
		return ChecksumHelper.CalculateHexChecksum(this.keyIdentifier, this.audalfData, algorithm.GetSettingsAsBytes());
	}

	private void CalculateAndUpdateChecksum()
	{
		this.checksum = this.CalculateHexChecksum();
	}

	#endregion // Checksum
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM