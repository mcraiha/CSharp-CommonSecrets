#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using System.Collections.Generic;

using System.Threading.Tasks;

namespace Tests
{
	public class CommonSecretsContainerAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task WithOneParameterAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "just a random";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			// Act
			KeyDerivationFunctionEntry result = csc.FindKeyDerivationFunctionEntryWithKeyIdentifier(kdfeIdentifier);

			// Assert
			Assert.AreSame(kdfe, result);
			Assert.Greater(csc.version, 0);
		}

		[Test]
		public async Task GetKeyDerivationFunctionEntryIdentifiersAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier1 = "no matter";
			KeyDerivationFunctionEntry kdfe1 = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier1, securityAsyncFunctions);

			string kdfeIdentifier2 = "yet another";
			KeyDerivationFunctionEntry kdfe2 = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier2, securityAsyncFunctions);

			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe1);

			// Act
			csc.keyDerivationFunctionEntries.Add(kdfe2);
			List<string> identifiers = new List<string>(csc.GetKeyDerivationFunctionEntryIdentifiers());

			// Assert
			Assert.AreEqual(2, identifiers.Count);
			CollectionAssert.Contains(kdfeIdentifier1, identifiers);
			CollectionAssert.Contains(kdfeIdentifier2, identifiers);
		}

		[Test]
		public async Task AddLoginInformationSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somethinghere";
			string password = "notdragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddLoginInformationSecretAsync(password, await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddLoginInformationSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddLoginInformationSecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddLoginInformationSecretAsync(password, await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddLoginInformationSecretAsync("", await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddLoginInformationSecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.loginInformationSecrets.Count);
		}

		[Test]
		public async Task AddNoteSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somethinghere";
			string password = "notdragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddNoteSecretAsync(password, await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddNoteSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddNoteSecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddNoteSecretAsync(password, await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddNoteSecretAsync("", await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddNoteSecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.noteSecrets.Count);
		}

		[Test]
		public async Task AddFileEntrySecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somefile12344";
			string password = "notthatdragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddFileEntrySecretAsync(password, await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddFileEntrySecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddFileEntrySecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddFileEntrySecretAsync(password, await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddFileEntrySecretAsync("", await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddFileEntrySecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.fileSecrets.Count);
		}

		[Test]
		public async Task AddContactSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somefile122344";
			string password = "notth3atdragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddContactSecretAsync(password, await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddContactSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddContactSecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddContactSecretAsync(password, await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddContactSecretAsync("", await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddContactSecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.contactSecrets.Count);
		}

		[Test]
		public async Task AddPaymentSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somekey122344";
			string password = "th3atdragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddPaymentCardSecretAsync(password, await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddPaymentCardSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddPaymentCardSecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddPaymentCardSecretAsync(password, await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddPaymentCardSecretAsync("", await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddPaymentCardSecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.paymentCardSecrets.Count);
		}

		[Test]
		public async Task AddHistoryAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somekey122344";
			string password = "th3atd9849#%ragon42";
			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);
			byte[] nullArray = null;
			
			// Act
			var addResultSuccess1 = await csc.AddHistorySecretAsync(password, await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddHistorySecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

			var addResultFailure1 = await csc.AddHistorySecretAsync(password, null, kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure2 = await csc.AddHistorySecretAsync(password, await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var addResultFailure3 = await csc.AddHistorySecretAsync("", await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var addResultFailure4 = await csc.AddHistorySecretAsync(nullArray, await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);

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

			Assert.IsFalse(addResultFailure4.success);
			Assert.IsFalse(string.IsNullOrEmpty(addResultFailure4.possibleError));

			Assert.AreEqual(2, csc.historySecrets.Count);
		}


		[Test]
		public async Task ReplaceLoginInformationSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somethingheremuch";
			string password = "notdragon42224!";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			LoginInformation add1 = await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions);
			LoginInformation add2 = await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions);

			LoginInformation replace1 = await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions);
			LoginInformation replace2 = await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddLoginInformationSecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddLoginInformationSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplaceLoginInformationSecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplaceLoginInformationSecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultFailure1 = await csc.ReplaceLoginInformationSecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplaceLoginInformationSecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplaceLoginInformationSecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomLoginInformationAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplaceLoginInformationSecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplaceLoginInformationSecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplaceLoginInformationSecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplaceLoginInformationSecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetURL(), replace1.GetURL(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetURL(), replace2.GetURL(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetURL(), await csc.loginInformationSecrets[0].GetURLAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetURL(), await csc.loginInformationSecrets[1].GetURLAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.loginInformationSecrets.Count);
		}

		[Test]
		public async Task ReplaceNoteSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = "somet!%&hinghere";
			string password = "not()=dragon42";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			Note add1 = await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions);
			Note add2 = await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions);

			Note replace1 = await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions);
			Note replace2 = await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddNoteSecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddNoteSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplaceNoteSecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplaceNoteSecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultFailure1 = await csc.ReplaceNoteSecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplaceNoteSecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplaceNoteSecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomNoteAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplaceNoteSecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplaceNoteSecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplaceNoteSecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplaceNoteSecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetNoteTitle(), replace1.GetNoteTitle(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetNoteTitle(), replace2.GetNoteTitle(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetNoteTitle(), await csc.noteSecrets[0].GetNoteTitleAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetNoteTitle(), await csc.noteSecrets[1].GetNoteTitleAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.noteSecrets.Count);
		}

		[Test]
		public async Task ReplaceFileEntrySecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = ",.-somefile12344";
			string password = "--!#notthatdragon42";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			FileEntry add1 = await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions);
			FileEntry add2 = await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions);

			FileEntry replace1 = await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions);
			FileEntry replace2 = await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddFileEntrySecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddFileEntrySecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplaceFileEntrySecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplaceFileEntrySecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = await csc.ReplaceFileEntrySecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplaceFileEntrySecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplaceFileEntrySecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomFileEntryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplaceFileEntrySecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplaceFileEntrySecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplaceFileEntrySecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplaceFileEntrySecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetFilename(), replace1.GetFilename(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetFilename(), replace2.GetFilename(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetFilename(), await csc.fileSecrets[0].GetFilenameAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetFilename(), await csc.fileSecrets[1].GetFilenameAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.fileSecrets.Count);
		}

		[Test]
		public async Task ReplaceContactSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = ",.-4somefile12344";
			string password = "--!#nottha2tdragon42";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			Contact add1 = await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions);
			Contact add2 = await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions);

			Contact replace1 = await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions);
			Contact replace2 = await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddContactSecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddContactSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplaceContactSecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplaceContactSecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = await csc.ReplaceContactSecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplaceContactSecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplaceContactSecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomContactAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplaceContactSecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplaceContactSecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplaceContactSecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplaceContactSecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetFirstName(), replace1.GetFirstName(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetFirstName(), replace2.GetFirstName(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetFirstName(), await csc.contactSecrets[0].GetFirstNameAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetFirstName(), await csc.contactSecrets[1].GetFirstNameAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.contactSecrets.Count);
		}

		[Test]
		public async Task ReplacePaymentSecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = ",.-4key12344";
			string password = "--!#n,.ottha2tdragon42";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			PaymentCard add1 = await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions);
			PaymentCard add2 = await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions);

			PaymentCard replace1 = await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions);
			PaymentCard replace2 = await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddPaymentCardSecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddPaymentCardSecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplacePaymentCardSecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplacePaymentCardSecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = await csc.ReplacePaymentCardSecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplacePaymentCardSecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplacePaymentCardSecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomPaymentCardAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplacePaymentCardSecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplacePaymentCardSecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplacePaymentCardSecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplacePaymentCardSecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetTitle(), replace1.GetTitle(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetTitle(), replace2.GetTitle(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetTitle(), await csc.paymentCardSecrets[0].GetTitleAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetTitle(), await csc.paymentCardSecrets[1].GetTitleAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.paymentCardSecrets.Count);
		}

		[Test]
		public async Task ReplaceHistorySecretAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string kdfeIdentifier = ",.-4key12344";
			string password = "--!#n,.ottha2tdragon42";

			byte[] nullArray = null;

			KeyDerivationFunctionEntry kdfe = await KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntryAsync(kdfeIdentifier, securityAsyncFunctions);
			CommonSecretsContainer csc = new CommonSecretsContainer(kdfe);

			History add1 = await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions);
			History add2 = await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions);

			History replace1 = await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions);
			History replace2 = await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions);
			
			// Act
			var addResultSuccess1 = await csc.AddHistorySecretAsync(password, add1, kdfeIdentifier, securityAsyncFunctions);
			var addResultSuccess2 = await csc.AddHistorySecretAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), add2, kdfeIdentifier, securityAsyncFunctions);

			var replaceResultSuccess1 = await csc.ReplaceHistorySecretAsync(0, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultSuccess2 = await csc.ReplaceHistorySecretAsync(1, await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), replace2, kdfeIdentifier, securityAsyncFunctions, SymmetricEncryptionAlgorithm.ChaCha20);

			var replaceResultFailure1 = await csc.ReplaceHistorySecretAsync(0, password, null, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure2 = await csc.ReplaceHistorySecretAsync(0, password, await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), "not existing", securityAsyncFunctions);
			var replaceResultFailure3 = await csc.ReplaceHistorySecretAsync(0, "", await ContentGeneratorAsync.GenerateRandomHistoryAsync(securityAsyncFunctions), kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure4 = await csc.ReplaceHistorySecretAsync(-1, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure5 = await csc.ReplaceHistorySecretAsync(2, password, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure6 = await csc.ReplaceHistorySecretAsync(0, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);
			var replaceResultFailure7 = await csc.ReplaceHistorySecretAsync(2, nullArray, replace1, kdfeIdentifier, securityAsyncFunctions);

			// Assert
			Assert.AreNotEqual(add1.GetDescription(), replace1.GetDescription(), "Make sure that random content do not match!");
			Assert.AreNotEqual(add2.GetDescription(), replace2.GetDescription(), "Make sure that random content do not match!");

			Assert.AreEqual(replace1.GetDescription(), await csc.historySecrets[0].GetDescriptionAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));
			Assert.AreEqual(replace2.GetDescription(), await csc.historySecrets[1].GetDescriptionAsync(await kdfe.GeneratePasswordBytesAsync(password, securityAsyncFunctions), securityAsyncFunctions));

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

			Assert.IsFalse(replaceResultFailure6.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure6.possibleError));

			Assert.IsFalse(replaceResultFailure7.success);
			Assert.IsFalse(string.IsNullOrEmpty(replaceResultFailure7.possibleError));

			Assert.AreEqual(2, csc.historySecrets.Count);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM