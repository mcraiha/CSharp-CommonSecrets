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

		[Test]
		public void ReplaceLoginInformationSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somethingheremuch";
			string password = "notdragon42224!";

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			LoginInformation add1 = ContentGenerator.GenerateRandomLoginInformation();
			LoginInformation add2 = ContentGenerator.GenerateRandomLoginInformation();

			LoginInformation replace1 = ContentGenerator.GenerateRandomLoginInformation();
			LoginInformation replace2 = ContentGenerator.GenerateRandomLoginInformation();
			
			// Act
			var addResultSuccess1 = csc.AddLoginInformationSecret(password, add1, kdfeIdentifier);
			var addResultSuccess2 = csc.AddLoginInformationSecret(kdfe.GeneratePasswordBytes(password), add2, kdfeIdentifier);

			var replaceResultSuccess1 = csc.ReplaceLoginInformationSecret(0, password, replace1, kdfeIdentifier);
			var replaceResultSuccess2 = csc.ReplaceLoginInformationSecret(1, kdfe.GeneratePasswordBytes(password), replace2, kdfeIdentifier);

			var replaceResultFailure1 = csc.ReplaceLoginInformationSecret(0, password, null, kdfeIdentifier);
			var replaceResultFailure2 = csc.ReplaceLoginInformationSecret(0, password, ContentGenerator.GenerateRandomLoginInformation(), "not existing");
			var replaceResultFailure3 = csc.ReplaceLoginInformationSecret(0, "", ContentGenerator.GenerateRandomLoginInformation(), kdfeIdentifier);
			var replaceResultFailure4 = csc.ReplaceLoginInformationSecret(-1, password, replace1, kdfeIdentifier);
			var replaceResultFailure5 = csc.ReplaceLoginInformationSecret(2, password, replace1, kdfeIdentifier);

			// Assert
			Assert.AreNotEqual(add1.GetURL(), replace1.GetURL(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetURL(), replace2.GetURL(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetURL(), csc.loginInformationSecrets[0].GetURL(kdfe.GeneratePasswordBytes(password)));
			Assert.AreEqual(replace2.GetURL(), csc.loginInformationSecrets[1].GetURL(kdfe.GeneratePasswordBytes(password)));

			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

			Assert.IsTrue(replaceResultSuccess1.success);
			Assert.AreEqual("", replaceResultSuccess1.possibleError);

			Assert.IsTrue(replaceResultSuccess2.success);
			Assert.AreEqual("", replaceResultSuccess2.possibleError);

			Assert.IsFalse(replaceResultFailure1.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure1.possibleError));

			Assert.IsFalse(replaceResultFailure2.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure2.possibleError));

			Assert.IsFalse(replaceResultFailure3.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure3.possibleError));

			Assert.IsFalse(replaceResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure4.possibleError));

			Assert.IsFalse(replaceResultFailure5.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure5.possibleError));

			Assert.AreEqual(2, csc.loginInformationSecrets.Count);
		}
	}
}