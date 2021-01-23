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