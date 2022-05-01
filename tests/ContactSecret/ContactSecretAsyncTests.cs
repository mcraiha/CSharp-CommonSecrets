#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using System.Threading;
using System.Collections.Generic;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class ContactSecretAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task ConstructorAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf3, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(testDictionary, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(cs);
			Assert.IsNotNull(cs.audalfData);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 4, 5, 6, 1, 82, 93, 102, 112, 120, 103, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0x13, 0xaa, 0xf5, 0x36, 0xbb, 0xf8, 0xbb, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

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
		public async Task GetContactAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			Contact contactCopy = await cs.GetContactAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(ComparisonHelper.AreContactsEqual(c1, contactCopy));
			Assert.AreEqual(c1.creationTime, contactCopy.creationTime);
			Assert.AreEqual(c1.modificationTime, contactCopy.modificationTime);
		}

		[Test]
		public async Task GetWithInvalidKeyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 90, 10, 11, 12, 13, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0x10, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			byte[] derivedKeyInvalid = Mutator.CreateMutatedByteArray(derivedKey);

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.ThrowsAsync<ArgumentNullException>(async () => await cs.GetFirstNameAsync(null, securityAsyncFunctions));
			Assert.ThrowsAsync<ArgumentException>(async () => await cs.GetFirstNameAsync(derivedKeyInvalid, securityAsyncFunctions));
		}

		[Test]
		public async Task GetSingleEntriesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 13, 11, 12, 13, 14, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act

			// Assert
			Assert.AreEqual(firstName, await cs.GetFirstNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(lastName, await cs.GetLastNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(middleName, await cs.GetMiddleNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(namePrefix, await cs.GetNamePrefixAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(nameSuffix, await cs.GetNameSuffixAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(nickname, await cs.GetNicknameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(company, await cs.GetCompanyAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(jobTitle, await cs.GetJobTitleAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(department, await cs.GetDepartmentAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, emails), await cs.GetEmailsAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, emailDescriptions), await cs.GetEmailDescriptionsAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, phoneNumbers), await cs.GetPhoneNumbersAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, phoneNumberDescriptions), await cs.GetPhoneNumberDescriptionsAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(emails, await cs.GetEmailsArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(emailDescriptions, await cs.GetEmailDescriptionsArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(phoneNumbers, await cs.GetPhoneNumbersArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(phoneNumberDescriptions, await cs.GetPhoneNumberDescriptionsArrayAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(country, await cs.GetCountryAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(streetAddress, await cs.GetStreetAddressAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(streetAddressAdditional, await cs.GetStreetAddressAdditionalAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(postalCode, await cs.GetPostalCodeAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(city, await cs.GetCityAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(poBox, await cs.GetPOBoxAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(birthday, await cs.GetBirthdayAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(relationship, await cs.GetRelationshipAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(notes, await cs.GetNotesAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(websites, await cs.GetWebsitesAsync(derivedKey, securityAsyncFunctions));
		}

		[Test]
		public async Task GetCreationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Contact contact = await Contact.CreateContactAsync("Super", "Hot", "Dragon", securityAsyncFunctions);
			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(contact, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			DateTimeOffset contactCreationTime = await cs.GetCreationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(contact.GetCreationTime(), contactCreationTime);
		}

		[Test]
		public async Task GetModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 15, 200, 3, 4, 15, 6, 7, 8, 9, 10, 11, 112, 139, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xfb, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xf3, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			Contact contact = await Contact.CreateContactAsync("Super", "Neat", "Dragon", securityAsyncFunctions);
			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(contact, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);
			
			// Act
			DateTimeOffset modificationTime1 = await cs.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);
			await Task.Delay(1100);
			await cs.SetFirstNameAsync("Better", derivedKey, securityAsyncFunctions);
			DateTimeOffset modificationTime2 = await cs.GetModificationTimeAsync(derivedKey, securityAsyncFunctions);

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public async Task GetKeyIdentifierAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 83, 91, 110, 221, 18, 213, 104, 15, 16 };
			byte[] initialCounter = new byte[] { 0xa0, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x59, 0x15, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			ContactSecret contactSecret = await ContactSecret.CreateContactSecretAsync(await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), keyIdentifier, skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(keyIdentifier, contactSecret.GetKeyIdentifier());
		}

		[Test]
		public async Task CanBeDecryptedWithDerivedPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey1 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 16 };
			byte[] derivedKey2 = new byte[16] { 111, 222, 31, 47, 75, 168, 78, 13, 61, 118, 221, 18, 213, 104, 15, 15 };
			byte[] initialCounter = new byte[] { 0xa7, 0xb1, 0xcb, 0xcd, 0xaa, 0xc5, 0xd3, 0xb5, 0x58, 0x51, 0x91, 0x2b, 0x33, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR);

			string keyIdentifier = "primary";

			// Act
			ContactSecret contactSecret = await ContactSecret.CreateContactSecretAsync(await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), keyIdentifier, skaAES_CTR, derivedKey1, securityAsyncFunctions);

			// Assert
			Assert.IsTrue(await contactSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey1, securityAsyncFunctions));
			Assert.IsFalse(await contactSecret.CanBeDecryptedWithDerivedPasswordAsync(null, securityAsyncFunctions));
			Assert.IsFalse(await contactSecret.CanBeDecryptedWithDerivedPasswordAsync(new byte[] {}, securityAsyncFunctions));
			Assert.IsFalse(await contactSecret.CanBeDecryptedWithDerivedPasswordAsync(derivedKey2, securityAsyncFunctions));
		}

		[Test]
		public async Task SetSingleEntriesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 1, 2, 3, 4, 3, 6, 4, 8, 9, 13, 11, 12, 13, 140, 15, 16 };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xd6, 0xf7, 0xf8, 0xf9, 0xfa, 0xf1, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = new Contact(await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions));

			ContactSecret cs = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			await cs.SetFirstNameAsync(firstName, derivedKey, securityAsyncFunctions);
			await cs.SetLastNameAsync(lastName, derivedKey, securityAsyncFunctions);
			await cs.SetMiddleNameAsync(middleName, derivedKey, securityAsyncFunctions);
			await cs.SetNamePrefixAsync(namePrefix, derivedKey, securityAsyncFunctions);
			await cs.SetNameSuffixAsync(nameSuffix, derivedKey, securityAsyncFunctions);
			await cs.SetNicknameAsync(nickname, derivedKey, securityAsyncFunctions);
			await cs.SetCompanyAsync(company, derivedKey, securityAsyncFunctions);
			await cs.SetJobTitleAsync(jobTitle, derivedKey, securityAsyncFunctions);
			await cs.SetDepartmentAsync(department, derivedKey, securityAsyncFunctions);
			await cs.SetEmailsAndDescriptionsAsync(emails, emailDescriptions, derivedKey, securityAsyncFunctions);
			await cs.SetPhoneNumbersAndDescriptionsAsync(phoneNumbers, phoneNumberDescriptions, derivedKey, securityAsyncFunctions);
			await cs.SetCountryAsync(country, derivedKey, securityAsyncFunctions);
			await cs.SetStreetAddressAsync(streetAddress, derivedKey, securityAsyncFunctions);
			await cs.SetStreetAddressAdditionalAsync(streetAddressAdditional, derivedKey, securityAsyncFunctions);
			await cs.SetPostalCodeAsync(postalCode, derivedKey, securityAsyncFunctions);
			await cs.SetCityAsync(city, derivedKey, securityAsyncFunctions);
			await cs.SetPOBoxAsync(poBox, derivedKey, securityAsyncFunctions);
			await cs.SetBirthdayAsync(birthday, derivedKey, securityAsyncFunctions);
			await cs.SetRelationshipAsync(relationship, derivedKey, securityAsyncFunctions);
			await cs.SetNotesAsync(notes, derivedKey, securityAsyncFunctions);
			await cs.SetWebsitesAsync(websites, derivedKey, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(firstName, await cs.GetFirstNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(lastName, await cs.GetLastNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(middleName, await cs.GetMiddleNameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(namePrefix, await cs.GetNamePrefixAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(nameSuffix, await cs.GetNameSuffixAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(nickname, await cs.GetNicknameAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(company, await cs.GetCompanyAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(jobTitle, await cs.GetJobTitleAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(department, await cs.GetDepartmentAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, emails), await cs.GetEmailsAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, emailDescriptions), await cs.GetEmailDescriptionsAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, phoneNumbers), await cs.GetPhoneNumbersAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(String.Join(Contact.separatorChar, phoneNumberDescriptions), await cs.GetPhoneNumberDescriptionsAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(emails, await cs.GetEmailsArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(emailDescriptions, await cs.GetEmailDescriptionsArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(phoneNumbers, await cs.GetPhoneNumbersArrayAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(phoneNumberDescriptions, await cs.GetPhoneNumberDescriptionsArrayAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(streetAddress, await cs.GetStreetAddressAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(country, await cs.GetCountryAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(streetAddress, await cs.GetStreetAddressAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(streetAddressAdditional, await cs.GetStreetAddressAdditionalAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(postalCode, await cs.GetPostalCodeAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(city, await cs.GetCityAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(poBox, await cs.GetPOBoxAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(birthday, await cs.GetBirthdayAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(relationship, await cs.GetRelationshipAsync(derivedKey, securityAsyncFunctions));
			Assert.AreEqual(notes, await cs.GetNotesAsync(derivedKey, securityAsyncFunctions));
			CollectionAssert.AreEqual(websites, await cs.GetWebsitesAsync(derivedKey, securityAsyncFunctions));
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

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
			Contact c1 = await Contact.CreateContactAsync(firstName, lastName, middleName, namePrefix, nameSuffix, nickname, company, jobTitle, department, 
										emails, emailDescriptions, phoneNumbers, phoneNumberDescriptions, 
										country, streetAddress, streetAddressAdditional, postalCode, city, poBox, birthday,
										websites, relationship, notes, securityAsyncFunctions);

			ContactSecret cs1 = await ContactSecret.CreateContactSecretAsync(c1, "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			string checksum1 = cs1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(cs1, Formatting.Indented);

			ContactSecret cs2 = JsonConvert.DeserializeObject<ContactSecret>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, cs2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] derivedKey = new byte[16] { 56, 2, 3, 4, 55, 76, 7, 8, 9, 10, 11, 12, 13, 14, 15, 255 };
			byte[] initialCounter = new byte[] { 0x00, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter, securityAsyncFunctions);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			ContactSecret contactSecret = await ContactSecret.CreateContactSecretAsync(await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), "does not matter", skaAES_CTR, derivedKey, securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await contactSecret.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			contactSecret.checksum = contactSecret.checksum.Remove(0, 1);
			bool shouldBeFalse = await contactSecret.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM