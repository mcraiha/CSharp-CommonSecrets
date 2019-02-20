using System;
using System.Collections.Generic;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class LoginInformationSecret
	{
		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public LoginInformationSecret()
		{

		}

		public LoginInformationSecret(LoginInformation loginInformation, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ LoginInformation.titleKey, loginInformation.title },
				{ LoginInformation.urlKey, loginInformation.url },
				{ LoginInformation.usernameKey, loginInformation.username },
				{ LoginInformation.passwordKey, loginInformation.password },
				{ LoginInformation.notesKey, loginInformation.notes },
				{ LoginInformation.creationTimeKey, loginInformation.creationTime },
				{ LoginInformation.modificationTimeKey, loginInformation.modificationTime },
				{ LoginInformation.iconKey, loginInformation.icon },
				{ LoginInformation.categoryKey, loginInformation.category },
				{ LoginInformation.tagsKey, loginInformation.tags },
			};

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		public LoginInformationSecret(Dictionary<string, object> loginInformationAsDictionary, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);
		}


		#region Common getters

		public string GetTitle(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.titleKey];
		}

		public string GetUrl(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.urlKey];
		}

		public string GetUsername(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.usernameKey];
		}

		public string GetPassword(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.passwordKey];
		}

		public string GetNotes(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.notesKey];
		}

		public DateTimeOffset GetCreationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (DateTimeOffset)loginInformationAsDictionary[LoginInformation.creationTimeKey];
		}

		public DateTimeOffset GetModificationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (DateTimeOffset)loginInformationAsDictionary[LoginInformation.modificationTimeKey];
		}

		public byte[] GetIcon(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (byte[])loginInformationAsDictionary[LoginInformation.iconKey];
		}

		public string GetCategory(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.categoryKey];
		}

		public string GetTags(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.tagsKey];
		}

		private static readonly DeserializationSettings deserializationSettings = new DeserializationSettings()
		{
			wantedDateTimeType = typeof(DateTimeOffset)
		};

		private Dictionary<string, object> GetLoginInformationAsDictionary(byte[] derivedPassword)
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

			Dictionary<string, object> loginInformationAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return loginInformationAsDictionary;
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