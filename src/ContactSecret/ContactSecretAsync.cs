#if ASYNC_WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Threading.Tasks;

namespace CSCommonSecrets;

/// <summary>
/// ContactSecret stores one encrypted contact
/// </summary>
public sealed partial class ContactSecret
{
	/// <summary>
	/// Default constructor for ContactSecret, async
	/// </summary>
	/// <param name="contact">Contact to encrypt</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	public static async Task<ContactSecret> CreateContactSecretAsync(Contact contact, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
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

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings);

		ContactSecret contactSecret = new ContactSecret()
		{
			keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier),
			algorithm = algorithm,
			// Encrypt the AUDALF payload with given algorithm
			audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions),
		};

		// Calculate new checksum
		await contactSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

		return contactSecret;
	}
	
	/// <summary>
	/// Constructor for custom dictionary, use this only if you know what you are doing, async
	/// </summary>
	/// <param name="contactAsDictionary">Dictionary containing contact keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	public static async Task<ContactSecret> CreateContactSecretAsync(Dictionary<string, object> contactAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		ContactSecret contactSecret = new ContactSecret()
		{
			keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier),
			algorithm = algorithm,
			// Encrypt the AUDALF payload with given algorithm
			audalfData = await algorithm.EncryptBytesAsync(serializedBytes, derivedPassword, securityFunctions),
		};

		// Calculate new checksum
		await contactSecret.CalculateAndUpdateChecksumAsync(securityFunctions);

		return contactSecret;
	}

	#region Common getters

	private async Task<Dictionary<string, object>> GetContactAsDictionaryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
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

		Dictionary<string, object> contactAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return contactAsDictionary;
	}

	/// <summary>
	/// Get Contact. Use this for situation where you want to convert secret -> non secret, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact</returns>
	public async Task<Contact> GetContactAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		Dictionary<string, object> dict = await this.GetContactAsDictionaryAsync(derivedPassword, securityFunctions);
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
		Contact returnValue = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
									emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
									country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
									websites, relationship, notes, securityFunctions);

		returnValue.creationTime = ((DateTimeOffset)dict[Contact.creationTimeKey]).ToUnixTimeSeconds();
		returnValue.modificationTime = ((DateTimeOffset)dict[Contact.modificationTimeKey]).ToUnixTimeSeconds();

		return returnValue;
	}

	/// <summary>
	/// Get contact first name, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact first name as string</returns>
	public async Task<string> GetFirstNameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.firstNameKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact last name, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact last name as string</returns>
	public async Task<string> GetLastNameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.lastNameKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact middle name, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact middle name as string</returns>
	public async Task<string> GetMiddleNameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.middleNameKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact name prefix, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact name prefix as string</returns>
	public async Task<string> GetNamePrefixAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.namePrefixKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact name suffix, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact name suffix as string</returns>
	public async Task<string> GetNameSuffixAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.nameSuffixKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact nickname, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact nickname as string</returns>
	public async Task<string> GetNicknameAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.nicknameKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact company, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact company as string</returns>
	public async Task<string> GetCompanyAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.companyKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact job title, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact job title as string</returns>
	public async Task<string> GetJobTitleAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.jobTitleKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact department, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact department as string</returns>
	public async Task<string> GetDepartmentAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.departmentKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get email addresses (separated with tab), async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact email addresses as tab separated string</returns>
	public async Task<string> GetEmailsAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.emailsKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get email addresses, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact email addresses as string array</returns>
	public async Task<string[]> GetEmailsArrayAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return ((string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.emailsKey, deserializationSettings, securityFunctions)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get email address descriptions (separated with tab), async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact email address descriptions as tab separated string</returns>
	public async Task<string> GetEmailDescriptionsAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.emailDescriptionsKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get email address descriptions, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact email address descriptions as string array</returns>
	public async Task<string[]> GetEmailDescriptionsArrayAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return ((string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.emailDescriptionsKey, deserializationSettings, securityFunctions)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get phone numbers (separated with tab), async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact phone numbers as tab separated string</returns>
	public async Task<string> GetPhoneNumbersAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumbersKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get phone numbers, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact phone numbers as string array</returns>
	public async Task<string[]> GetPhoneNumbersArrayAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return ((string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumbersKey, deserializationSettings, securityFunctions)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get phone numbers descriptions (separated with tab), async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact phone numbers descriptions as tab separated string</returns>
	public async Task<string> GetPhoneNumberDescriptionsAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumberDescriptionsKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get phone numbers descriptions, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact phone numbers descriptions as string array</returns>
	public async Task<string[]> GetPhoneNumberDescriptionsArrayAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return ((string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumberDescriptionsKey, deserializationSettings, securityFunctions)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get country, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact country as string</returns>
	public async Task<string> GetCountryAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.countryKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get street address, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact street address as string</returns>
	public async Task<string> GetStreetAddressAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.streetAddressKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get street address additional, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact street address additional  as string</returns>
	public async Task<string> GetStreetAddressAdditionalAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.streetAddressAdditionalKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get postal code, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact postal code as string</returns>
	public async Task<string> GetPostalCodeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.postalCodeKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get city, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact city as string</returns>
	public async Task<string> GetCityAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.cityKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get PO Box, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact PO Box as string</returns>
	public async Task<string> GetPOBoxAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.poBoxKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get birthday, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact birthday as string</returns>
	public async Task<string> GetBirthdayAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.birthdayKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get websites, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact websites as string array</returns>
	public async Task<string[]> GetWebsitesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return ((string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.websitesKey, deserializationSettings, securityFunctions)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get relationship 
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact relationship as string</returns>
	public async Task<string> GetRelationshipAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.relationshipKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get notes, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact notes as string</returns>
	public async Task<string> GetNotesAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (string)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.notesKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact creation time, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact creation time as DateTimeOffset</returns>
	public async Task<DateTimeOffset> GetCreationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.creationTimeKey, deserializationSettings, securityFunctions);
	}

	/// <summary>
	/// Get contact modification time, async
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>Contact modification time as DateTimeOffset</returns>
	public async Task<DateTimeOffset> GetModificationTimeAsync(byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return (DateTimeOffset)await Helpers.GetSingleValueAsync(this.audalfData, this.algorithm, derivedPassword, Contact.modificationTimeKey, deserializationSettings, securityFunctions);
	}

	

	#endregion // Common getters


	#region Common setters

	/// <summary>
	/// Try to set new first name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newFirstName">New first name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetFirstNameAsync(string newFirstName, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.firstNameKey, newFirstName, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new last name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newLastName">New last name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetLastNameAsync(string newLastName, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.lastNameKey, newLastName, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new middle name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newMiddleName">New middle name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetMiddleNameAsync(string newMiddleName, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.middleNameKey, newMiddleName, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new name prefix for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newNamePrefix">New name prefix</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetNamePrefixAsync(string newNamePrefix, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.namePrefixKey, newNamePrefix, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new name suffix for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newNameSuffix">New name suffix</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetNameSuffixAsync(string newNameSuffix, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.nameSuffixKey, newNameSuffix, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new nickname for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newNickname">New nickname</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetNicknameAsync(string newNickname, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.nicknameKey, newNickname, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new company for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newCompany">New company</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetCompanyAsync(string newCompany, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.companyKey, newCompany, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new job title for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newJobTitle">New job title</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetJobTitleAsync(string newJobTitle, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.jobTitleKey, newJobTitle, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new department for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newDepartment">New department</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetDepartmentAsync(string newDepartment, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.departmentKey, newDepartment, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new emails and email descriptions for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newEmails">New emails</param>
	/// <param name="newEmailDescriptions">New email descriptions</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetEmailsAndDescriptionsAsync(string[] newEmails, string[] newEmailDescriptions, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.emailsKey, string.Join(Contact.separatorString, newEmails), DateTimeOffset.UtcNow, derivedPassword, securityFunctions) &&
				await this.GenericSetAsync(Contact.emailDescriptionsKey, string.Join(Contact.separatorString, newEmailDescriptions), DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new phone numbers and phone number descriptions for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newPhoneNumbers">New emails</param>
	/// <param name="newPhoneNumberDescriptions">New email descriptions</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetPhoneNumbersAndDescriptionsAsync(string[] newPhoneNumbers, string[] newPhoneNumberDescriptions, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.phoneNumbersKey, string.Join(Contact.separatorString, newPhoneNumbers), DateTimeOffset.UtcNow, derivedPassword, securityFunctions) &&
				await this.GenericSetAsync(Contact.phoneNumberDescriptionsKey, string.Join(Contact.separatorString, newPhoneNumberDescriptions), DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new country for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newCountry">New country</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetCountryAsync(string newCountry, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.countryKey, newCountry, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new street address for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newStreetAddress">New street address</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetStreetAddressAsync(string newStreetAddress, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.streetAddressKey, newStreetAddress, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new street address for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newStreetAddressAdditional">New street address additional</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetStreetAddressAdditionalAsync(string newStreetAddressAdditional, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.streetAddressAdditionalKey, newStreetAddressAdditional, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new postal code for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newPostalCode">New postal code</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetPostalCodeAsync(string newPostalCode, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.postalCodeKey, newPostalCode, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new city for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newCity">New city</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetCityAsync(string newCity, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.cityKey, newCity, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new PO box for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newPOBox">New PO box</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetPOBoxAsync(string newPOBox, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.poBoxKey, newPOBox, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new birthday for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newBirthday">New birthday</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetBirthdayAsync(string newBirthday, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.birthdayKey, newBirthday, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new relationship for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newRelationship">New relationship</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetRelationshipAsync(string newRelationship, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.relationshipKey, newRelationship, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new notes for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newNotes">New notes</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetNotesAsync(string newNotes, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	/// <summary>
	/// Try to set new websites for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret, async
	/// </summary>
	/// <param name="newWebsites">New websites</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public async Task<bool> SetWebsitesAsync(string[] newWebsites, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		return await this.GenericSetAsync(Contact.websitesKey, string.Join(Contact.separatorString, newWebsites), DateTimeOffset.UtcNow, derivedPassword, securityFunctions);
	}

	private async Task<bool> GenericSetAsync(string key, object value, DateTimeOffset modificationTime, byte[] derivedPassword, ISecurityAsyncFunctions securityFunctions)
	{
		try 
		{
			Dictionary<string, object> contactAsDictionary = await this.GetContactAsDictionaryAsync(derivedPassword, securityFunctions);;
			// Update wanted value
			contactAsDictionary[key] = value;
			// Update modification time
			contactAsDictionary[Contact.modificationTimeKey] = modificationTime;

			// Generate new algorithm since data has changed
			this.algorithm = SymmetricKeyAlgorithm.GenerateNew(this.algorithm.GetSymmetricEncryptionAlgorithm(), securityFunctions);

			// Create AUDALF payload from dictionary
			byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings );

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

	#endregion // Common setters

	#region Checksum

	/// <summary>
	/// Check if checksum matches content, async
	/// </summary>
	/// <param name="securityFunctions">Security functions</param>
	/// <returns>True if matches; False otherwise</returns>
	public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
	{
		return checksum == await this.CalculateHexChecksumAsync(securityFunctions);
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

#endif // ASYNC_WITH_CUSTOM