using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class FileEntrySecret
	{
		public byte[] keyIdentifier { get; set; }

		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public FileEntrySecret()
		{

		}

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
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		public FileEntrySecret(Dictionary<string, object> fileEntryAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		#region Common getters

		public string GetFilename(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (string)fileEntryAsDictionary[FileEntry.filenameKey];
		}

		public byte[] GetFileContent(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (byte[])fileEntryAsDictionary[FileEntry.fileContentKey];
		}

		public DateTimeOffset GetCreationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (DateTimeOffset)fileEntryAsDictionary[FileEntry.creationTimeKey];
		}

		public DateTimeOffset GetModificationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> fileEntryAsDictionary = this.GetFileEntryAsDictionary(derivedPassword);
			return (DateTimeOffset)fileEntryAsDictionary[FileEntry.modificationTimeKey];
		}

		private static readonly DeserializationSettings deserializationSettings = new DeserializationSettings()
		{
			wantedDateTimeType = typeof(DateTimeOffset)
		};

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

			Dictionary<string, object> fileEntryAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return fileEntryAsDictionary;
		}

		#endregion // Common getters


		#region Common setters

		public bool SetFilename(string newFilename, byte[] derivedPassword)
		{
			return this.GenericSet(FileEntry.filenameKey, newFilename, DateTimeOffset.UtcNow, derivedPassword);
		}

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
				byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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

		public string GetChecksumAsHex()
		{
			return this.checksum;
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
}