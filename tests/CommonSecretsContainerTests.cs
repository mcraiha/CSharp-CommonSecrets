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
		public void AddContactSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somefile122344";
			string password = "notth3atdragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			
			// Act
			var addResultSuccess1 = csc.AddContactSecret(password, ContentGenerator.GenerateRandomContact(), kdfeIdentifier);
			var addResultSuccess2 = csc.AddContactSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomContact(), kdfeIdentifier);

			var addResultFailure1 = csc.AddContactSecret(password, null, kdfeIdentifier);
			var addResultFailure2 = csc.AddContactSecret(password, ContentGenerator.GenerateRandomContact(), "not existing");
			var addResultFailure3 = csc.AddContactSecret("", ContentGenerator.GenerateRandomContact(), kdfeIdentifier);

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

			Assert.AreEqual(2, csc.contactSecrets.Count);
		}

		[Test]
		public void AddPaymentSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somekey122344";
			string password = "th3atdragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			
			// Act
			var addResultSuccess1 = csc.AddPaymentCardSecret(password, ContentGenerator.GenerateRandomPaymentCard(), kdfeIdentifier);
			var addResultSuccess2 = csc.AddPaymentCardSecret(kdfe.GeneratePasswordBytes(password), ContentGenerator.GenerateRandomPaymentCard(), kdfeIdentifier);

			var addResultFailure1 = csc.AddPaymentCardSecret(password, null, kdfeIdentifier);
			var addResultFailure2 = csc.AddPaymentCardSecret(password, ContentGenerator.GenerateRandomPaymentCard(), "not existing");
			var addResultFailure3 = csc.AddPaymentCardSecret("", ContentGenerator.GenerateRandomPaymentCard(), kdfeIdentifier);

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

			Assert.AreEqual(2, csc.paymentCardSecrets.Count);
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

		[Test]
		public void ReplaceNoteSecretTest()
		{
			// Arrange
			string kdfeIdentifier = "somet!%&hinghere";
			string password = "not()=dragon42";
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			Note add1 = ContentGenerator.GenerateRandomNote();
			Note add2 = ContentGenerator.GenerateRandomNote();

			Note replace1 = ContentGenerator.GenerateRandomNote();
			Note replace2 = ContentGenerator.GenerateRandomNote();
			
			// Act
			var addResultSuccess1 = csc.AddNoteSecret(password, add1, kdfeIdentifier);
			var addResultSuccess2 = csc.AddNoteSecret(kdfe.GeneratePasswordBytes(password), add2, kdfeIdentifier);

			var replaceResultSuccess1 = csc.ReplaceNoteSecret(0, password, replace1, kdfeIdentifier);
			var replaceResultSuccess2 = csc.ReplaceNoteSecret(1, kdfe.GeneratePasswordBytes(password), replace2, kdfeIdentifier);

			var replaceResultFailure1 = csc.ReplaceNoteSecret(0, password, null, kdfeIdentifier);
			var replaceResultFailure2 = csc.ReplaceNoteSecret(0, password, ContentGenerator.GenerateRandomNote(), "not existing");
			var replaceResultFailure3 = csc.ReplaceNoteSecret(0, "", ContentGenerator.GenerateRandomNote(), kdfeIdentifier);
			var replaceResultFailure4 = csc.ReplaceNoteSecret(-1, password, replace1, kdfeIdentifier);
			var replaceResultFailure5 = csc.ReplaceNoteSecret(2, password, replace1, kdfeIdentifier);

			// Assert
			Assert.AreNotEqual(add1.GetNoteTitle(), replace1.GetNoteTitle(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetNoteTitle(), replace2.GetNoteTitle(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetNoteTitle(), csc.noteSecrets[0].GetNoteTitle(kdfe.GeneratePasswordBytes(password)));
			Assert.AreEqual(replace2.GetNoteTitle(), csc.noteSecrets[1].GetNoteTitle(kdfe.GeneratePasswordBytes(password)));

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

			Assert.AreEqual(2, csc.noteSecrets.Count);
		}

		[Test]
		public void ReplaceFileEntrySecretTest()
		{
			// Arrange
			string kdfeIdentifier = ",.-somefile12344";
			string password = "--!#notthatdragon42";

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			FileEntry add1 = ContentGenerator.GenerateRandomFileEntry();
			FileEntry add2 = ContentGenerator.GenerateRandomFileEntry();

			FileEntry replace1 = ContentGenerator.GenerateRandomFileEntry();
			FileEntry replace2 = ContentGenerator.GenerateRandomFileEntry();
			
			// Act
			var addResultSuccess1 = csc.AddFileEntrySecret(password, add1, kdfeIdentifier);
			var addResultSuccess2 = csc.AddFileEntrySecret(kdfe.GeneratePasswordBytes(password), add2, kdfeIdentifier);

			var replaceResultSuccess1 = csc.ReplaceFileEntrySecret(0, password, replace1, kdfeIdentifier);
			var replaceResultSuccess2 = csc.ReplaceFileEntrySecret(1, kdfe.GeneratePasswordBytes(password), replace2, kdfeIdentifier, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = csc.ReplaceFileEntrySecret(0, password, null, kdfeIdentifier);
			var replaceResultFailure2 = csc.ReplaceFileEntrySecret(0, password, ContentGenerator.GenerateRandomFileEntry(), "not existing");
			var replaceResultFailure3 = csc.ReplaceFileEntrySecret(0, "", ContentGenerator.GenerateRandomFileEntry(), kdfeIdentifier);
			var replaceResultFailure4 = csc.ReplaceFileEntrySecret(-1, password, replace1, kdfeIdentifier);
			var replaceResultFailure5 = csc.ReplaceFileEntrySecret(2, password, replace1, kdfeIdentifier);

			// Assert
			Assert.AreNotEqual(add1.GetFilename(), replace1.GetFilename(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetFilename(), replace2.GetFilename(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetFilename(), csc.fileSecrets[0].GetFilename(kdfe.GeneratePasswordBytes(password)));
			Assert.AreEqual(replace2.GetFilename(), csc.fileSecrets[1].GetFilename(kdfe.GeneratePasswordBytes(password)));

			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

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

			Assert.AreEqual(2, csc.fileSecrets.Count);
		}

		[Test]
		public void ReplaceContactSecretTest()
		{
			// Arrange
			string kdfeIdentifier = ",.-4somefile12344";
			string password = "--!#nottha2tdragon42";

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			Contact add1 = ContentGenerator.GenerateRandomContact();
			Contact add2 = ContentGenerator.GenerateRandomContact();

			Contact replace1 = ContentGenerator.GenerateRandomContact();
			Contact replace2 = ContentGenerator.GenerateRandomContact();
			
			// Act
			var addResultSuccess1 = csc.AddContactSecret(password, add1, kdfeIdentifier);
			var addResultSuccess2 = csc.AddContactSecret(kdfe.GeneratePasswordBytes(password), add2, kdfeIdentifier);

			var replaceResultSuccess1 = csc.ReplaceContactSecret(0, password, replace1, kdfeIdentifier);
			var replaceResultSuccess2 = csc.ReplaceContactSecret(1, kdfe.GeneratePasswordBytes(password), replace2, kdfeIdentifier, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = csc.ReplaceContactSecret(0, password, null, kdfeIdentifier);
			var replaceResultFailure2 = csc.ReplaceContactSecret(0, password, ContentGenerator.GenerateRandomContact(), "not existing");
			var replaceResultFailure3 = csc.ReplaceContactSecret(0, "", ContentGenerator.GenerateRandomContact(), kdfeIdentifier);
			var replaceResultFailure4 = csc.ReplaceContactSecret(-1, password, replace1, kdfeIdentifier);
			var replaceResultFailure5 = csc.ReplaceContactSecret(2, password, replace1, kdfeIdentifier);

			// Assert
			Assert.AreNotEqual(add1.GetFirstName(), replace1.GetFirstName(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetFirstName(), replace2.GetFirstName(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetFirstName(), csc.contactSecrets[0].GetFirstName(kdfe.GeneratePasswordBytes(password)));
			Assert.AreEqual(replace2.GetFirstName(), csc.contactSecrets[1].GetFirstName(kdfe.GeneratePasswordBytes(password)));

			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

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

			Assert.AreEqual(2, csc.contactSecrets.Count);
		}

		[Test]
		public void ReplacePaymentSecretTest()
		{
			// Arrange
			string kdfeIdentifier = ",.-4key12344";
			string password = "--!#n,.ottha2tdragon42";

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry(kdfeIdentifier);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			PaymentCard add1 = ContentGenerator.GenerateRandomPaymentCard();
			PaymentCard add2 = ContentGenerator.GenerateRandomPaymentCard();

			PaymentCard replace1 = ContentGenerator.GenerateRandomPaymentCard();
			PaymentCard replace2 = ContentGenerator.GenerateRandomPaymentCard();
			
			// Act
			var addResultSuccess1 = csc.AddPaymentCardSecret(password, add1, kdfeIdentifier);
			var addResultSuccess2 = csc.AddPaymentCardSecret(kdfe.GeneratePasswordBytes(password), add2, kdfeIdentifier);

			var replaceResultSuccess1 = csc.ReplacePaymentCardSecret(0, password, replace1, kdfeIdentifier);
			var replaceResultSuccess2 = csc.ReplacePaymentCardSecret(1, kdfe.GeneratePasswordBytes(password), replace2, kdfeIdentifier, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = csc.ReplacePaymentCardSecret(0, password, null, kdfeIdentifier);
			var replaceResultFailure2 = csc.ReplacePaymentCardSecret(0, password, ContentGenerator.GenerateRandomPaymentCard(), "not existing");
			var replaceResultFailure3 = csc.ReplacePaymentCardSecret(0, "", ContentGenerator.GenerateRandomPaymentCard(), kdfeIdentifier);
			var replaceResultFailure4 = csc.ReplacePaymentCardSecret(-1, password, replace1, kdfeIdentifier);
			var replaceResultFailure5 = csc.ReplacePaymentCardSecret(2, password, replace1, kdfeIdentifier);

			// Assert
			Assert.AreNotEqual(add1.GetTitle(), replace1.GetTitle(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetTitle(), replace2.GetTitle(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetTitle(), csc.paymentCardSecrets[0].GetTitle(kdfe.GeneratePasswordBytes(password)));
			Assert.AreEqual(replace2.GetTitle(), csc.paymentCardSecrets[1].GetTitle(kdfe.GeneratePasswordBytes(password)));

			Assert.IsTrue(addResultSuccess1.success);
			Assert.AreEqual("", addResultSuccess1.possibleError);

			Assert.IsTrue(addResultSuccess2.success);
			Assert.AreEqual("", addResultSuccess2.possibleError);

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

			Assert.AreEqual(2, csc.paymentCardSecrets.Count);
		}
	}
}