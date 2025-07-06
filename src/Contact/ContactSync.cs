#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Text;

namespace CSCommonSecrets;

/// <summary>
/// Contact stores one plaintext (anyone can read) contact
/// </summary>
public sealed partial class Contact
{
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
	/// <param name="updatedEmailDescriptions">Updated email descriptions (tab separated)</param>
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
	/// <param name="updatedEmailDescriptions">Updated email descriptions (array)</param>
	public void UpdateEmailsAndDescriptions(string[] updatedEmails, string[] updatedEmailDescriptions)
	{
		this.UpdateEmailsAndDescriptions(string.Join(separatorString, updatedEmails), string.Join(separatorString, updatedEmailDescriptions));
	}

	/// <summary>
	/// Update phone numbers and descriptions (both are tab separated)
	/// </summary>
	/// <param name="updatedPhoneNumbers">Updated phone numbers (tab separated)</param>
	/// <param name="updatedPhoneNumberDescriptions">Updated phone number descriptions (tab separated)</param>
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

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM