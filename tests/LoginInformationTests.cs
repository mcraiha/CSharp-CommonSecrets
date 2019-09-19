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
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newUsername: "dragon", newPassword: "password1");

			// Act

			// Assert
			Assert.IsNotNull(li1);
			Assert.IsNotNull(li2);
		}

		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newUsername: "dragon", newPassword: "password1");			
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newUsername: "dragon", newPassword: "password1");

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
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newUsername: "dragon123", newPassword: "password13");
			
			// Act
			LoginInformation li2 = li1.ShallowCopy();

			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(li2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public void ChecksumSurvivesRoundtrip()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newUsername: "dragon123", newPassword: "password13");

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