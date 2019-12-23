using System;
using System.Threading;
using NUnit.Framework;
using CSCommonSecrets;
using Newtonsoft.Json;

namespace Tests
{
	public class LoginInformationTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern");

			// Act

			// Assert
			Assert.IsNotNull(li1);
			Assert.IsNotNull(li2);
			Assert.IsNotNull(li3);
		}

		[Test]
		public void ModificationTimeTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");

			// Act
			DateTimeOffset modificationTime1 = li.GetModificationTime();
			Thread.Sleep(1100);
			li.UpdateUsername("dragon2");
			DateTimeOffset modificationTime2 = li.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto);			
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto);

			// Act
			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();
			string checksum3 = li3.GetChecksumAsHex();

			string updatedPassword = li3.password + "A";
			li3.UpdatePassword(updatedPassword);
			string checksum4 = li3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ShallowCopyTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13");
			
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern");

			// Act
			LoginInformation li2 = li1.ShallowCopy();

			LoginInformation li4 = li3.ShallowCopy();

			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();

			string checksum3 = li3.GetChecksumAsHex();
			string checksum4 = li4.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(li2);
			Assert.IsNotNull(li4);
			Assert.AreEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum4);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13");

			// Act
			string checksum1 = li1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(li1, Formatting.Indented);

			LoginInformation li2 = JsonConvert.DeserializeObject<LoginInformation>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, li2.GetChecksumAsHex());
		}
	}
}