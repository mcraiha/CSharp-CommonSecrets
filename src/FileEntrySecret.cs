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

		public FileEntrySecret(Dictionary<string, object> fileEntryAsDictionary, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(fileEntryAsDictionary);

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		#region Common getters

		public string GetFilename(byte[] derivedPassword)
		{
			if (derivedPassword == null || derivedPassword.Length < 1)
			{
				// TODO: Throw here
			}

			// Try to decrypt the binary
			byte[] decryptedAUDALF = algorithm.EncryptBytes(this.audalfData, derivedPassword);

			if (!AUDALF_Deserialize.IsAUDALF(decryptedAUDALF))
			{
				// TODO: exit here
			}

			Dictionary<string, object> fileEntryAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF);
			return (string)fileEntryAsDictionary[FileEntry.filenameKey];
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