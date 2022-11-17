#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class CommonSecretPlainNewtonsoftJsonTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			// Act
			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			// Assert
			Assert.IsNotNull(json);
			Assert.IsTrue(json.Contains("version"));
		}

		[Test]
		public void RoundTripTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string loginTitle1 = "Some nice website";
			string loginUrl1 = "https://hopefullynobodybuysthisdomain.com";
			string loginEmail1 = "backto@localhost";
			string loginUsername1 = "superniceuser";
			string loginPassword1 = "dragon77!"; 

			string noteTitle1 = "some notes";
			string noteText1 = "words are so hard sometimes 😠";

			string filename1 = "test.txt";
			byte[] file1Content = new byte[] { 1, 34, 46, 47, 24, 33, 4};

			string firstname1 = "Dragon";
			string lastname1 = "Laster";
			string middlename1 = "Midder";

			string cardTitle1 = "Super payment card";
			string cardNameOnCard1 = "Cool dragon";
			string cardCardType1 = "Debit";
			string cardNumber1 = "000123456789999";
			string cardSecurityCode1 = "123";
			string cardStartDate1 = "10/19";
			string cardExpirationDate1 = "02/25";
			string cardNotes = "The best payment option";
			
			// Act
			csc.loginInformations.Add(new LoginInformation(loginTitle1, loginUrl1, loginEmail1, loginUsername1, loginPassword1));
			csc.notes.Add(new Note(noteTitle1, noteText1));
			csc.files.Add(new FileEntry(filename1, file1Content));
			csc.contacts.Add(new Contact(firstname1, lastname1, middlename1));
			csc.paymentCards.Add(new PaymentCard(cardTitle1, cardNameOnCard1, cardCardType1, cardNumber1, cardSecurityCode1, cardStartDate1, cardExpirationDate1, cardNotes));

			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);
			//TestContext.Out.WriteLine(json);

			CommonSecretsContainer cscDeserialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(loginTitle1, cscDeserialized.loginInformations[0].title);
			Assert.AreEqual(loginUrl1, cscDeserialized.loginInformations[0].url);
			Assert.AreEqual(loginEmail1, cscDeserialized.loginInformations[0].email);
			Assert.AreEqual(loginUsername1, cscDeserialized.loginInformations[0].username);
			Assert.AreEqual(loginPassword1, cscDeserialized.loginInformations[0].password);

			Assert.AreEqual(noteTitle1, System.Text.Encoding.UTF8.GetString(cscDeserialized.notes[0].noteTitle));
			Assert.AreEqual(noteText1, System.Text.Encoding.UTF8.GetString(cscDeserialized.notes[0].noteText));

			Assert.AreEqual(filename1, cscDeserialized.files[0].filename);
			CollectionAssert.AreEqual(file1Content, cscDeserialized.files[0].fileContent);

			Assert.AreEqual(firstname1, System.Text.Encoding.UTF8.GetString(cscDeserialized.contacts[0].firstName));
			Assert.AreEqual(lastname1, System.Text.Encoding.UTF8.GetString(cscDeserialized.contacts[0].lastName));
			Assert.AreEqual(middlename1, System.Text.Encoding.UTF8.GetString(cscDeserialized.contacts[0].middleName));

			Assert.AreEqual(cardTitle1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].title));
			Assert.AreEqual(cardNameOnCard1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].nameOnCard));
			Assert.AreEqual(cardCardType1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].cardType));
			Assert.AreEqual(cardNumber1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].number));
			Assert.AreEqual(cardSecurityCode1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].securityCode));
			Assert.AreEqual(cardStartDate1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].startDate));
			Assert.AreEqual(cardExpirationDate1, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].expirationDate));
			Assert.AreEqual(cardNotes, System.Text.Encoding.UTF8.GetString(cscDeserialized.paymentCards[0].notes));
		}

		[Test]
		public void RoundTripComplexTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string password = "dragon667";
			byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			SettingsAES_CTR settingsAES_CTR1 = new SettingsAES_CTR(initialCounter1);
			SymmetricKeyAlgorithm skaAES = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR1);

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry("does not matter");

			int loginsAmount = 12;
			int loginsSecretAmount = 14;

			int notesAmount = 17;
			int notesSecretAmount = 11;

			int filesAmount = 5;
			int filesSecretAmount = 3;

			int contactAmount = 4;
			int contactSecretAmount = 2;

			int paymentAmount = 3;
			int paymentSecretAmount = 7;

			// Act
			byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

			csc.keyDerivationFunctionEntries.Add(kdfe);

			for (int i = 0; i < loginsAmount; i++)
			{
				csc.loginInformations.Add(ContentGeneratorSync.GenerateRandomLoginInformation());
			}

			for (int i = 0; i < loginsSecretAmount; i++)
			{
				csc.loginInformationSecrets.Add(new LoginInformationSecret(ContentGeneratorSync.GenerateRandomLoginInformation(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			}

			for (int i = 0; i < notesAmount; i++)
			{
				csc.notes.Add(ContentGeneratorSync.GenerateRandomNote());
			}

			for (int i = 0; i < notesSecretAmount; i++)
			{
				csc.noteSecrets.Add(new NoteSecret(ContentGeneratorSync.GenerateRandomNote(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			}

			for (int i = 0; i < filesAmount; i++)
			{
				csc.files.Add(ContentGeneratorSync.GenerateRandomFileEntry());
			}

			for (int i = 0; i < filesSecretAmount; i++)
			{
				csc.fileSecrets.Add(new FileEntrySecret(ContentGeneratorSync.GenerateRandomFileEntry(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			}

			for (int i = 0; i < contactAmount; i++)
			{
				csc.contacts.Add(ContentGeneratorSync.GenerateRandomContact());
			}

			for (int i = 0; i < contactSecretAmount; i++)
			{
				csc.contactSecrets.Add(new ContactSecret(ContentGeneratorSync.GenerateRandomContact(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			}

			for (int i = 0; i < paymentAmount; i++)
			{
				csc.paymentCards.Add(ContentGeneratorSync.GenerateRandomPaymentCard());
			}

			for (int i = 0; i < paymentSecretAmount; i++)
			{
				csc.paymentCardSecrets.Add(new PaymentCardSecret(ContentGeneratorSync.GenerateRandomPaymentCard(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			}

			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			CommonSecretsContainer cscDeserialized = JsonConvert.DeserializeObject<CommonSecretsContainer>(json);

			// Assert
			Assert.AreEqual(1, csc.keyDerivationFunctionEntries.Count);
			Assert.AreEqual(1, cscDeserialized.keyDerivationFunctionEntries.Count);
			Assert.IsTrue(ComparisonHelper.AreKeyDerivationFunctionEntriesEqual(csc.keyDerivationFunctionEntries[0], cscDeserialized.keyDerivationFunctionEntries[0]));

			Assert.AreEqual(loginsAmount, csc.loginInformations.Count);
			Assert.AreEqual(loginsAmount, cscDeserialized.loginInformations.Count);
			for (int i = 0; i < loginsAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreLoginInformationsEqual(csc.loginInformations[i], cscDeserialized.loginInformations[i]));
			}

			Assert.AreEqual(loginsSecretAmount, csc.loginInformationSecrets.Count);
			Assert.AreEqual(loginsSecretAmount, cscDeserialized.loginInformationSecrets.Count);
			for (int i = 0; i < loginsSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreLoginInformationSecretsEqual(csc.loginInformationSecrets[i], cscDeserialized.loginInformationSecrets[i]));
			}


			Assert.AreEqual(notesAmount, csc.notes.Count);
			Assert.AreEqual(notesAmount, cscDeserialized.notes.Count);
			for (int i = 0; i < notesAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreNotesEqual(csc.notes[i], cscDeserialized.notes[i]));
			}

			Assert.AreEqual(notesSecretAmount, csc.noteSecrets.Count);
			Assert.AreEqual(notesSecretAmount, cscDeserialized.noteSecrets.Count);
			for (int i = 0; i < notesSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreNotesSecretEqual(csc.noteSecrets[i], cscDeserialized.noteSecrets[i]));
			}


			Assert.AreEqual(filesAmount, csc.files.Count);
			Assert.AreEqual(filesAmount, cscDeserialized.files.Count);
			for (int i = 0; i < filesAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreFileEntriesEqual(csc.files[i], cscDeserialized.files[i]));
			}

			Assert.AreEqual(filesSecretAmount, csc.fileSecrets.Count);
			Assert.AreEqual(filesSecretAmount, cscDeserialized.fileSecrets.Count);
			for (int i = 0; i < filesSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreFileEntrySecretsEqual(csc.fileSecrets[i], cscDeserialized.fileSecrets[i]));
			}

			Assert.AreEqual(contactAmount, csc.contacts.Count);
			Assert.AreEqual(contactAmount, cscDeserialized.contacts.Count);
			for (int i = 0; i < contactAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreContactsEqual(csc.contacts[i], cscDeserialized.contacts[i]));
			}

			Assert.AreEqual(contactSecretAmount, csc.contactSecrets.Count);
			Assert.AreEqual(contactSecretAmount, cscDeserialized.contactSecrets.Count);
			for (int i = 0; i < contactSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.AreContactSecretsEqual(csc.contactSecrets[i], cscDeserialized.contactSecrets[i]));
			}

			Assert.AreEqual(paymentAmount, csc.paymentCards.Count);
			Assert.AreEqual(paymentAmount, cscDeserialized.paymentCards.Count);
			for (int i = 0; i < paymentAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.ArePaymentCardsEqual(csc.paymentCards[i], cscDeserialized.paymentCards[i]));
			}

			Assert.AreEqual(paymentSecretAmount, csc.paymentCardSecrets.Count);
			Assert.AreEqual(paymentSecretAmount, cscDeserialized.paymentCardSecrets.Count);
			for (int i = 0; i < paymentSecretAmount; i++)
			{
				Assert.IsTrue(ComparisonHelper.ArePaymentCardSecretsEqual(csc.paymentCardSecrets[i], cscDeserialized.paymentCardSecrets[i]));
			}
		}

		/*[Test]
		public void WriteJsonTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			string password = "dragon667";

			byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			SettingsAES_CTR settingsAES_CTR1 = new SettingsAES_CTR(initialCounter1);
			SymmetricKeyAlgorithm skaAES = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR1);

			SettingsChaCha20 settingsChaCha20_1 = SettingsChaCha20.CreateWithCryptographicRandomNumbers();
			SymmetricKeyAlgorithm skaChaCha20 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.ChaCha20, 256, settingsChaCha20_1);

			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry("does not matter");

			// Act
			byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

			csc.keyDerivationFunctionEntries.Add(kdfe);

			csc.loginInformations.Add(ContentGenerator.GenerateRandomLoginInformation());

			csc.loginInformationSecrets.Add(new LoginInformationSecret(ContentGenerator.GenerateRandomLoginInformation(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));
			
			csc.notes.Add(ContentGenerator.GenerateRandomNote());

			csc.noteSecrets.Add(new NoteSecret(ContentGenerator.GenerateRandomNote(), kdfe.GetKeyIdentifier(), skaChaCha20, derivedPassword));
	
			csc.files.Add(ContentGenerator.GenerateRandomFileEntry());

			csc.fileSecrets.Add(new FileEntrySecret(ContentGenerator.GenerateRandomFileEntry(), kdfe.GetKeyIdentifier(), skaAES, derivedPassword));

			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			System.IO.File.WriteAllText("commonsecrets.json", json);
		}*/
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM