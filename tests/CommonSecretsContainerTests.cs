using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;

namespace Tests
{
	public class CommonSecretsContainerTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void WithOneParameterTest()
		{
			// Arrange

			// Act

			// Assert

		}

		[Test]
		public void AddLoginInformationSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somethinghere";
			string password = "notdragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			
			// Act
			var addResult = csc.AddLoginInformationSecret(password, ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResult.success);
			Assert.AreEqual("", addResult.possibleError);
		}
	}

}