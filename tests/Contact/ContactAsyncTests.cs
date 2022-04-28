#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class ContactAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task ConstructorSimpleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = new Contact();
			Contact c2 = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(c1);
			Assert.IsNotNull(c2);
		}

		[Test]
		public async Task ConstructorFullAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string firstName = "first";
			string lastName = "last";
			string middleName = "middle";
			string namePrefix = "Sir";
			string nameSuffix = "III";
			string nickname = "LasterDragon";
			string company = "EverDragons";
			string jobTitle = "Top dragon";
			string department = "Vault";
			string[] emails = { "some@dragon663.com", "cooldragon123@dragons.com" };
			string[] emailDescriptions = { "work", "home" };
			string[] phoneNumbers = { "123-123-123", "234-234-234" };
			string[] phoneNumberDescriptions = { "work", "hotel" };
			string country = "dragonland";
			string streetAddress = "dragon street 12";
			string streetAddressAdditional = "no addition";
			string postalCode = "123456";
			string city = "dragoncity";
			string poBox = "no po box";
			string birthday = "13-09-1687";
			string relationship = "single";
			string notes = "Very cool dragon";
			string[] websites = { "https://dacoolastdragonsforlife.com", "https://nicevalleyvaults.net" };
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(c1);
		}

		[Test]
		public async Task ModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact contact = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act
			DateTimeOffset modificationTime1 = contact.GetModificationTime();
			await Task.Delay(1100);
			await contact.UpdateFirstNameAsync("dragon", securityAsyncFunctions);
			DateTimeOffset modificationTime2 = contact.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public async Task GetValuesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string firstName = "Super";
			string lastName = "Awesome";
			string middleName = "Mega";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIII";
			string nickname = "MegaDragon";
			string company = "EverDragons CO";
			string jobTitle = "Mid dragon";
			string department = "Cave";
			string[] emails = { "som24e@dragon663.com", "cooldra14gon123@dragons.com" };
			string[] emailDescriptions = { "work", "home" };
			string[] phoneNumbers = { "1234-123-123", "2344-234-234" };
			string[] phoneNumberDescriptions = { "work", "hotel" };
			string country = "dragonland II";
			string streetAddress = "dragon street 122";
			string streetAddressAdditional = "no addition";
			string postalCode = "12345";
			string city = "dragoncave";
			string poBox = "no po box";
			string birthday = "11-09-1697";
			string relationship = "single";
			string notes = "Very awesome dragon";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);


			// Act

			// Assert
			Assert.AreEqual(firstName, c1.GetFirstName());
			Assert.AreEqual(lastName, c1.GetLastName());
			Assert.AreEqual(middleName, c1.GetMiddleName());
			Assert.AreEqual(namePrefix, c1.GetNamePrefix());
			Assert.AreEqual(nameSuffix, c1.GetNameSuffix());
			Assert.AreEqual(nickname, c1.GetNickname());
			Assert.AreEqual(company, c1.GetCompany());
			Assert.AreEqual(jobTitle, c1.GetJobTitle());
			Assert.AreEqual(department, c1.GetDepartment());
			CollectionAssert.AreEqual(emails, c1.GetEmailsArray());
			CollectionAssert.AreEqual(emailDescriptions, c1.GetEmailDescriptionsArray());
			CollectionAssert.AreEqual(phoneNumbers, c1.GetPhoneNumbersArray());
			CollectionAssert.AreEqual(phoneNumberDescriptions, c1.GetPhoneNumberDescriptionsArray());
			Assert.AreEqual(country, c1.GetCountry());
			Assert.AreEqual(streetAddress, c1.GetStreetAddress());
			Assert.AreEqual(streetAddressAdditional, c1.GetStreetAddressAdditional());
			Assert.AreEqual(postalCode, c1.GetPostalCode());
			Assert.AreEqual(city, c1.GetCity());
			Assert.AreEqual(poBox, c1.GetPOBox());
			Assert.AreEqual(birthday, c1.GetBirthday());
			Assert.AreEqual(relationship, c1.GetRelationship());
			Assert.AreEqual(notes, c1.GetNotes());
			CollectionAssert.AreEqual(websites, c1.GetWebsitesArray());
		}

		[Test]
		public async Task UpdateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string firstName = "Super";
			string lastName = "Awesome";
			string middleName = "Mega";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIII";
			string nickname = "MegaDragon";
			string company = "EverDragons CO";
			string jobTitle = "Mid dragon";
			string department = "Cave";
			string[] emails = { "som24e@dragon663.com", "cooldra14gon123@dragons.com" };
			string[] emailDescriptions = { "work", "home" };
			string[] phoneNumbers = { "1234-123-123", "2344-234-234" };
			string[] phoneNumberDescriptions = { "work", "hotel" };
			string country = "dragonland II";
			string streetAddress = "dragon street 122";
			string streetAddressAdditional = "no addition";
			string postalCode = "12345";
			string city = "dragoncave";
			string poBox = "no po box";
			string birthday = "11-09-1697";
			string relationship = "single";
			string notes = "Very awesome dragon";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = await Contact.CreateContactAsync("", "", "", securityAsyncFunctions);


			// Act
			await c1.UpdateFirstNameAsync(firstName, securityAsyncFunctions);
			await c1.UpdateLastNameAsync(lastName, securityAsyncFunctions);
			await c1.UpdateMiddleNameAsync(middleName, securityAsyncFunctions);
			await c1.UpdateNamePrefixAsync(namePrefix, securityAsyncFunctions);
			await c1.UpdateNameSuffixAsync(nameSuffix, securityAsyncFunctions);
			await c1.UpdateNicknameAsync(nickname, securityAsyncFunctions);
			await c1.UpdateCompanyAsync(company, securityAsyncFunctions);
			await c1.UpdateJobTitleAsync(jobTitle, securityAsyncFunctions);
			await c1.UpdateDepartmentAsync(department, securityAsyncFunctions);
			await c1.UpdateEmailsAndDescriptionsAsync(emails, emailDescriptions, securityAsyncFunctions);
			await c1.UpdatePhoneNumbersAndDescriptionsAsync(phoneNumbers, phoneNumberDescriptions, securityAsyncFunctions);
			await c1.UpdateCountryAsync(country, securityAsyncFunctions);
			await c1.UpdateStreetAddressAsync(streetAddress, securityAsyncFunctions);
			await c1.UpdateStreetAddressAdditionalAsync(streetAddressAdditional, securityAsyncFunctions);
			await c1.UpdatePostalCodeAsync(postalCode, securityAsyncFunctions);
			await c1.UpdateCityAsync(city, securityAsyncFunctions);
			await c1.UpdatePOBoxAsync(poBox, securityAsyncFunctions);
			await c1.UpdateBirthdayAsync(birthday, securityAsyncFunctions);
			await c1.UpdateRelationshipAsync(relationship, securityAsyncFunctions);
			await c1.UpdateNotesAsync(notes, securityAsyncFunctions);
			await c1.UpdateWebsitesAsync(websites, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(firstName, c1.GetFirstName());
			Assert.AreEqual(lastName, c1.GetLastName());
			Assert.AreEqual(middleName, c1.GetMiddleName());
			Assert.AreEqual(namePrefix, c1.GetNamePrefix());
			Assert.AreEqual(nameSuffix, c1.GetNameSuffix());
			Assert.AreEqual(nickname, c1.GetNickname());
			Assert.AreEqual(company, c1.GetCompany());
			Assert.AreEqual(jobTitle, c1.GetJobTitle());
			Assert.AreEqual(department, c1.GetDepartment());
			CollectionAssert.AreEqual(emails, c1.GetEmailsArray());
			CollectionAssert.AreEqual(emailDescriptions, c1.GetEmailDescriptionsArray());
			CollectionAssert.AreEqual(phoneNumbers, c1.GetPhoneNumbersArray());
			CollectionAssert.AreEqual(phoneNumberDescriptions, c1.GetPhoneNumberDescriptionsArray());
			Assert.AreEqual(country, c1.GetCountry());
			Assert.AreEqual(streetAddress, c1.GetStreetAddress());
			Assert.AreEqual(streetAddressAdditional, c1.GetStreetAddressAdditional());
			Assert.AreEqual(postalCode, c1.GetPostalCode());
			Assert.AreEqual(city, c1.GetCity());
			Assert.AreEqual(poBox, c1.GetPOBox());
			Assert.AreEqual(birthday, c1.GetBirthday());
			Assert.AreEqual(relationship, c1.GetRelationship());
			Assert.AreEqual(notes, c1.GetNotes());
			CollectionAssert.AreEqual(websites, c1.GetWebsitesArray());
		}

		[Test, Description("Make sure both overloads work identically")]
		public async Task UpdateEmailsAndDescriptionsOverloadAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = await Contact.CreateContactAsync("", "", "", securityAsyncFunctions);
			Contact c2 = await Contact.CreateContactAsync("", "", "", securityAsyncFunctions);

			string[] emailsArray = { "som24e@dragon663.com", "cooldra14gon123@dragons.com" };
			string emails = string.Join(Contact.separatorString, emailsArray);

			string[] emailDescriptionsArray = { "work", "home" };
			string emailDescriptions = string.Join(Contact.separatorString, emailDescriptionsArray);

			// Act
			await c1.UpdateEmailsAndDescriptionsAsync(emailsArray, emailDescriptionsArray, securityAsyncFunctions);
			await c2.UpdateEmailsAndDescriptionsAsync(emails, emailDescriptions, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(emailsArray.Length, c1.GetEmailsArray().Length);
			CollectionAssert.AreEqual(c1.GetEmailsArray(), c2.GetEmailsArray());
			CollectionAssert.AreEqual(c1.GetEmailDescriptionsArray(), c2.GetEmailDescriptionsArray());
		}

		[Test]
		public async Task ShallowCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act
			Contact c2 = c1.ShallowCopy();

			string checksum1 = c1.GetChecksumAsHex();
			string checksum2 = c2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(c2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act
			Contact c2 = new Contact(c1);

			// Assert
			Assert.AreNotSame(c1.firstName, c2.firstName);
			Assert.AreEqual(c1.firstName, c2.firstName);

			Assert.AreNotSame(c1.middleName, c2.middleName);
			Assert.AreEqual(c1.middleName, c2.middleName);

			Assert.AreNotSame(c1.lastName, c2.lastName);
			Assert.AreEqual(c1.lastName, c2.lastName);

			Assert.AreEqual(c1.modificationTime, c2.modificationTime);
			Assert.AreEqual(c1.creationTime, c2.creationTime);

			Assert.AreEqual(c1.checksum, c2.checksum);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act
			string checksum1 = c1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(c1, Formatting.Indented);

			Contact c2 = JsonConvert.DeserializeObject<Contact>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, c2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			Contact c1 = await Contact.CreateContactAsync("first", "last", "middle", securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await c1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			c1.checksum = c1.checksum.Remove(0, 1);
			bool shouldBeFalse = await c1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM