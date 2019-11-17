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
			string kdfeIdentifier = "just a random";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			// Act
			KeyDerivationFunctionEntry result = csc.FindKeyDerivationFunctionEntryWithKeyIdentifier(kdfeIdentifier);

			// Assert
			Assert.AreSame(kdfe, result);
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
			var addResult1 = csc.AddLoginInformationSecret(password, ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);
			var addResult2 = csc.AddLoginInformationSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResult1.success);
			Assert.AreEqual("", addResult1.possibleError);

			Assert.IsTrue(addResult2.success);
			Assert.AreEqual("", addResult2.possibleError);

			Assert.AreEqual(2, csc.loginInformationSecrets.Count);
		}

		[Test]
		public void AddNoteSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somethinghere";
			string password = "notdragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			
			// Act
			var addResult1 = csc.AddNoteSecret(password, ContentGenerator.GenerateRandomNote(), kdfeIdentifier);
			var addResult2 = csc.AddNoteSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomNote(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResult1.success);
			Assert.AreEqual("", addResult1.possibleError);

			Assert.IsTrue(addResult2.success);
			Assert.AreEqual("", addResult2.possibleError);

			Assert.AreEqual(2, csc.noteSecrets.Count);
		}

		[Test]
		public void AddFileEntrySecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somefile12344";
			string password = "notthatdragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			
			// Act
			var addResult = csc.AddFileEntrySecret(password, ContentGenerator.GenerateRandomFileEntry(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResult.success);
			Assert.AreEqual("", addResult.possibleError);
			Assert.AreEqual(1, csc.fileSecrets.Count);
		}
	}
}