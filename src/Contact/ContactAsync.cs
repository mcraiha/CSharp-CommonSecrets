#if ASYNC_WITH_CUSTOM

using System;
using System.Text;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Contact stores one plaintext (anyone can read) contact
	/// </summary>
	public sealed partial class Contact
	{
		/// <summary>
		/// Default small constructor for Contact, async
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<Contact> CreateContactAsync(string firstName, string lastName, string middleName, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateContactAsync(firstName, lastName, middleName, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Constructor (small) with creation time override, async
		/// </summary>
		/// <param name="firstName">First name</param>
		/// <param name="lastName">Last name</param>
		/// <param name="middleName">Middle name</param>
		/// <param name="time">Creation time</param>
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<Contact> CreateContactAsync(string firstName, string lastName, string middleName, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			Contact contact = new Contact();
			contact.firstName = Encoding.UTF8.GetBytes(firstName);
			contact.lastName = Encoding.UTF8.GetBytes(lastName);
			contact.middleName = Encoding.UTF8.GetBytes(middleName);
			contact.creationTime = time.ToUnixTimeSeconds();
			contact.modificationTime = time.ToUnixTimeSeconds();

			await contact.CalculateAndUpdateChecksumAsync(securityFunctions);

			return contact;
		}

		/// <summary>
		/// Constuctor (full), async
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
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<Contact> CreateContactAsync(string firstName, string lastName, string middleName, string namePrefix, string nameSuffix, string nickname, string company, string jobTitle, string department, string[] emails, string[] emailDescriptions, string[] phoneNumbers, string[] phoneNumberDescriptions, string country, string streetAddress, string streetAddressAdditional, string postalCode, string city, string poBox, string birthday, string[] websites, string relationship, string notes, ISecurityAsyncFunctions securityFunctions)
		{
			return await CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday, websites, relationship, notes, DateTimeOffset.UtcNow, securityFunctions);
		}

		/// <summary>
		/// Constuctor (full) with creation time override, async
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
		/// <param name="securityFunctions">Security functions</param>
		public static async Task<Contact> CreateContactAsync(string firstName, string lastName, string middleName, string namePrefix, string nameSuffix, string nickname, string company, string jobTitle, string department, string[] emails, string[] emailDescriptions, string[] phoneNumbers, string[] phoneNumberDescriptions, string country, string streetAddress, string streetAddressAdditional, string postalCode, string city, string poBox, string birthday, string[] websites, string relationship, string notes, DateTimeOffset time, ISecurityAsyncFunctions securityFunctions)
		{
			Contact contact = new Contact();

			contact.firstName = Encoding.UTF8.GetBytes(firstName);
			contact.lastName = Encoding.UTF8.GetBytes(lastName);
			contact.middleName = Encoding.UTF8.GetBytes(middleName);
			contact.namePrefix = Encoding.UTF8.GetBytes(namePrefix);
			contact.nameSuffix = Encoding.UTF8.GetBytes(nameSuffix);
			contact.nickname = Encoding.UTF8.GetBytes(nickname);
			contact.company = Encoding.UTF8.GetBytes(company);
			contact.jobTitle = Encoding.UTF8.GetBytes(jobTitle);
			contact.department = Encoding.UTF8.GetBytes(department);
			contact.emails = Encoding.UTF8.GetBytes(string.Join(separatorString, emails));
			contact.emailDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, emailDescriptions));
			contact.phoneNumbers = Encoding.UTF8.GetBytes(string.Join(separatorString, phoneNumbers));
			contact.phoneNumberDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, phoneNumberDescriptions));
			contact.country = Encoding.UTF8.GetBytes(country);
			contact.streetAddress = Encoding.UTF8.GetBytes(streetAddress);
			contact.streetAddressAdditional = Encoding.UTF8.GetBytes(streetAddressAdditional);
			contact.postalCode = Encoding.UTF8.GetBytes(postalCode);
			contact.city = Encoding.UTF8.GetBytes(city);
			contact.poBox = Encoding.UTF8.GetBytes(poBox);
			contact.birthday = Encoding.UTF8.GetBytes(birthday);
			contact.websites = Encoding.UTF8.GetBytes(string.Join(separatorString, websites));
			contact.relationship = Encoding.UTF8.GetBytes(relationship);
			contact.notes = Encoding.UTF8.GetBytes(notes);
			contact.creationTime = time.ToUnixTimeSeconds();
			contact.modificationTime = time.ToUnixTimeSeconds();

			await contact.CalculateAndUpdateChecksumAsync(securityFunctions);

			return contact;
		}

		#region Updates

		/// <summary>
		/// Update first name, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedFirstName">Updated first name</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateFirstNameAsync(string updatedFirstName, ISecurityAsyncFunctions securityFunctions)
		{
			this.firstName = Encoding.UTF8.GetBytes(updatedFirstName);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update last name, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedLastName">Updated last name</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateLastNameAsync(string updatedLastName, ISecurityAsyncFunctions securityFunctions)
		{
			this.lastName = Encoding.UTF8.GetBytes(updatedLastName);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update middle name, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedMiddleName">Updated middle name</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateMiddleNameAsync(string updatedMiddleName, ISecurityAsyncFunctions securityFunctions)
		{
			this.middleName = Encoding.UTF8.GetBytes(updatedMiddleName);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update name prefix, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNamePrefix">Updated name prefix</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNamePrefixAsync(string updatedNamePrefix, ISecurityAsyncFunctions securityFunctions)
		{
			this.namePrefix = Encoding.UTF8.GetBytes(updatedNamePrefix);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update name suffix, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNameSuffix">Updated name suffix</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNameSuffixAsync(string updatedNameSuffix, ISecurityAsyncFunctions securityFunctions)
		{
			this.nameSuffix = Encoding.UTF8.GetBytes(updatedNameSuffix);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update nickname, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNickname">Updated nickname</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNicknameAsync(string updatedNickname, ISecurityAsyncFunctions securityFunctions)
		{
			this.nickname = Encoding.UTF8.GetBytes(updatedNickname);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update company, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCompany">Updated company</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateCompanyAsync(string updatedCompany, ISecurityAsyncFunctions securityFunctions)
		{
			this.company = Encoding.UTF8.GetBytes(updatedCompany);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update job title, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedJobTitle">Updated job title</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateJobTitleAsync(string updatedJobTitle, ISecurityAsyncFunctions securityFunctions)
		{
			this.jobTitle = Encoding.UTF8.GetBytes(updatedJobTitle);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update department, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedDepartment">Updated department</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateDepartmentAsync(string updatedDepartment, ISecurityAsyncFunctions securityFunctions)
		{
			this.department = Encoding.UTF8.GetBytes(updatedDepartment);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update emails and descriptions (both are tab separated), async
		/// </summary>
		/// <param name="updatedEmails">Updated emails (tab separated)</param>
		/// <param name="updatedEmailDescriptions">Updated email descriptions (tab separated)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateEmailsAndDescriptionsAsync(string updatedEmails, string updatedEmailDescriptions, ISecurityAsyncFunctions securityFunctions)
		{
			this.emails = Encoding.UTF8.GetBytes(updatedEmails);
			this.emailDescriptions = Encoding.UTF8.GetBytes(updatedEmailDescriptions);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update emails and descriptions (both are arrays), async
		/// </summary>
		/// <param name="updatedEmails">Updated emails (array)</param>
		/// <param name="updatedEmailDescriptions">Updated email descriptions (array)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateEmailsAndDescriptionsAsync(string[] updatedEmails, string[] updatedEmailDescriptions, ISecurityAsyncFunctions securityFunctions)
		{
			this.emails = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedEmails));
			this.emailDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedEmailDescriptions));

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update phone numbers and descriptions (both are tab separated), async
		/// </summary>
		/// <param name="updatedPhoneNumbers">Updated phone numbers (tab separated)</param>
		/// <param name="updatedPhoneNumberDescriptions">Updated phone number descriptions (tab separated</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdatePhoneNumbersAndDescriptionsAsync(string updatedPhoneNumbers, string updatedPhoneNumberDescriptions, ISecurityAsyncFunctions securityFunctions)
		{
			this.phoneNumbers = Encoding.UTF8.GetBytes(updatedPhoneNumbers);
			this.phoneNumberDescriptions = Encoding.UTF8.GetBytes(updatedPhoneNumberDescriptions);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update phone numbers and descriptions (both are arrays), async
		/// </summary>
		/// <param name="updatedPhoneNumbers">Updated phone numbers (array)</param>
		/// <param name="updatedPhoneNumberDescriptions">Updated phone number descriptions (array)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdatePhoneNumbersAndDescriptionsAsync(string[] updatedPhoneNumbers, string[] updatedPhoneNumberDescriptions, ISecurityAsyncFunctions securityFunctions)
		{
			this.phoneNumbers = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedPhoneNumbers));
			this.phoneNumberDescriptions = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedPhoneNumberDescriptions));

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update country, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCountry">Updated country</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateCountryAsync(string updatedCountry, ISecurityAsyncFunctions securityFunctions)
		{
			this.country = Encoding.UTF8.GetBytes(updatedCountry);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update street address, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedStreetAddress">Updated street address</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateStreetAddressAsync(string updatedStreetAddress, ISecurityAsyncFunctions securityFunctions)
		{
			this.streetAddress = Encoding.UTF8.GetBytes(updatedStreetAddress);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update street address additional, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedStreetAddressAdditional">Updated street address additional</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateStreetAddressAdditionalAsync(string updatedStreetAddressAdditional, ISecurityAsyncFunctions securityFunctions)
		{
			this.streetAddressAdditional = Encoding.UTF8.GetBytes(updatedStreetAddressAdditional);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update city, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCity">Updated city</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateCityAsync(string updatedCity, ISecurityAsyncFunctions securityFunctions)
		{
			this.city = Encoding.UTF8.GetBytes(updatedCity);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update postal code, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPostalCode">Updated postal code</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdatePostalCodeAsync(string updatedPostalCode, ISecurityAsyncFunctions securityFunctions)
		{
			this.postalCode = Encoding.UTF8.GetBytes(updatedPostalCode);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update PO Box, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPOBox">Updated PO Box</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdatePOBoxAsync(string updatedPOBox, ISecurityAsyncFunctions securityFunctions)
		{
			this.poBox = Encoding.UTF8.GetBytes(updatedPOBox);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update birthday, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedBirthday">Updated birthday</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateBirthdayAsync(string updatedBirthday, ISecurityAsyncFunctions securityFunctions)
		{
			this.birthday = Encoding.UTF8.GetBytes(updatedBirthday);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update websites (tab separated), async
		/// </summary>
		/// <param name="updatedWebsites">Updated websites (tab separated)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateWebsitesAsync(string updatedWebsites, ISecurityAsyncFunctions securityFunctions)
		{
			this.websites = Encoding.UTF8.GetBytes(updatedWebsites);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update websites (array), async
		/// </summary>
		/// <param name="updatedWebsites">Updated websites (array)</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateWebsitesAsync(string[] updatedWebsites, ISecurityAsyncFunctions securityFunctions)
		{
			this.websites = Encoding.UTF8.GetBytes(string.Join(separatorString, updatedWebsites));

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update relationship, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedRelationship">Updated relationship</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateRelationshipAsync(string updatedRelationship, ISecurityAsyncFunctions securityFunctions)
		{
			this.relationship = Encoding.UTF8.GetBytes(updatedRelationship);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update notes, async
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNotes">Updated notes</param>
		/// <param name="securityFunctions">Security functions</param>
		public async Task UpdateNotesAsync(string updatedNotes, ISecurityAsyncFunctions securityFunctions)
		{
			this.notes = Encoding.UTF8.GetBytes(updatedNotes);

			this.UpdateModificationTime();

			await this.CalculateAndUpdateChecksumAsync(securityFunctions);
		}

		/// <summary>
		/// Update modification time from current UTC timestamp
		/// </summary>
		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		#endregion // Updates

		/// <summary>
		/// Check if checksum matches content
		/// </summary>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>True if matches; False otherwise</returns>
		public async Task<bool> CheckIfChecksumMatchesContentAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return checksum == await CalculateHexChecksumAsync(securityFunctions);
		}

		private async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, this.firstName, this.lastName, this.middleName, this.namePrefix, this.nameSuffix, this.nickname, 
														this.company, this.jobTitle, this.department, this.emails, this.emailDescriptions, this.phoneNumbers, this.phoneNumberDescriptions,
														this.country, this.streetAddress, this.streetAddressAdditional, this.postalCode, this.city, this.poBox,
														this.birthday, this.websites, this.relationship, this.notes,
														BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime));
		}

		private async Task CalculateAndUpdateChecksumAsync(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksumAsync(securityFunctions);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM