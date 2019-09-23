using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class LoginInformationSecret
	{
		public byte[] keyIdentifier { get; set; }

		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public LoginInformationSecret()
		{

		}

		public LoginInformationSecret(LoginInformation loginInformation, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ LoginInformation.titleKey, loginInformation.GetTitle() },
				{ LoginInformation.urlKey, loginInformation.GetURL() },
				{ LoginInformation.usernameKey, loginInformation.GetUsername() },
				{ LoginInformation.passwordKey, loginInformation.GetPassword() },
				{ LoginInformation.notesKey, loginInformation.GetNotes() },
				{ LoginInformation.creationTimeKey, loginInformation.creationTime },
				{ LoginInformation.modificationTimeKey, loginInformation.modificationTime },
				{ LoginInformation.iconKey, loginInformation.GetIcon() },
				{ LoginInformation.categoryKey, loginInformation.GetCategory() },
				{ LoginInformation.tagsKey, loginInformation.GetTags() },
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

		public LoginInformationSecret(Dictionary<string, object> loginInformationAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
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

		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
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
			return ChecksumHelper.CalculateHexChecksum(this.keyIdentifier, this.audalfData, this.algorithm.GetSettingsAsBytes());
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum
	}

}