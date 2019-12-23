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
			Assert.Greater(csc.version, 0);
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
			var addResultSuccess1 = csc.AddLoginInformationSecret(password, ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);
			var addResultSuccess2 = csc.AddLoginInformationSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);

			var addResultFailure1 = csc.AddLoginInformationSecret(password, null, kdfeIdentifier);
			var addResultFailure2 = csc.AddLoginInformationSecret(password, ContentGenerator.GenerateRandomLoginInformation(), "not existing");
			var addResultFailure3 = csc.AddLoginInformationSecret("", ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

			Assert.IsFalse(addResultFailure1.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure1.possibleError));

			Assert.IsFalse(addResultFailure2.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure2.possibleError));

			Assert.IsFalse(addResultFailure3.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure3.possibleError));

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
			var addResultSuccess1 = csc.AddNoteSecret(password, ContentGenerator.GenerateRandomNote(), kdfeIdentifier);
			var addResultSuccess2 = csc.AddNoteSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomNote(), kdfeIdentifier);

			var addResultFailure1 = csc.AddNoteSecret(password, null, kdfeIdentifier);
			var addResultFailure2 = csc.AddNoteSecret(password, ContentGenerator.GenerateRandomNote(), "not existing");
			var addResultFailure3 = csc.AddNoteSecret("", ContentGenerator.GenerateRandomNote(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

			Assert.IsFalse(addResultFailure1.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure1.possibleError));

			Assert.IsFalse(addResultFailure2.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure2.possibleError));

			Assert.IsFalse(addResultFailure3.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure3.possibleError));

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
			var addResultSuccess1 = csc.AddFileEntrySecret(password, ContentGenerator.GenerateRandomFileEntry(), kdfeIdentifier);
			var addResultSuccess2 = csc.AddFileEntrySecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomFileEntry(), kdfeIdentifier);

			var addResultFailure1 = csc.AddFileEntrySecret(password, null, kdfeIdentifier);
			var addResultFailure2 = csc.AddFileEntrySecret(password, ContentGenerator.GenerateRandomFileEntry(), "not existing");
			var addResultFailure3 = csc.AddFileEntrySecret("", ContentGenerator.GenerateRandomFileEntry(), kdfeIdentifier);

			// Assert
			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

			Assert.IsFalse(addResultFailure1.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure1.possibleError));

			Assert.IsFalse(addResultFailure2.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure2.possibleError));

			Assert.IsFalse(addResultFailure3.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure3.possibleError));

			Assert.AreEqual(2, csc.fileSecrets.Count);
		}
	}
}