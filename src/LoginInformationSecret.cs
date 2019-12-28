using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	public sealed class LoginInformationSecret
	{
		/// <summary>
		/// Key identifier bytes (this is plaintext information), in normal case it is better to use GetKeyIdentifier()
		/// </summary>
		public byte[] keyIdentifier { get; set; }

		/// <summary>
		/// AUDALF data as byte array (this is secret/ecrypted information)
		/// </summary>
		public byte[] audalfData { get; set; } = new byte[0];

		/// <summary>
		/// Symmetric Key Algorithm for this LoginInformationSecret (this is plaintext information)
		/// </summary>
		public SymmetricKeyAlgorithm algorithm { get; set; }

		/// <summary>
		/// Checksum of the data (this is plaintext information)
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public LoginInformationSecret()
		{

		}

		/// <summary>
		/// Default constructor for LoginInformationSecret
		/// </summary>
		/// <param name="loginInformation">LoginInformation that will be stored as secret</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">SymmetricKeyAlgorithm used to encrypt the LoginInformation</param>
		/// <param name="derivedPassword">Derived password</param>
		public LoginInformationSecret(LoginInformation loginInformation, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ LoginInformation.titleKey, loginInformation.GetTitle() },
				{ LoginInformation.urlKey, loginInformation.GetURL() },
				{ LoginInformation.emailKey, loginInformation.GetEmail() },
				{ LoginInformation.usernameKey, loginInformation.GetUsername() },
				{ LoginInformation.passwordKey, loginInformation.GetPassword() },
				{ LoginInformation.notesKey, loginInformation.GetNotes() },
				{ LoginInformation.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(loginInformation.creationTime) },
				{ LoginInformation.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(loginInformation.modificationTime) },
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

		public string GetURL(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.urlKey];
		}

		public string GetEmail(byte[] derivedPassword)
		{
			Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
			return (string)loginInformationAsDictionary[LoginInformation.emailKey];
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
			byte[] decryptedAUDALF = algorithm.EncryptBytes(this.audalfData, derivedPassword);

			var audalfCheck = Helpers.CheckAUDALFbytes(decryptedAUDALF);

			if (!audalfCheck.valid)
			{
				return false;
			}

			return true;
		}

		#endregion // Common getters

		#region Common setters

		public bool SetTitle(string newTitle, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.titleKey, newTitle, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetURL(string newURL, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.urlKey, newURL, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetEmail(string newEmail, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.emailKey, newEmail, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetUsername(string newUsername, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.usernameKey, newUsername, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetPassword(string newPassword, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.passwordKey, newPassword, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetNotes(string newNotes, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetCreationTime(DateTimeOffset newCreationTime, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.creationTimeKey, newCreationTime,  DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetModificationTime(DateTimeOffset newModificationTime, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.modificationTimeKey, newModificationTime,  DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetIcon(byte[] newIcon, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.iconKey, newIcon, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetCategory(string newCategory, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.categoryKey, newCategory, DateTimeOffset.UtcNow, derivedPassword);
		}

		public bool SetTags(string newTags, byte[] derivedPassword)
		{
			return this.GenericSet(LoginInformation.tagsKey, newTags, DateTimeOffset.UtcNow, derivedPassword);
		}

		private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
		{
			try 
			{
				Dictionary<string, object> loginInformationAsDictionary = this.GetLoginInformationAsDictionary(derivedPassword);
				loginInformationAsDictionary[key] = value;

				if (key != LoginInformation.creationTimeKey && key != LoginInformation.modificationTimeKey)
				{
					loginInformationAsDictionary[LoginInformation.modificationTimeKey] = modificationTime;
				}

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(loginInformationAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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
			return ChecksumHelper.CalculateHexChecksum(this.keyIdentifier, this.audalfData, this.algorithm.GetSettingsAsBytes());
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum
	}

}