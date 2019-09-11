using System;
using System.Collections.Generic;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class FileEntrySecret
	{
		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public FileEntrySecret()
		{

		}

		public FileEntrySecret(FileEntry fileEntry, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ FileEntry.filenameKey, fileEntry.filename },
				{ FileEntry.fileContentKey, fileEntry.fileContent },
				{ FileEntry.creationTimeKey, fileEntry.creationTime },
				{ FileEntry.modificationTimeKey, fileEntry.modificationTime },
			};

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		public FileEntrySecret(Dictionary<string, object> fileEntryAsDictionary, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		#region Common getters

		public byte[] GetFilenameUTF8Bytes(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (byte[])fileEntryAsDictionary[FileEntry.filenameKey];
		}

		public byte[] GetFileContent(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (byte[])fileEntryAsDictionary[FileEntry.fileContentKey];
		}

		private Dictionary<string, object> GetFileEntryAsDictionary(byte[] derivedPassword)
		{
			var passwordCheck = Helpers.CheckDerivedPassword(derivedPassword);

			if (!passwordCheck.valid)
			{
				throw passwordCheck.exception;
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = algorithm.EncryptBytes(this.audalfData, derivedPassword);

			var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

			if (!audalfCheck.valid)
			{
				throw audalfCheck.exception;
			}

			Dictionary<string, object> fileEntryAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF);

			return fileEntryAsDictionary;
		}

		#endregion // Common getters


		#region Checksum

		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.audalfData, algorithm.GetSettingsAsBytes());
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum
	}
}