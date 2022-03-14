#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class ContactTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorSimpleTest()
		{
			// Arrange
			Contact c1 = new Contact();
			Contact c2 = new Contact("first", "last", "middle");

			// Act

			// Assert
			Assert.IsNotNull(c1);
			Assert.IsNotNull(c2);
		}

		[Test]
		public void ConstructorFullTest()
		{
			// Arrange
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
			Contact c1 = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);

			// Act

			// Assert
			Assert.IsNotNull(c1);
		}

		[Test]
		public void GetValuesTest()
		{
			// Arrange
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
			Contact c1 = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);


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
		public void UpdateTest()
		{
			// Arrange
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
			Contact c1 = new Contact("", "", "");


			// Act
			c1.UpdateFirstName(firstName);
			c1.UpdateLastName(lastName);
			c1.UpdateMiddleName(middleName);
			c1.UpdateNamePrefix(namePrefix);
			c1.UpdateNameSuffix(nameSuffix);
			c1.UpdateNickname(nickname);
			c1.UpdateCompany(company);
			c1.UpdateJobTitle(jobTitle);
			c1.UpdateDepartment(department);
			c1.UpdateEmailsAndDescriptions(emails, emailDescriptions);
			c1.UpdatePhoneNumbersAndDescriptions(phoneNumbers, phoneNumberDescriptions);
			c1.UpdateCountry(country);
			c1.UpdateStreetAddress(streetAddress);
			c1.UpdateStreetAddressAdditional(streetAddressAdditional);
			c1.UpdatePostalCode(postalCode);
			c1.UpdateCity(city);
			c1.UpdatePOBox(poBox);
			c1.UpdateBirthday(birthday);
			c1.UpdateRelationship(relationship);
			c1.UpdateNotes(notes);
			c1.UpdateWebsites(websites);

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
		public void ShallowCopyTest()
		{
			// Arrange
			Contact c1 = new Contact("first", "last", "middle");

			// Act
			Contact c2 = c1.ShallowCopy();

			string checksum1 = c1.GetChecksumAsHex();
			string checksum2 = c2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(c2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			Contact c1 = new Contact("first", "last", "middle");

			// Act
			Contact c2 = new Contact(c1);

			// Assert
			Assert.AreNotSame(c1.firstName, c2.firstName);
			CollectionAssert.AreEqual(c1.firstName, c2.firstName);

			Assert.AreNotSame(c1.middleName, c2.middleName);
			CollectionAssert.AreEqual(c1.middleName, c2.middleName);

			Assert.AreNotSame(c1.lastName, c2.lastName);
			CollectionAssert.AreEqual(c1.lastName, c2.lastName);

			Assert.AreEqual(c1.modificationTime, c2.modificationTime);
			Assert.AreEqual(c1.creationTime, c2.creationTime);

			Assert.AreEqual(c1.checksum, c2.checksum);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			Contact c1 = new Contact("first", "last", "middle");

			// Act
			string checksum1 = c1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(c1, Formatting.Indented);

			Contact c2 = JsonConvert.DeserializeObject<Contact>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, c2.GetChecksumAsHex());
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM