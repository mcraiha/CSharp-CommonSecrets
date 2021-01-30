using System;
using System.Text;

namespace CSCommonSecrets
{
	/// <summary>
	/// Contact stores one plaintext (anyone can read) contact
	/// </summary>
	public sealed class Contact
	{
		/// <summary>
		/// Contact first name as byte array
		/// </summary>
		public byte[] firstName { get; set; } = new byte[0];

		/// <summary>
		/// Contact first name key
		/// </summary>
		public static readonly string firstNameKey = nameof(firstName);

		/// <summary>
		/// Contact last name as byte array
		/// </summary>
		public byte[] lastName { get; set; } = new byte[0];

		/// <summary>
		/// Contact last name key
		/// </summary>
		public static readonly string lastNameKey = nameof(lastName);

		/// <summary>
		/// Contact middle name as byte array
		/// </summary>
		public byte[] middleName { get; set; } = new byte[0];

		/// <summary>
		/// Contact middle name key
		/// </summary>
		public static readonly string middleNameKey = nameof(middleName);

		/// <summary>
		/// Contact name prefix as byte array
		/// </summary>
		public byte[] namePrefix { get; set; } = new byte[0];

		/// <summary>
		/// Contact name prefix key
		/// </summary>
		public static readonly string namePrefixKey = nameof(namePrefix);

		/// <summary>
		/// Contact name suffix as byte array
		/// </summary>
		public byte[] nameSuffix { get; set; } = new byte[0];

		/// <summary>
		/// Contact name suffix key
		/// </summary>
		public static readonly string nameSuffixKey = nameof(nameSuffix);

		/// <summary>
		/// Contact nickname as byte array
		/// </summary>
		public byte[] nickname { get; set; } = new byte[0];

		/// <summary>
		/// Contact nickname key
		/// </summary>
		public static readonly string nicknameKey = nameof(nickname);

		/// <summary>
		/// Contact company as byte array
		/// </summary>
		public byte[] company { get; set; } = new byte[0];

		/// <summary>
		/// Contact company key
		/// </summary>
		public static readonly string companyKey = nameof(company);

		/// <summary>
		/// Contact job title as byte array
		/// </summary>
		public byte[] jobTitle { get; set; } = new byte[0];

		/// <summary>
		/// Contact job title key
		/// </summary>
		public static readonly string jobTitleKey = nameof(jobTitle);

		/// <summary>
		/// Contact department as byte array
		/// </summary>
		public byte[] department { get; set; } = new byte[0];

		/// <summary>
		/// Contact department key
		/// </summary>
		public static readonly string departmentKey = nameof(department);

		/// <summary>
		/// Contact emails as byte array, contains tab (\t) separated email entries
		/// </summary>
		public byte[] emails { get; set; } = new byte[0];

		/// <summary>
		/// Contact emails key
		/// </summary>
		public static readonly string emailsKey = nameof(emails);

		/// <summary>
		/// Contact email descriptions as byte array, contains tab (\t) separated email description entries
		/// </summary>
		public byte[] emailDescriptions { get; set; } = new byte[0];

		/// <summary>
		/// Contact email descriptions key
		/// </summary>
		public static readonly string emailDescriptionsKey = nameof(emailDescriptions);

		/// <summary>
		/// Contact phone numbers as byte array, contains tab (\t) separated phone number entries
		/// </summary>
		public byte[] phoneNumbers { get; set; } = new byte[0];

		/// <summary>
		/// Contact phone numbers key
		/// </summary>
		public static readonly string phoneNumbersKey = nameof(phoneNumbers);

		/// <summary>
		/// Contact phone number descriptions as byte array, contains tab (\t) separated phone number description entries
		/// </summary>
		public byte[] phoneNumberDescriptions { get; set; } = new byte[0];

		/// <summary>
		/// Contact phone number descriptions key
		/// </summary>
		public static readonly string phoneNumberDescriptionsKey = nameof(phoneNumberDescriptions);

		/// <summary>
		/// Contact country as byte array
		/// </summary>
		public byte[] country { get; set; } = new byte[0];

		/// <summary>
		/// Contact country key
		/// </summary>
		public static readonly string countryKey = nameof(country);

		/// <summary>
		/// Contact street address as byte array
		/// </summary>
		public byte[] streetAddress { get; set; } = new byte[0];

		/// <summary>
		/// Contact street address key
		/// </summary>
		public static readonly string streetAddressKey = nameof(streetAddress);

		/// <summary>
		/// Contact street address additional as byte array
		/// </summary>
		public byte[] streetAddressAdditional { get; set; } = new byte[0];

		/// <summary>
		/// Contact street address additional key
		/// </summary>
		public static readonly string streetAddressAdditionalKey = nameof(streetAddressAdditional);

		/// <summary>
		/// Contact postal code as byte array
		/// </summary>
		public byte[] postalCode { get; set; } = new byte[0];

		/// <summary>
		/// Contact postal code key
		/// </summary>
		public static readonly string postalCodeKey = nameof(postalCode);

		/// <summary>
		/// Contact city as byte array
		/// </summary>
		public byte[] city { get; set; } = new byte[0];

		/// <summary>
		/// Contact city key
		/// </summary>
		public static readonly string cityKey = nameof(city);

		/// <summary>
		/// Contact PO Box as byte array
		/// </summary>
		public byte[] poBox { get; set; } = new byte[0];

		/// <summary>
		/// Contact PO Box key
		/// </summary>
		public static readonly string poBoxKey = nameof(poBox);

		/// <summary>
		/// Contact birthday as byte array
		/// </summary>
		public byte[] birthday { get; set; } = new byte[0];

		/// <summary>
		/// Contact birthday key
		/// </summary>
		public static readonly string birthdayKey = nameof(birthday);

		/// <summary>
		/// Contact websites as byte array, contains tab (\t) separated website entries
		/// </summary>
		public byte[] websites { get; set; } = new byte[0];

		/// <summary>
		/// Contact websites key
		/// </summary>
		public static readonly string websitesKey = nameof(websites);

		/// <summary>
		/// Contact relationship as byte array
		/// </summary>
		public byte[] relationship { get; set; } = new byte[0];

		/// <summary>
		/// Contact relationship key
		/// </summary>
		public static readonly string relationshipKey = nameof(relationship);

		/// <summary>
		/// Contact notes as bytes, in normal case you want to use GetNotes() and UpdateNotes()
		/// </summary>
		public byte[] notes { get; set; } = new byte[0];

		/// <summary>
		/// Key for storing notes data to AUDALF
		/// </summary>
		public static readonly string notesKey = nameof(notes);

		/// <summary>
		/// Creation time of contact, in Unix seconds since epoch
		/// </summary>
		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing contact creation time to AUDALF
		/// </summary>
		public static readonly string creationTimeKey = nameof(creationTime);

		/// <summary>
		/// Last modification time of contact, in Unix seconds since epoch
		/// </summary>
		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

		/// <summary>
		/// Key for storing contact last modification time to AUDALF
		/// </summary>
		public static readonly string modificationTimeKey = nameof(modificationTime);

		/// <summary>
		/// Calculated checksum of contact
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// Tab \t is used to separate entries in lists
		/// </summary>
		public static readonly char separatorChar = '\t';

		/// <summary>
		/// Tab \t is used to separate entries in lists
		/// </summary>
		public static readonly string separatorString = '\t'.ToString();

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public Contact()
		{
			
		}

		/// <summary>
		/// Deep copy existing Contact to new Contact
		/// </summary>
		/// <param name="copyThis">Contact to copy</param>
		public Contact(Contact copyThis)
		{
			this.firstName = new byte[copyThis.firstName.Length];
			Buffer.BlockCopy(copyThis.firstName, 0, this.firstName, 0, copyThis.firstName.Length);

			this.lastName = new byte[copyThis.lastName.Length];
			Buffer.BlockCopy(copyThis.lastName, 0, this.lastName, 0, copyThis.lastName.Length);

			this.middleName = new byte[copyThis.middleName.Length];
			Buffer.BlockCopy(copyThis.middleName, 0, this.middleName, 0, copyThis.middleName.Length);

			this.namePrefix = new byte[copyThis.namePrefix.Length];
			Buffer.BlockCopy(copyThis.namePrefix, 0, this.namePrefix, 0, copyThis.namePrefix.Length);

			this.nameSuffix = new byte[copyThis.nameSuffix.Length];
			Buffer.BlockCopy(copyThis.nameSuffix, 0, this.nameSuffix, 0, copyThis.nameSuffix.Length);

			this.nickname = new byte[copyThis.nickname.Length];
			Buffer.BlockCopy(copyThis.nickname, 0, this.nickname, 0, copyThis.nickname.Length);

			this.company = new byte[copyThis.company.Length];
			Buffer.BlockCopy(copyThis.company, 0, this.company, 0, copyThis.company.Length);

			this.jobTitle = new byte[copyThis.jobTitle.Length];
			Buffer.BlockCopy(copyThis.jobTitle, 0, this.jobTitle, 0, copyThis.jobTitle.Length);

			this.department = new byte[copyThis.department.Length];
			Buffer.BlockCopy(copyThis.department, 0, this.department, 0, copyThis.department.Length);

			this.emails = new byte[copyThis.emails.Length];
			Buffer.BlockCopy(copyThis.emails, 0, this.emails, 0, copyThis.emails.Length);

			this.emailDescriptions = new byte[copyThis.emailDescriptions.Length];
			Buffer.BlockCopy(copyThis.emailDescriptions, 0, this.emailDescriptions, 0, copyThis.emailDescriptions.Length);

			this.phoneNumbers = new byte[copyThis.phoneNumbers.Length];
			Buffer.BlockCopy(copyThis.phoneNumbers, 0, this.phoneNumbers, 0, copyThis.phoneNumbers.Length);

			this.phoneNumberDescriptions = new byte[copyThis.phoneNumberDescriptions.Length];
			Buffer.BlockCopy(copyThis.phoneNumberDescriptions, 0, this.phoneNumberDescriptions, 0, copyThis.phoneNumberDescriptions.Length);

			this.country = new byte[copyThis.country.Length];
			Buffer.BlockCopy(copyThis.country, 0, this.country, 0, copyThis.country.Length);

			this.streetAddress = new byte[copyThis.streetAddress.Length];
			Buffer.BlockCopy(copyThis.streetAddress, 0, this.streetAddress, 0, copyThis.streetAddress.Length);

			this.streetAddressAdditional = new byte[copyThis.streetAddressAdditional.Length];
			Buffer.BlockCopy(copyThis.streetAddressAdditional, 0, this.streetAddressAdditional, 0, copyThis.streetAddressAdditional.Length);

			this.postalCode = new byte[copyThis.postalCode.Length];
			Buffer.BlockCopy(copyThis.postalCode, 0, this.postalCode, 0, copyThis.postalCode.Length);

			this.city = new byte[copyThis.city.Length];
			Buffer.BlockCopy(copyThis.city, 0, this.city, 0, copyThis.city.Length);

			this.poBox = new byte[copyThis.poBox.Length];
			Buffer.BlockCopy(copyThis.poBox, 0, this.poBox, 0, copyThis.poBox.Length);

			this.birthday = new byte[copyThis.birthday.Length];
			Buffer.BlockCopy(copyThis.birthday, 0, this.birthday, 0, copyThis.birthday.Length);

			this.websites = new byte[copyThis.websites.Length];
			Buffer.BlockCopy(copyThis.websites, 0, this.websites, 0, copyThis.websites.Length);

			this.relationship = new byte[copyThis.relationship.Length];
			Buffer.BlockCopy(copyThis.relationship, 0, this.relationship, 0, copyThis.relationship.Length);

			this.notes = new byte[copyThis.notes.Length];
			Buffer.BlockCopy(copyThis.notes, 0, this.notes, 0, copyThis.notes.Length);

			this.creationTime = copyThis.creationTime;
			this.modificationTime = copyThis.modificationTime;

			this.checksum = copyThis.checksum;
		}

		/// <summary>
		/// Default small constructor for Contact
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		public Contact(string firstName, string lastName, string middleName) : this (firstName, lastName, middleName, DateTimeOffset.UtcNow)
		{

		}

		/// <summary>
		/// Constructor (small) with creation time override
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		/// <param name="time">Creation time</param>
		public Contact(string firstName, string lastName, string middleName, DateTimeOffset time)
		{
			this.firstName = Encoding.UTF8.GetBytes(firstName);
			this.lastName = Encoding.UTF8.GetBytes(lastName);
			this.middleName = Encoding.UTF8.GetBytes(middleName);
			this.creationTime = time.ToUnixTimeSeconds();
			this.modificationTime = time.ToUnixTimeSeconds();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Constuctor (full)
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		/// <param name="namePrefix">Name prefix</param>
		/// <param name="nameSuffix">Name suffix</param>
		/// <param name="nickname">Nickname</param>
		/// <param name="company">Company</param>
		/// <param name="jobTitle">Job title</param>
		/// <param name="department">Department</param>
		/// <param name="emails">Emails</param>
		/// <param name="emailDescriptions">Email descriptions</param>
		/// <param name="phoneNumbers">Phone numbers</param>
		/// <param name="phoneNumberDescriptions">Phone number descriptions</param>
		/// <param name="country">Country</param>
		/// <param name="streetAddress">Street address</param>
		/// <param name="streetAddressAdditional">Street address (additional)</param>
		/// <param name="postalCode">Postal code</param>
		/// <param name="city">City</param>
		/// <param name="poBox">PO Box</param>
		/// <param name="birthday">Birthday</param>
		/// <param name="websites">Websites</param>
		/// <param name="relationship">Relationship</param>
		/// <param name="notes">Notes</param>
		public Contact(string firstName, string lastName, string middleName, string namePrefix, string nameSuffix, string nickname, string company, string jobTitle, string department, string[] emails, string[] emailDescriptions, string[] phoneNumbers, string[] phoneNumberDescriptions, string country, string streetAddress, string streetAddressAdditional, string postalCode, string city, string poBox, string birthday, string[] websites, string relationship, string notes)
		: this (firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday, websites, relationship, notes, DateTimeOffset.UtcNow)
		{

		}

		/// <summary>
		/// Constuctor (full) with creation time override
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		/// <param name="namePrefix">Name prefix</param>
		/// <param name="nameSuffix">Name suffix</param>
		/// <param name="nickname">Nickname</param>
		/// <param name="company">Company</param>
		/// <param name="jobTitle">Job title</param>
		/// <param name="department">Department</param>
		/// <param name="emails">Emails</param>
		/// <param name="emailDescriptions">Email descriptions</param>
		/// <param name="phoneNumbers">Phone numbers</param>
		/// <param name="phoneNumberDescriptions">Phone number descriptions</param>
		/// <param name="country">Country</param>
		/// <param name="streetAddress">Street address</param>
		/// <param name="streetAddressAdditional">Street address (additional)</param>
		/// <param name="postalCode">Postal code</param>
		/// <param name="city">City</param>
		/// <param name="poBox">PO Box</param>
		/// <param name="birthday">Birthday</param>
		/// <param name="websites">Websites</param>
		/// <param name="relationship">Relationship</param>
		/// <param name="notes">Notes</param>
		/// <param name="time">Creation time</param>
		public Contact(string firstName, string lastName, string middleName, string namePrefix, string nameSuffix, string nickname, string company, string jobTitle, string department, string[] emails, string[] emailDescriptions, string[] phoneNumbers, string[] phoneNumberDescriptions, string country, string streetAddress, string streetAddressAdditional, string postalCode, string city, string poBox, string birthday, string[] websites, string relationship, string notes, DateTimeOffset time)
		{
			this.firstName = Encoding.UTF8.GetBytes(firstName);
			this.lastName = Encoding.UTF8.GetBytes(lastName);
			this.middleName = Encoding.UTF8.GetBytes(middleName);
			this.namePrefix = Encoding.UTF8.GetBytes(namePrefix);
			this.nameSuffix = Encoding.UTF8.GetBytes(nameSuffix);
			this.nickname = Encoding.UTF8.GetBytes(nickname);
			this.company = Encoding.UTF8.GetBytes(company);
			this.jobTitle = Encoding.UTF8.GetBytes(jobTitle);
			this.department = Encoding.UTF8.GetBytes(department);
			this.emails = Encoding.UTF8.GetBytes(string.Join(separatorString, emails));
			this.emailDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, emailDescriptions));
			this.phoneNumbers = Encoding.UTF8.GetBytes(string.Join(separatorString, phoneNumbers));
			this.phoneNumberDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, phoneNumberDescriptions));
			this.country = Encoding.UTF8.GetBytes(country);
			this.streetAddress = Encoding.UTF8.GetBytes(streetAddress);
			this.streetAddressAdditional = Encoding.UTF8.GetBytes(streetAddressAdditional);
			this.postalCode = Encoding.UTF8.GetBytes(postalCode);
			this.city = Encoding.UTF8.GetBytes(city);
			this.poBox = Encoding.UTF8.GetBytes(poBox);
			this.birthday = Encoding.UTF8.GetBytes(birthday);
			this.websites = Encoding.UTF8.GetBytes(string.Join(separatorString, websites));
			this.relationship = Encoding.UTF8.GetBytes(relationship);
			this.notes = Encoding.UTF8.GetBytes(notes);
			this.creationTime = time.ToUnixTimeSeconds();
			this.modificationTime = time.ToUnixTimeSeconds();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Create shallow copy, mostly for testing purposes
		/// </summary>
		/// <returns>Shallow copy of Contact</returns>
		public Contact ShallowCopy()
		{
			return (Contact) this.MemberwiseClone();
		}

		#region Updates

		/// <summary>
		/// Update first name
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedFirstName">Updated first name</param>
		public void UpdateFirstName(string updatedFirstName)
		{
			this.firstName = Encoding.UTF8.GetBytes(updatedFirstName);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update last name
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedLastName">Updated last name</param>
		public void UpdateLastName(string updatedLastName)
		{
			this.lastName = Encoding.UTF8.GetBytes(updatedLastName);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update middle name
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedMiddleName">Updated middle name</param>
		public void UpdateMiddleName(string updatedMiddleName)
		{
			this.middleName = Encoding.UTF8.GetBytes(updatedMiddleName);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update name prefix
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNamePrefix">Updated name prefix</param>
		public void UpdateNamePrefix(string updatedNamePrefix)
		{
			this.namePrefix = Encoding.UTF8.GetBytes(updatedNamePrefix);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update name suffix
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNameSuffix">Updated name suffix</param>
		public void UpdateNameSuffix(string updatedNameSuffix)
		{
			this.nameSuffix = Encoding.UTF8.GetBytes(updatedNameSuffix);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update nickname
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNickname">Updated nickname</param>
		public void UpdateNickname(string updatedNickname)
		{
			this.nickname = Encoding.UTF8.GetBytes(updatedNickname);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update company
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCompany">Updated company</param>
		public void UpdateCompany(string updatedCompany)
		{
			this.company = Encoding.UTF8.GetBytes(updatedCompany);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update job title
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedJobTitle">Updated job title</param>
		public void UpdateJobTitle(string updatedJobTitle)
		{
			this.jobTitle = Encoding.UTF8.GetBytes(updatedJobTitle);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update department
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedDepartment">Updated department</param>
		public void UpdateDepartment(string updatedDepartment)
		{
			this.department = Encoding.UTF8.GetBytes(updatedDepartment);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update emails and descriptions (both are tab separated)
		/// </summary>
		/// <param name="updatedEmails">Updated emails (tab separated)</param>
		/// <param name="updatedEmailDescriptions">Updated email descriptions (tab separated</param>
		public void UpdateEmailsAndDescriptions(string updatedEmails, string updatedEmailDescriptions)
		{
			this.emails = Encoding.UTF8.GetBytes(updatedEmails);
			this.emailDescriptions = Encoding.UTF8.GetBytes(updatedEmailDescriptions);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update emails and descriptions (both are arrays)
		/// </summary>
		/// <param name="updatedEmails">Updated emails (array)</param>
		/// <param name="updatedEmailDescriptions">Updated email descriptions (array</param>
		public void UpdateEmailsAndDescriptions(string[] updatedEmails, string[] updatedEmailDescriptions)
		{
			this.emails = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedEmails));
			this.emailDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedEmailDescriptions));

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update phone numbers and descriptions (both are tab separated)
		/// </summary>
		/// <param name="updatedPhoneNumbers">Updated phone numbers (tab separated)</param>
		/// <param name="updatedPhoneNumberDescriptions">Updated phone number descriptions (tab separated</param>
		public void UpdatePhoneNumbersAndDescriptions(string updatedPhoneNumbers, string updatedPhoneNumberDescriptions)
		{
			this.phoneNumbers = Encoding.UTF8.GetBytes(updatedPhoneNumbers);
			this.phoneNumberDescriptions = Encoding.UTF8.GetBytes(updatedPhoneNumberDescriptions);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update phone numbers and descriptions (both are arrays)
		/// </summary>
		/// <param name="updatedPhoneNumbers">Updated phone numbers (array)</param>
		/// <param name="updatedPhoneNumberDescriptions">Updated phone number descriptions (array)</param>
		public void UpdatePhoneNumbersAndDescriptions(string[] updatedPhoneNumbers, string[] updatedPhoneNumberDescriptions)
		{
			this.phoneNumbers = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedPhoneNumbers));
			this.phoneNumberDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedPhoneNumberDescriptions));

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update country
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCountry">Updated country</param>
		public void UpdateCountry(string updatedCountry)
		{
			this.country = Encoding.UTF8.GetBytes(updatedCountry);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update street address
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedStreetAddress">Updated street address</param>
		public void UpdateStreetAddress(string updatedStreetAddress)
		{
			this.streetAddress = Encoding.UTF8.GetBytes(updatedStreetAddress);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update street address additional
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedStreetAddressAdditional">Updated street address additional</param>
		public void UpdateStreetAddressAdditional(string updatedStreetAddressAdditional)
		{
			this.streetAddressAdditional = Encoding.UTF8.GetBytes(updatedStreetAddressAdditional);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update city
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCity">Updated city</param>
		public void UpdateCity(string updatedCity)
		{
			this.city = Encoding.UTF8.GetBytes(updatedCity);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update postal code
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPostalCode">Updated postal code</param>
		public void UpdatePostalCode(string updatedPostalCode)
		{
			this.postalCode = Encoding.UTF8.GetBytes(updatedPostalCode);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update PO Box
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPOBox">Updated PO Box</param>
		public void UpdatePOBox(string updatedPOBox)
		{
			this.poBox = Encoding.UTF8.GetBytes(updatedPOBox);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update birthday
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedBirthday">Updated birthday</param>
		public void UpdateBirthday(string updatedBirthday)
		{
			this.birthday = Encoding.UTF8.GetBytes(updatedBirthday);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update websites (tab separated)
		/// </summary>
		/// <param name="updatedWebsites">Updated websites (tab separated)</param>
		public void UpdateWebsites(string updatedWebsites)
		{
			this.websites = Encoding.UTF8.GetBytes(updatedWebsites);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update websites (array)
		/// </summary>
		/// <param name="updatedWebsites">Updated websites (array)</param>
		public void UpdateWebsites(string[] updatedWebsites)
		{
			this.websites = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedWebsites));

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update relationship
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedRelationship">Updated relationship</param>
		public void UpdateRelationship(string updatedRelationship)
		{
			this.relationship = Encoding.UTF8.GetBytes(updatedRelationship);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update notes
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNotes">Updated notes</param>
		public void UpdateNotes(string updatedNotes)
		{
			this.notes = Encoding.UTF8.GetBytes(updatedNotes);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update modification time from current UTC timestamp
		/// </summary>
		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		#endregion // Updates

		#region Getters

		/// <summary>
		/// Get first name
		/// </summary>
		/// <returns>First name as string</returns>
		public string GetFirstName()
		{
			return System.Text.Encoding.UTF8.GetString(this.firstName);
		}

		/// <summary>
		/// Get last name
		/// </summary>
		/// <returns>Last name as string</returns>
		public string GetLastName()
		{
			return System.Text.Encoding.UTF8.GetString(this.lastName);
		}

		/// <summary>
		/// Get middle name
		/// </summary>
		/// <returns>Middle name as string</returns>
		public string GetMiddleName()
		{
			return System.Text.Encoding.UTF8.GetString(this.middleName);
		}

		/// <summary>
		/// Get name prefix
		/// </summary>
		/// <returns>Name prefix as string</returns>
		public string GetNamePrefix()
		{
			return System.Text.Encoding.UTF8.GetString(this.namePrefix);
		}

		/// <summary>
		/// Get name suffix
		/// </summary>
		/// <returns>Name suffix as string</returns>
		public string GetNameSuffix()
		{
			return System.Text.Encoding.UTF8.GetString(this.nameSuffix);
		}

		/// <summary>
		/// Get nickname
		/// </summary>
		/// <returns>Nickname as string</returns>
		public string GetNickname()
		{
			return System.Text.Encoding.UTF8.GetString(this.nickname);
		}

		/// <summary>
		/// Get company
		/// </summary>
		/// <returns>Company as string</returns>
		public string GetCompany()
		{
			return System.Text.Encoding.UTF8.GetString(this.company);
		}

		/// <summary>
		/// Get job title
		/// </summary>
		/// <returns>Job title as string</returns>
		public string GetJobTitle()
		{
			return System.Text.Encoding.UTF8.GetString(this.jobTitle);
		}

		/// <summary>
		/// Get department
		/// </summary>
		/// <returns>Department as string</returns>
		public string GetDepartment()
		{
			return System.Text.Encoding.UTF8.GetString(this.department);
		}

		/// <summary>
		/// Get emails
		/// </summary>
		/// <returns>Emails as string</returns>
		public string GetEmails()
		{
			return System.Text.Encoding.UTF8.GetString(this.emails);
		}

		/// <summary>
		/// Get emails as string array
		/// </summary>
		/// <returns>Emails as string array</returns>
		public string[] GetEmailsArray()
		{
			return System.Text.Encoding.UTF8.GetString(this.emails).Split(separatorChar);
		}

		/// <summary>
		/// Get email descriptions
		/// </summary>
		/// <returns>Email descriptions as string</returns>
		public string GetEmailDescriptions()
		{
			return System.Text.Encoding.UTF8.GetString(this.emailDescriptions);
		}

		/// <summary>
		/// Get email descriptions as string array
		/// </summary>
		/// <returns>Email descriptions as string array</returns>
		public string[] GetEmailDescriptionsArray()
		{
			return System.Text.Encoding.UTF8.GetString(this.emailDescriptions).Split(separatorChar);
		}

		/// <summary>
		/// Get phone numbers
		/// </summary>
		/// <returns>Phone numbers as string</returns>
		public string GetPhoneNumbers()
		{
			return System.Text.Encoding.UTF8.GetString(this.phoneNumbers);
		}

		/// <summary>
		/// Get phone numbers as string array
		/// </summary>
		/// <returns>Phone numbers as string string</returns>
		public string[] GetPhoneNumbersArray()
		{
			return System.Text.Encoding.UTF8.GetString(this.phoneNumbers).Split(separatorChar);
		}

		/// <summary>
		/// Get phone number descriptions
		/// </summary>
		/// <returns>Phone number descriptions as string</returns>
		public string GetPhoneNumberDescriptions()
		{
			return System.Text.Encoding.UTF8.GetString(this.phoneNumberDescriptions);
		}

		/// <summary>
		/// Get phone number descriptions as string array
		/// </summary>
		/// <returns>Phone number descriptions as string</returns>
		public string[] GetPhoneNumberDescriptionsArray()
		{
			return System.Text.Encoding.UTF8.GetString(this.phoneNumberDescriptions).Split(separatorChar);
		}

		/// <summary>
		/// Get country
		/// </summary>
		/// <returns>Country as string</returns>
		public string GetCountry()
		{
			return System.Text.Encoding.UTF8.GetString(this.country);
		}

		/// <summary>
		/// Get street address
		/// </summary>
		/// <returns>Street address as string</returns>
		public string GetStreetAddress()
		{
			return System.Text.Encoding.UTF8.GetString(this.streetAddress);
		}

		/// <summary>
		/// Get street address additional
		/// </summary>
		/// <returns>Street address additional as string</returns>
		public string GetStreetAddressAdditional()
		{
			return System.Text.Encoding.UTF8.GetString(this.streetAddressAdditional);
		}

		/// <summary>
		/// Get postal code
		/// </summary>
		/// <returns>Postal code as string</returns>
		public string GetPostalCode()
		{
			return System.Text.Encoding.UTF8.GetString(this.postalCode);
		}

		/// <summary>
		/// Get city
		/// </summary>
		/// <returns>City as string</returns>
		public string GetCity()
		{
			return System.Text.Encoding.UTF8.GetString(this.city);
		}

		/// <summary>
		/// Get PO Box
		/// </summary>
		/// <returns>PO Box as string</returns>
		public string GetPOBox()
		{
			return System.Text.Encoding.UTF8.GetString(this.poBox);
		}

		/// <summary>
		/// Get birthday
		/// </summary>
		/// <returns>Birthday as string</returns>
		public string GetBirthday()
		{
			return System.Text.Encoding.UTF8.GetString(this.birthday);
		}

		/// <summary>
		/// Get websites
		/// </summary>
		/// <returns>Websites as string</returns>
		public string GetWebsites()
		{
			return System.Text.Encoding.UTF8.GetString(this.websites);
		}

		/// <summary>
		/// Get websites as string array
		/// </summary>
		/// <returns>Websites as string array</returns>
		public string[] GetWebsitesArray()
		{
			return System.Text.Encoding.UTF8.GetString(this.websites).Split(separatorChar);
		}

		/// <summary>
		/// Get relationship
		/// </summary>
		/// <returns>Relationship as string</returns>
		public string GetRelationship()
		{
			return System.Text.Encoding.UTF8.GetString(this.relationship);
		}

		/// <summary>
		/// Get notes
		/// </summary>
		/// <returns>Notes as string</returns>
		public string GetNotes()
		{
			return System.Text.Encoding.UTF8.GetString(this.notes);
		}

		/// <summary>
		/// Get creation time
		/// </summary>
		/// <returns>Creation time as DateTimeOffset</returns>
		public DateTimeOffset GetCreationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.creationTime);
		}

		/// <summary>
		/// Get modification time
		/// </summary>
		/// <returns>Modification time as DateTimeOffset</returns>
		public DateTimeOffset GetModificationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.modificationTime);
		}

		#endregion // Getters

		/// <summary>
		/// Get checksum as hex
		/// </summary>
		/// <returns>Hex string</returns>
		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		/// <summary>
		/// Check if checksum matches content
		/// </summary>
		/// <returns>True if matches; False otherwise</returns>
		public bool CheckIfChecksumMatchesContent()
		{
			return checksum == CalculateHexChecksum();
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.firstName, this.lastName, this.middleName, this.namePrefix, this.nameSuffix, this.nickname, 
														this.company, this.jobTitle, this.department, this.emails, this.emailDescriptions, this.phoneNumbers, this.phoneNumberDescriptions,
														this.country, this.streetAddress, this.streetAddressAdditional, this.postalCode, this.city, this.poBox,
														this.birthday, this.websites, this.relationship, this.notes,
														BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime));
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}
}