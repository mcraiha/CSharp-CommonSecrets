#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

using System.Threading.Tasks;

namespace Tests
{
	public class PaymentCardAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task ConstructorAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc1 = new PaymentCard();
			PaymentCard pc2 = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(pc1);
			Assert.IsNotNull(pc2);
		}

		[Test]
		public async Task ModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			DateTimeOffset modificationTime1 = pc.GetModificationTime();
			await Task.Delay(1100);
			await pc.UpdateNameOnCardAsync("Coolr dragonz", securityAsyncFunctions);
			DateTimeOffset modificationTime2 = pc.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public async Task SetGetTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newTitle = "Another super bank";

			// Act
			await pc.UpdateTitleAsync(newTitle, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newTitle, pc.GetTitle());
		}

		[Test]
		public async Task SetGetNameOnCardAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newNameOnCard = "Another super bank";

			// Act
			await pc.UpdateNameOnCardAsync(newNameOnCard, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newNameOnCard, pc.GetNameOnCard());
		}

		[Test]
		public async Task SetGetCardTypeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newCardType = "Credit";

			// Act
			await pc.UpdateCardTypeAsync(newCardType, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newCardType, pc.GetCardType());
		}

		[Test]
		public async Task SetGetNumberAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newNumber = "1110000000001234";

			// Act
			await pc.UpdateNumberAsync(newNumber, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newNumber, pc.GetNumber());
		}

		[Test]
		public async Task SetGetSecurityCodeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newSecurityCode = "987";

			// Act
			await pc.UpdateSecurityCodeAsync(newSecurityCode, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newSecurityCode, pc.GetSecurityCode());
		}

		[Test]
		public async Task SetGetStartDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newStartDate = "12/21";

			// Act
			await pc.UpdateStartDateAsync(newStartDate, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newStartDate, pc.GetStartDate());
		}

		[Test]
		public async Task SetGetExpirationDateAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newExpirationDate = "01/41";

			// Act
			await pc.UpdateExpirationDateAsync(newExpirationDate, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newExpirationDate, pc.GetExpirationDate());
		}

		[Test]
		public async Task SetGetNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			string newNotes = "Maybe I could get a cooler card...";

			// Act
			await pc.UpdateNotesAsync(newNotes, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newNotes, pc.GetNotes());
		}

		[Test]
		public async Task ChecksumChangesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			PaymentCard pc1 = new PaymentCard();
			PaymentCard pc2 = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);
			PaymentCard pc3 = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			string checksum1 = pc1.GetChecksumAsHex();
			string checksum2 = pc2.GetChecksumAsHex();
			string checksum3 = pc3.GetChecksumAsHex();

			string updatedTitle = pc3.GetTitle() + "A";
			await pc3.UpdateTitleAsync(updatedTitle, securityAsyncFunctions);
			string checksum4 = pc3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public async Task ShallowCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc1 = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			PaymentCard pc2 = pc1.ShallowCopy();

			string checksum1 = pc1.GetChecksumAsHex();
			string checksum2 = pc2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(pc2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string title = "Bank of  Dragon";
			string nameOnCard = "Cool Dragon";
			string cardType = "Debit";
			string number = "0000000000001234";
			string securityCode = "111";
			string startDate = "11/20";
			string expirationDate = "05/33";
			string notes = "Super cool card I have here";

			PaymentCard pc1 = await PaymentCard.CreatePaymentCardAsync(title, nameOnCard, cardType, number, securityCode, startDate, expirationDate, notes, securityAsyncFunctions);

			// Act
			PaymentCard pc2 = new PaymentCard(pc1);

			// Assert
			Assert.AreNotSame(pc1.title, pc2.title);
			CollectionAssert.AreEqual(pc1.title, pc2.title);

			Assert.AreNotSame(pc1.nameOnCard, pc2.nameOnCard);
			CollectionAssert.AreEqual(pc1.nameOnCard, pc2.nameOnCard);

			Assert.AreNotSame(pc1.cardType, pc2.cardType);
			CollectionAssert.AreEqual(pc1.cardType, pc2.cardType);

			Assert.AreNotSame(pc1.number, pc2.number);
			CollectionAssert.AreEqual(pc1.number, pc2.number);

			Assert.AreNotSame(pc1.securityCode, pc2.securityCode);
			CollectionAssert.AreEqual(pc1.securityCode, pc2.securityCode);

			Assert.AreNotSame(pc1.startDate, pc2.startDate);
			CollectionAssert.AreEqual(pc1.startDate, pc2.startDate);

			Assert.AreNotSame(pc1.expirationDate, pc2.expirationDate);
			CollectionAssert.AreEqual(pc1.expirationDate, pc2.expirationDate);

			Assert.AreNotSame(pc1.notes, pc2.notes);
			CollectionAssert.AreEqual(pc1.notes, pc2.notes);

			Assert.AreEqual(pc1.modificationTime, pc2.modificationTime);
			Assert.AreEqual(pc1.creationTime, pc2.creationTime);

			Assert.AreEqual(pc1.checksum, pc2.checksum);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			PaymentCard pc1 = await PaymentCard.CreatePaymentCardAsync("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here", securityAsyncFunctions);

			// Act
			string checksum1 = pc1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(pc1, Formatting.Indented);

			PaymentCard pc2 = JsonConvert.DeserializeObject<PaymentCard>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, pc2.GetChecksumAsHex());
		}
	}
}

#endif // ASYNC_WITH_CUSTOM