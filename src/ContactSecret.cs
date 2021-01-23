using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

namespace CSCommonSecrets
{
	/// <summary>
	/// ContactSecret stores one encrypted contact
	/// </summary>
	public sealed class ContactSecret
	{
		/// <summary>
		/// Key identifier bytes (this is plaintext information), in normal case it is better to use GetKeyIdentifier()
		/// </summary>
		public byte[] keyIdentifier { get; set; }

		/// <summary>
		/// AUDALF data as byte array (this is secret/encrypted information)
		/// </summary>
		public byte[] audalfData { get; set; } = new byte[0];

		/// <summary>
		/// Symmetric Key Algorithm for this ContactSecret (this is plaintext information)
		/// </summary>
		public SymmetricKeyAlgorithm algorithm { get; set; }

		/// <summary>
		/// Checksum of the data (this is plaintext information)
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public ContactSecret()
		{

		}

		/// <summary>
		/// Default constructor for ContactSecret
		/// </summary>
		/// <param name="contact">Contact to encrypt</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		public ContactSecret(Contact contact, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			Dictionary<string, object> dictionaryForAUDALF = new Dictionary<string, object>()
			{
				{ Contact.firstNameKey, contact.GetFirstName() },
				{ Contact.lastNameKey, contact.GetLastName() },
				{ Contact.middleNameKey, contact.GetMiddleName() },
				{ Contact.namePrefixKey, contact.GetNamePrefix() },
				{ Contact.nameSuffixKey, contact.GetNameSuffix() },
				{ Contact.nicknameKey, contact.GetNickname() },
				{ Contact.companyKey, contact.GetCompany() },
				{ Contact.jobTitleKey, contact.GetJobTitle() },
				{ Contact.departmentKey, contact.GetDepartment() },
				{ Contact.emailsKey, contact.GetEmails() },
				{ Contact.emailDescriptionsKey, contact.GetEmailDescriptions() },
				{ Contact.phoneNumbersKey, contact.GetPhoneNumbers() },
				{ Contact.phoneNumberDescriptionsKey, contact.GetPhoneNumberDescriptions() },
				{ Contact.countryKey, contact.GetCountry() },
				{ Contact.streetAddressKey, contact.GetStreetAddress() },
				{ Contact.streetAddressAdditionalKey, contact.GetStreetAddressAdditional() },
				{ Contact.postalCodeKey, contact.GetPostalCode() },
				{ Contact.cityKey, contact.GetCity() },
				{ Contact.poBoxKey, contact.GetPOBox() },
				{ Contact.birthdayKey, contact.GetBirthday() },
				{ Contact.relationshipKey, contact.GetRelationship() },
				{ Contact.notesKey, contact.GetNotes() },
				{ Contact.websitesKey, contact.GetWebsites() },
				{ Contact.creationTimeKey, DateTimeOffset.FromUnixTimeSeconds(contact.creationTime) },
				{ Contact.modificationTimeKey, DateTimeOffset.FromUnixTimeSeconds(contact.modificationTime) },
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

		/// <summary>
		/// Deep copy existing ContactSecret
		/// </summary>
		/// <param name="copyThis">Deep copy this</param>
		public ContactSecret(ContactSecret copyThis)
		{
			this.keyIdentifier = new byte[copyThis.keyIdentifier.Length];
			Buffer.BlockCopy(copyThis.keyIdentifier, 0, this.keyIdentifier, 0, copyThis.keyIdentifier.Length);

			this.audalfData = new byte[copyThis.audalfData.Length];
			Buffer.BlockCopy(copyThis.audalfData, 0, this.audalfData, 0, copyThis.audalfData.Length);

			this.algorithm = new SymmetricKeyAlgorithm(copyThis.algorithm);
			this.checksum = copyThis.checksum;
		}

		private static readonly SerializationSettings serializationSettings = new SerializationSettings() { dateTimeFormat = DateTimeFormat.UnixInSeconds };

		/// <summary>
		/// Constructor for custom dictionary, use this only if you know what you are doing
		/// </summary>
		/// <param name="contactAsDictionary">Dictionary containing contact keys and values</param>
		/// <param name="keyIdentifier">Key identifier</param>
		/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
		/// <param name="derivedPassword">Derived password</param>
		public ContactSecret(Dictionary<string, object> contactAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
		{
			this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

			this.algorithm = algorithm;

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

			// Encrypt the AUDALF payload with given algorithm
			this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		#region Common getters

		/// <summary>
		/// Get Contact. Use this for situation where you want to convert secret -> non secret
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact</returns>
		public Contact GetContact(byte[] derivedPassword)
		{
			Dictionary<string, object> dict = this.GetContactAsDictionary(derivedPassword);
			string firstName = (string)dict[Contact.firstNameKey];
			string lastName = (string)dict[Contact.lastNameKey];
			string middleName = (string)dict[Contact.middleNameKey];
			string namePrefix = (string)dict[Contact.namePrefixKey];
			string nameSuffix = (string)dict[Contact.nameSuffixKey];
			string nickname = (string)dict[Contact.nicknameKey];
			string company = (string)dict[Contact.companyKey];
			string jobTitle = (string)dict[Contact.jobTitleKey];
			string department = (string)dict[Contact.departmentKey];
			string[] emails = ((string)dict[Contact.emailsKey]).Split(Contact.separatorChar);
			string[] emailDescriptions = ((string)dict[Contact.emailDescriptionsKey]).Split(Contact.separatorChar);
			string[] phoneNumbers = ((string)dict[Contact.phoneNumbersKey]).Split(Contact.separatorChar);
			string[] phoneNumberDescriptions = ((string)dict[Contact.phoneNumberDescriptionsKey]).Split(Contact.separatorChar);
			string country = (string)dict[Contact.countryKey];
			string streetAddress = (string)dict[Contact.streetAddressKey];
			string streetAddressAdditional = (string)dict[Contact.streetAddressAdditionalKey];
			string postalCode = (string)dict[Contact.postalCodeKey];
			string city = (string)dict[Contact.cityKey];
			string poBox = (string)dict[Contact.poBoxKey];
			string birthday = (string)dict[Contact.birthdayKey];
			string relationship = (string)dict[Contact.relationshipKey];
			string notes = (string)dict[Contact.notesKey];
			string[] websites = ((string)dict[Contact.websitesKey]).Split(Contact.separatorChar);
			Contact returnValue = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);

			returnValue.creationTime = ((DateTimeOffset)dict[Contact.creationTimeKey]).ToUnixTimeSeconds();
			returnValue.modificationTime = ((DateTimeOffset)dict[Contact.modificationTimeKey]).ToUnixTimeSeconds();

			return returnValue;
		}

		/// <summary>
		/// Get contact first name
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact first name as string</returns>
		public string GetFirstName(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.firstNameKey];
		}

		/// <summary>
		/// Get contact last name
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact last name as string</returns>
		public string GetLastName(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.lastNameKey];
		}

