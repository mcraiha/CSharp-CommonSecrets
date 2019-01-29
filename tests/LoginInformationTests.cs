using NUnit.Framework;
using CSCommonSecrets;

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
			string checksum1 = li1.GetChecksumAsBase64();
			string checksum2 = li2.GetChecksumAsBase64();
			string checksum3 = li3.GetChecksumAsBase64();

			string updatedPassword = li3.password + "A";
			li3.UpdatePassword(updatedPassword);
			string checksum4 = li3.GetChecksumAsBase64();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}
	}
}