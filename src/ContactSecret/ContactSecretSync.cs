#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Collections.Generic;
using System.Text;
using CSharp_AUDALF;

using System.Diagnostics.CodeAnalysis;

namespace CSCommonSecrets;

/// <summary>
/// ContactSecret stores one encrypted contact
/// </summary>
public sealed partial class ContactSecret
{
	/// <summary>
	/// Default constructor for ContactSecret
	/// </summary>
	/// <param name="contact">Contact to encrypt</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
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
		byte[] serializedBytes = AUDALF_Serialize.Serialize(dictionaryForAUDALF, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	/// <summary>
	/// Constructor for custom dictionary, use this only if you know what you are doing
	/// </summary>
	/// <param name="contactAsDictionary">Dictionary containing contact keys and values</param>
	/// <param name="keyIdentifier">Key identifier</param>
	/// <param name="algorithm">Symmetric Key Algorithm used for encryption</param>
	/// <param name="derivedPassword">Derived password</param>
	[SetsRequiredMembers]
	public ContactSecret(Dictionary<string, object> contactAsDictionary, string keyIdentifier, SymmetricKeyAlgorithm algorithm, byte[] derivedPassword)
	{
		this.keyIdentifier = Encoding.UTF8.GetBytes(keyIdentifier);

		this.algorithm = algorithm;

		// Create AUDALF payload from dictionary
		byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

		// Encrypt the AUDALF payload with given algorithm
		this.audalfData = algorithm.EncryptBytes(serializedBytes, derivedPassword);

		// Calculate new checksum
		this.CalculateAndUpdateChecksum();
	}

	#region Common getters

	private Dictionary<string, object> GetContactAsDictionary(byte[] derivedPassword)
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

		Dictionary<string, object> contactAsDictionary = AUDALF_Deserialize.Deserialize<string, object>(decryptedAUDALF, settings: deserializationSettings);

		return contactAsDictionary;
	}

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
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.firstNameKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact last name
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact last name as string</returns>
	public string GetLastName(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.lastNameKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact middle name
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact middle name as string</returns>
	public string GetMiddleName(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.middleNameKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact name prefix
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact name prefix as string</returns>
	public string GetNamePrefix(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.namePrefixKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact name suffix
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact name suffix as string</returns>
	public string GetNameSuffix(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.nameSuffixKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact nickname
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact nickname as string</returns>
	public string GetNickname(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.nicknameKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact company
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact company as string</returns>
	public string GetCompany(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.companyKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact job title
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact job title as string</returns>
	public string GetJobTitle(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.jobTitleKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact department
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact department as string</returns>
	public string GetDepartment(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.departmentKey, deserializationSettings);
	}

	/// <summary>
	/// Get email addresses (separated with tab)
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact email addresses as single string</returns>
	public string GetEmails(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.emailsKey, deserializationSettings);
	}

	/// <summary>
	/// Get email addresses as string array
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact email addresses as string array</returns>
	public string[] GetEmailsArray(byte[] derivedPassword)
	{
		return ((string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.emailsKey, deserializationSettings)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get email address descriptions (separated with tab)
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact email address descriptions as single string</returns>
	public string GetEmailDescriptions(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.emailDescriptionsKey, deserializationSettings);
	}

	/// <summary>
	/// Get email address descriptions
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact email address descriptions as string array</returns>
	public string[] GetEmailDescriptionsArray(byte[] derivedPassword)
	{
		return ((string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.emailDescriptionsKey, deserializationSettings)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get phone numbers (separated with tab)
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact phone numbers as single string</returns>
	public string GetPhoneNumbers(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumbersKey, deserializationSettings);
	}

	/// <summary>
	/// Get phone numbers as string array
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact phone numbers as string array</returns>
	public string[] GetPhoneNumbersArray(byte[] derivedPassword)
	{
		return ((string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumbersKey, deserializationSettings)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get phone numbers descriptions (separated with tab)
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact phone numbers descriptions as single string</returns>
	public string GetPhoneNumberDescriptions(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumberDescriptionsKey, deserializationSettings);
	}

	/// <summary>
	/// Get phone numbers descriptions
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact phone numbers descriptions as string array</returns>
	public string[] GetPhoneNumberDescriptionsArray(byte[] derivedPassword)
	{
		return ((string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.phoneNumberDescriptionsKey, deserializationSettings)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get country
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact country as string</returns>
	public string GetCountry(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.countryKey, deserializationSettings);
	}

	/// <summary>
	/// Get street address
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact street address as string</returns>
	public string GetStreetAddress(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.streetAddressKey, deserializationSettings);
	}

	/// <summary>
	/// Get street address additional 
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact street address additional  as string</returns>
	public string GetStreetAddressAdditional(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.streetAddressAdditionalKey, deserializationSettings);
	}

	/// <summary>
	/// Get postal code
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact postal code as string</returns>
	public string GetPostalCode(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.postalCodeKey, deserializationSettings);
	}

	/// <summary>
	/// Get city
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact city as string</returns>
	public string GetCity(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.cityKey, deserializationSettings);
	}

	/// <summary>
	/// Get PO Box
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact PO Box as string</returns>
	public string GetPOBox(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.poBoxKey, deserializationSettings);
	}

	/// <summary>
	/// Get birthday
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact birthday as string</returns>
	public string GetBirthday(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.birthdayKey, deserializationSettings);
	}

	/// <summary>
	/// Get websites
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact websites as string array</returns>
	public string[] GetWebsites(byte[] derivedPassword)
	{
		return ((string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.websitesKey, deserializationSettings)).Split(Contact.separatorChar);
	}

	/// <summary>
	/// Get relationship 
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact relationship  as string</returns>
	public string GetRelationship(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.relationshipKey, deserializationSettings);
	}

	/// <summary>
	/// Get notes 
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact notes  as string</returns>
	public string GetNotes(byte[] derivedPassword)
	{
		return (string)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.notesKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact creation time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact creation time as DateTimeOffset</returns>
	public DateTimeOffset GetCreationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.creationTimeKey, deserializationSettings);
	}

	/// <summary>
	/// Get contact modification time
	/// </summary>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>Contact modification time as DateTimeOffset</returns>
	public DateTimeOffset GetModificationTime(byte[] derivedPassword)
	{
		return (DateTimeOffset)Helpers.GetSingleValue(this.audalfData, this.algorithm, derivedPassword, Contact.modificationTimeKey, deserializationSettings);
	}

	#endregion // Common getters


	#region Common setters

	/// <summary>
	/// Try to set new first name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newFirstName">New first name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetFirstName(string newFirstName, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.firstNameKey, newFirstName, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new last name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newLastName">New last name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetLastName(string newLastName, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.lastNameKey, newLastName, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new middle name for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newMiddleName">New middle name</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetMiddleName(string newMiddleName, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.middleNameKey, newMiddleName, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new name prefix for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newNamePrefix">New name prefix</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetNamePrefix(string newNamePrefix, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.namePrefixKey, newNamePrefix, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new name suffix for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newNameSuffix">New name suffix</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetNameSuffix(string newNameSuffix, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.nameSuffixKey, newNameSuffix, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new nickname for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newNickname">New nickname</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetNickname(string newNickname, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.nicknameKey, newNickname, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new company for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newCompany">New company</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetCompany(string newCompany, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.companyKey, newCompany, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new job title for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newJobTitle">New job title</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetJobTitle(string newJobTitle, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.jobTitleKey, newJobTitle, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new department for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newDepartment">New department</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetDepartment(string newDepartment, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.departmentKey, newDepartment, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new emails and email descriptions for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newEmails">New emails</param>
	/// <param name="newEmailDescriptions">New email descriptions</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetEmailsAndDescriptions(string[] newEmails, string[] newEmailDescriptions, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.emailsKey, string.Join(Contact.separatorString, newEmails), DateTimeOffset.UtcNow, derivedPassword) &&
				this.GenericSet(Contact.emailDescriptionsKey, string.Join(Contact.separatorString, newEmailDescriptions), DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new phone numbers and phone number descriptions for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newPhoneNumbers">New emails</param>
	/// <param name="newPhoneNumberDescriptions">New email descriptions</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetPhoneNumbersAndDescriptions(string[] newPhoneNumbers, string[] newPhoneNumberDescriptions, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.phoneNumbersKey, string.Join(Contact.separatorString, newPhoneNumbers), DateTimeOffset.UtcNow, derivedPassword) &&
				this.GenericSet(Contact.phoneNumberDescriptionsKey, string.Join(Contact.separatorString, newPhoneNumberDescriptions), DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new country for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newCountry">New country</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetCountry(string newCountry, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.countryKey, newCountry, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new street address for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newStreetAddress">New street address</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetStreetAddress(string newStreetAddress, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.streetAddressKey, newStreetAddress, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new street address for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newStreetAddressAdditional">New street address additional</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetStreetAddressAdditional(string newStreetAddressAdditional, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.streetAddressAdditionalKey, newStreetAddressAdditional, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new postal code for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newPostalCode">New postal code</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetPostalCode(string newPostalCode, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.postalCodeKey, newPostalCode, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new city for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newCity">New city</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetCity(string newCity, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.cityKey, newCity, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new PO box for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newPOBox">New PO box</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetPOBox(string newPOBox, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.poBoxKey, newPOBox, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new birthday for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newBirthday">New birthday</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetBirthday(string newBirthday, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.birthdayKey, newBirthday, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new relationship for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newRelationship">New relationship</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetRelationship(string newRelationship, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.relationshipKey, newRelationship, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new notes for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newNotes">New notes</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetNotes(string newNotes, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.notesKey, newNotes, DateTimeOffset.UtcNow, derivedPassword);
	}

	/// <summary>
	/// Try to set new websites for contact secret by decrypting the current contact secret, setting a new value and then encrypting the modified contact secret
	/// </summary>
	/// <param name="newWebsites">New websites</param>
	/// <param name="derivedPassword">Derived password</param>
	/// <returns>True if set goes correctly; False otherwise</returns>
	public bool SetWebsites(string[] newWebsites, byte[] derivedPassword)
	{
		return this.GenericSet(Contact.websitesKey, string.Join(Contact.separatorString, newWebsites), DateTimeOffset.UtcNow, derivedPassword);
	}

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
			byte[] serializedBytes = AUDALF_Serialize.Serialize(contactAsDictionary, valueTypes: null, serializationSettings: serializationSettings);

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