		/// <summary>
		/// Get contact middle name
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact middle name as string</returns>
		public string GetMiddleName(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.middleNameKey];
		}

		/// <summary>
		/// Get contact name prefix
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact name prefix as string</returns>
		public string GetNamePrefix(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.namePrefixKey];
		}

		/// <summary>
		/// Get contact name suffix
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact name suffix as string</returns>
		public string GetNameSuffix(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.nameSuffixKey];
		}

		/// <summary>
		/// Get contact nickname
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact nickname as string</returns>
		public string GetNickname(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.nicknameKey];
		}

		/// <summary>
		/// Get contact company
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact company as string</returns>
		public string GetCompany(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.companyKey];
		}

		/// <summary>
		/// Get contact job title
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact job title as string</returns>
		public string GetJobTitle(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.jobTitleKey];
		}

		/// <summary>
		/// Get contact department
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact department as string</returns>
		public string GetDepartment(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.departmentKey];
		}

		/// <summary>
		/// Get email addresses
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact email addresses as string array</returns>
		public string[] GetEmails(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return ((string)contactAsDictionary[Contact.emailsKey]).Split(Contact.separatorChar);
		}

		/// <summary>
		/// Get email address descriptions
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact email address descriptions as string array</returns>
		public string[] GetEmailDescriptions(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return ((string)contactAsDictionary[Contact.emailDescriptionsKey]).Split(Contact.separatorChar);
		}

		/// <summary>
		/// Get phone numbers
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact phone numbers as string array</returns>
		public string[] GetPhoneNumbers(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return ((string)contactAsDictionary[Contact.phoneNumbersKey]).Split(Contact.separatorChar);
		}

		/// <summary>
		/// Get phone numbers descriptions
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact phone numbers descriptions as string array</returns>
		public string[] GetPhoneNumberDescriptions(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return ((string)contactAsDictionary[Contact.phoneNumberDescriptionsKey]).Split(Contact.separatorChar);
		}

		/// <summary>
		/// Get country
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact country as string</returns>
		public string GetCountry(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.countryKey];
		}

		/// <summary>
		/// Get street address
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact street address as string</returns>
		public string GetStreetAddress(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.streetAddressKey];
		}

		/// <summary>
		/// Get street address additional 
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact street address additional  as string</returns>
		public string GetStreetAddressAdditional(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.streetAddressAdditionalKey];
		}

		/// <summary>
		/// Get postal code
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact postal code as string</returns>
		public string GetPostalCode(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.postalCodeKey];
		}

		/// <summary>
		/// Get city
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact city as string</returns>
		public string GetCity(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.cityKey];
		}

		/// <summary>
		/// Get PO Box
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact PO Box as string</returns>
		public string GetPOBox(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.poBoxKey];
		}

		/// <summary>
		/// Get birthday
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact birthday as string</returns>
		public string GetBirthday(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.birthdayKey];
		}

		/// <summary>
		/// Get websites
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact websites as string array</returns>
		public string[] GetWebsites(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return ((string)contactAsDictionary[Contact.websitesKey]).Split(Contact.separatorChar);
		}

		/// <summary>
		/// Get relationship 
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact relationship  as string</returns>
		public string GetRelationship(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.relationshipKey];
		}

		/// <summary>
		/// Get notes 
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact notes  as string</returns>
		public string GetNotes(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (string)contactAsDictionary[Contact.notesKey];
		}

		/// <summary>
		/// Get contact creation time
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact creation time as DateTimeOffset</returns>
		public DateTimeOffset GetCreationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (DateTimeOffset)contactAsDictionary[Contact.creationTimeKey];
		}

		/// <summary>
		/// Get contact modification time
		/// </summary>
		/// <param name="derivedPassword">Derived password</param>
		/// <returns>Contact modification time as DateTimeOffset</returns>
		public DateTimeOffset GetModificationTime(byte[] derivedPassword)
		{
			Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
			return (DateTimeOffset)contactAsDictionary[Contact.modificationTimeKey];
		}

		#endregion // Common getters


		#region Common setters

		private bool GenericSet(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword)
		{
			try 
			{
				Dictionary<string, object> contactAsDictionary = this.GetContactAsDictionary(derivedPassword);
				// Update wanted value
				contactAsDictionary[key] = value;
				// Update modification time
				contactAsDictionary[Contact.modificationTimeKey] = modificationTime;

				// Generate new algorithm since data has changed
				this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm());

				// Create AUDALF payload from dictionary
				byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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

		private static readonly DeserializationSettings deserializationSettings = new DeserializationSettings()
		{
			wantedDateTimeType = typeof(DateTimeOffset)
		};

		private Dictionary<string, object> GetContactAsDictionary(byte[] derivedPassword)
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

			Dictionary<string, object> contactAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

			return contactAsDictionary;
		}

		/// <summary>
		/// Get key identifier
		/// </summary>
		/// <returns>Key identifier as string</returns>
		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
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