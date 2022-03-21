#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Tests
{
	public class ContactSecretTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf3, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			DateTimeOffset time = DateTimeOffset.UtcNow;

			Dictionary<string, object> testDictionary = new Dictionary<string, object>()
			{
				{ Contact.firstNameKey, "first" },
				{ Contact.lastNameKey, "last" },
				{ Contact.middleNameKey, "middle" },
				{ Contact.namePrefixKey, "prefix" },
				{ Contact.nameSuffixKey, "suffix" },
				{ Contact.nicknameKey, "nickname" },
				{ Contact.companyKey, "company" },
				{ Contact.jobTitleKey, "job title" },
				{ Contact.departmentKey, "department" },
				{ Contact.emailsKey, "something@somecompany.com" },
				{ Contact.emailDescriptionsKey, "work" },
				{ Contact.phoneNumbersKey, "123-568" },
				{ Contact.phoneNumberDescriptionsKey, "home" },
				{ Contact.countryKey, "country" },
				{ Contact.streetAddressKey, "address somewhere" },
				{ Contact.streetAddressAdditionalKey, "no addition" },
				{ Contact.postalCodeKey, "12365" },
				{ Contact.cityKey, "my city" },
				{ Contact.poBoxKey, "no po box here" },
				{ Contact.birthdayKey, "20.11.2000" },
				{ Contact.relationshipKey, "married" },
				{ Contact.notesKey, "no notes" },
				{ Contact.websitesKey, "https://somesite.com" },
				{ Contact.creationTimeKey, time },
				{ Contact.modificationTimeKey, time },
			};

			ContactSecret cs = new ContactSecret(testDictionary, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.IsNotNull(cs);
			Assert.IsNotNull(cs.audalfData);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 112, 120, 103, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0x13, 0xaa, 0xf5, 0x36, 0xbb, 0xf8, 0xbb, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string firstName = "Super11";
			string lastName = "Awesome22";
			string middleName = "Mega33";
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

			ContactSecret cs = new ContactSecret(c1, "does not matter", skaAES_CTR, derivedKey);

			// Act
			ContactSecret csCopy = new ContactSecret(cs);

			// Assert
			CollectionAssert.AreEqual(cs.audalfData, csCopy.audalfData, "AUDALF byte arrays should have same content");
			Assert.AreNotSame(cs.audalfData, csCopy.audalfData, "AUDALF byte arrays should be in different memory locations");
			CollectionAssert.AreEqual(cs.keyIdentifier, csCopy.keyIdentifier);
			Assert.AreNotSame(cs.keyIdentifier, csCopy.keyIdentifier, "Key identifier byte arrays should be in different memory locations");
			Assert.AreEqual(cs.checksum, csCopy.checksum);
		}

		[Test]
		public void GetContactTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string firstName = "Super110";
			string lastName = "Awesome220";
			string middleName = "Mega330";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIII";
			string nickname = "MegaDragon";
			string company = "EverDragons COTYERT";
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
			string notes = "Very awesome dragon again";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);

			ContactSecret cs = new ContactSecret(c1, "does not matter", skaAES_CTR, derivedKey);

			// Act
			Contact contactCopy = cs.GetContact(derivedKey);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreContactsEqual(c1, contactCopy));
			Assert.AreEqual(c1.creationTime, contactCopy.creationTime);
			Assert.AreEqual(c1.modificationTime, contactCopy.modificationTime);
		}

		[Test]
		public void GetSingleEntriesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string firstName = "Super2110";
			string lastName = "Awesome3220";
			string middleName = "Mega4330";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIXII";
			string nickname = "MegaDragon13";
			string company = "EverDragons COTYERT";
			string jobTitle = "Mid dragon";
			string department = "13rd Cave";
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
			string notes = "Very awesome dragon again";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);

			ContactSecret cs = new ContactSecret(c1, "does not matter", skaAES_CTR, derivedKey);

			// Act

			// Assert
			Assert.AreEqual(firstName, cs.GetFirstName(derivedKey));
			Assert.AreEqual(lastName, cs.GetLastName(derivedKey));
			Assert.AreEqual(middleName, cs.GetMiddleName(derivedKey));
			Assert.AreEqual(namePrefix, cs.GetNamePrefix(derivedKey));
			Assert.AreEqual(nameSuffix, cs.GetNameSuffix(derivedKey));
			Assert.AreEqual(nickname, cs.GetNickname(derivedKey));
			Assert.AreEqual(company, cs.GetCompany(derivedKey));
			Assert.AreEqual(jobTitle, cs.GetJobTitle(derivedKey));
			Assert.AreEqual(department, cs.GetDepartment(derivedKey));
			CollectionAssert.AreEqual(emails, cs.GetEmails(derivedKey));
			CollectionAssert.AreEqual(emailDescriptions, cs.GetEmailDescriptions(derivedKey));
			CollectionAssert.AreEqual(phoneNumbers, cs.GetPhoneNumbers(derivedKey));
			CollectionAssert.AreEqual(phoneNumberDescriptions, cs.GetPhoneNumberDescriptions(derivedKey));
			Assert.AreEqual(country, cs.GetCountry(derivedKey));
			Assert.AreEqual(streetAddress, cs.GetStreetAddress(derivedKey));
			Assert.AreEqual(streetAddressAdditional, cs.GetStreetAddressAdditional(derivedKey));
			Assert.AreEqual(postalCode, cs.GetPostalCode(derivedKey));
			Assert.AreEqual(city, cs.GetCity(derivedKey));
			Assert.AreEqual(poBox, cs.GetPOBox(derivedKey));
			Assert.AreEqual(birthday, cs.GetBirthday(derivedKey));
			Assert.AreEqual(relationship, cs.GetRelationship(derivedKey));
			Assert.AreEqual(notes, cs.GetNotes(derivedKey));
			CollectionAssert.AreEqual(websites, cs.GetWebsites(derivedKey));
		}

		[Test]
		public void GetKeyIdentifierTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x59, 0x15, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			ContactSecret contactSecret = new ContactSecret(ContentGeneratorSync.GenerateRandomContact(), keyIdentifier, skaAES_CTR, derivedKey);

			// Assert
			Assert.AreEqual(keyIdentifier, contactSecret.GetKeyIdentifier());
		}

		[Test]
		public void CanBeDecryptedWithDerivedPasswordTest()
		{
			byte[] derivedKey1 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x91, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			ContactSecret contactSecret = new ContactSecret(ContentGeneratorSync.GenerateRandomContact(), keyIdentifier, skaAES_CTR, derivedKey1);

			// Assert
			Assert.IsTrue(contactSecret.CanBeDecryptedWithDerivedPassword(derivedKey1));
			Assert.IsFalse(contactSecret.CanBeDecryptedWithDerivedPassword(null));
			Assert.IsFalse(contactSecret.CanBeDecryptedWithDerivedPassword(new byte[] {}));
			Assert.IsFalse(contactSecret.CanBeDecryptedWithDerivedPassword(derivedKey2));
		}

		[Test]
		public void SetSingleEntriesTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 3, 6, 4, 8, 9, 13, 11, 12, 13, 140, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xd6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string firstName = "Super2110";
			string lastName = "Aweso2me3220";
			string middleName = "Mega4330";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIXII";
			string nickname = "MegaDragon13";
			string company = "EverDragons COTYERT1";
			string jobTitle = "Mi3d dragon";
			string department = "13rd Cave";
			string[] emails = { "som24e@dragon663.com", "cooldra14gon123@dragons.com" };
			string[] emailDescriptions = { "work", "home" };
			string[] phoneNumbers = { "1234-123-123", "2344-234-234" };
			string[] phoneNumberDescriptions = { "work", "hotel" };
			string country = "drago4nland II";
			string streetAddress = "dragon street 122";
			string streetAddressAdditional = "no addition";
			string postalCode = "112345";
			string city = "dragvoncave";
			string poBox = "no po box";
			string birthday = "11-09-1697";
			string relationship = "single!";
			string notes = "Very awesome dragon again";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = new Contact(ContentGeneratorSync.GenerateRandomContact());

			ContactSecret cs = new ContactSecret(c1, "does not matter", skaAES_CTR, derivedKey);

			// Act
			cs.SetFirstName(firstName, derivedKey);
			cs.SetLastName(lastName, derivedKey);
			cs.SetMiddleName(middleName, derivedKey);
			cs.SetNamePrefix(namePrefix, derivedKey);
			cs.SetNameSuffix(nameSuffix, derivedKey);
			cs.SetNickname(nickname, derivedKey);
			cs.SetCompany(company, derivedKey);
			cs.SetJobTitle(jobTitle, derivedKey);
			cs.SetDepartment(department, derivedKey);
			cs.SetEmailsAndDescriptions(emails, emailDescriptions, derivedKey);
			cs.SetPhoneNumbersAndDescriptions(phoneNumbers, phoneNumberDescriptions, derivedKey);
			cs.SetCountry(country, derivedKey);
			cs.SetStreetAddress(streetAddress, derivedKey);
			cs.SetStreetAddressAdditional(streetAddressAdditional, derivedKey);
			cs.SetPostalCode(postalCode, derivedKey);
			cs.SetCity(city, derivedKey);
			cs.SetPOBox(poBox, derivedKey);
			cs.SetBirthday(birthday, derivedKey);
			cs.SetRelationship(relationship, derivedKey);
			cs.SetNotes(notes, derivedKey);
			cs.SetWebsites(websites, derivedKey);

			// Assert
			Assert.AreEqual(firstName, cs.GetFirstName(derivedKey));
			Assert.AreEqual(lastName, cs.GetLastName(derivedKey));
			Assert.AreEqual(middleName, cs.GetMiddleName(derivedKey));
			Assert.AreEqual(namePrefix, cs.GetNamePrefix(derivedKey));
			Assert.AreEqual(nameSuffix, cs.GetNameSuffix(derivedKey));
			Assert.AreEqual(nickname, cs.GetNickname(derivedKey));
			Assert.AreEqual(company, cs.GetCompany(derivedKey));
			Assert.AreEqual(jobTitle, cs.GetJobTitle(derivedKey));
			Assert.AreEqual(department, cs.GetDepartment(derivedKey));
			CollectionAssert.AreEqual(emails, cs.GetEmails(derivedKey));
			CollectionAssert.AreEqual(emailDescriptions, cs.GetEmailDescriptions(derivedKey));
			CollectionAssert.AreEqual(phoneNumbers, cs.GetPhoneNumbers(derivedKey));
			CollectionAssert.AreEqual(phoneNumberDescriptions, cs.GetPhoneNumberDescriptions(derivedKey));
			Assert.AreEqual(country, cs.GetCountry(derivedKey));
			Assert.AreEqual(streetAddress, cs.GetStreetAddress(derivedKey));
			Assert.AreEqual(streetAddressAdditional, cs.GetStreetAddressAdditional(derivedKey));
			Assert.AreEqual(postalCode, cs.GetPostalCode(derivedKey));
			Assert.AreEqual(city, cs.GetCity(derivedKey));
			Assert.AreEqual(poBox, cs.GetPOBox(derivedKey));
			Assert.AreEqual(birthday, cs.GetBirthday(derivedKey));
			Assert.AreEqual(relationship, cs.GetRelationship(derivedKey));
			Assert.AreEqual(notes, cs.GetNotes(derivedKey));
			CollectionAssert.AreEqual(websites, cs.GetWebsites(derivedKey));
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			string firstName = "Super1101";
			string lastName = "Awesome2202";
			string middleName = "Mega3303";
			string namePrefix = "Sirest";
			string nameSuffix = "IIIII";
			string nickname = "MegaDragon";
			string company = "EverDragons COTYERT";
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
			string notes = "Very awesome dragon again";
			string[] websites = { "https://dacoolastdragons4life.com", "https://nicevalleyvaults.net" };
			Contact c1 = new Contact(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes);

			ContactSecret cs1 = new ContactSecret(c1, "does not matter", skaAES_CTR, derivedKey);

			// Act
			string checksum1 = cs1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(cs1, Formatting.Indented);

			ContactSecret cs2 = JsonConvert.DeserializeObject<ContactSecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, cs2.GetChecksumAsHex());
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM