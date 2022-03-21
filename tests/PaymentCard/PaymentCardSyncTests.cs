#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;
using System.Threading;

namespace Tests
{
	public class PaymentCardSyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			PaymentCard pc1 = new PaymentCard();
			PaymentCard pc2 = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act

			// Assert
			Assert.IsNotNull(pc1);
			Assert.IsNotNull(pc2);
		}

		[Test]
		public void ModificationTimeTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act
			DateTimeOffset modificationTime1 = pc.GetModificationTime();
			Thread.Sleep(1100);
			pc.UpdateNameOnCard("Coolr dragonz");
			DateTimeOffset modificationTime2 = pc.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public void SetGetTitleTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newTitle = "Another super bank";

			// Act
			pc.UpdateTitle(newTitle);

			// Assert
			Assert.AreEqual(newTitle, pc.GetTitle());
		}

		[Test]
		public void SetGetNameOnCardTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newNameOnCard = "Another super bank";

			// Act
			pc.UpdateNameOnCard(newNameOnCard);

			// Assert
			Assert.AreEqual(newNameOnCard, pc.GetNameOnCard());
		}

		[Test]
		public void SetGetCardTypeTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newCardType = "Credit";

			// Act
			pc.UpdateCardType(newCardType);

			// Assert
			Assert.AreEqual(newCardType, pc.GetCardType());
		}

		[Test]
		public void SetGetNumberTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newNumber = "1110000000001234";

			// Act
			pc.UpdateNumber(newNumber);

			// Assert
			Assert.AreEqual(newNumber, pc.GetNumber());
		}

		[Test]
		public void SetGetSecurityCodeTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newSecurityCode = "987";

			// Act
			pc.UpdateSecurityCode(newSecurityCode);

			// Assert
			Assert.AreEqual(newSecurityCode, pc.GetSecurityCode());
		}

		[Test]
		public void SetGetStartDateTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newStartDate = "12/21";

			// Act
			pc.UpdateStartDate(newStartDate);

			// Assert
			Assert.AreEqual(newStartDate, pc.GetStartDate());
		}

		[Test]
		public void SetGetExpirationDateTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newExpirationDate = "01/41";

			// Act
			pc.UpdateExpirationDate(newExpirationDate);

			// Assert
			Assert.AreEqual(newExpirationDate, pc.GetExpirationDate());
		}

		[Test]
		public void SetGetNotesTest()
		{
			// Arrange
			PaymentCard pc = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			string newNotes = "Maybe I could get a cooler card...";

			// Act
			pc.UpdateNotes(newNotes);

			// Assert
			Assert.AreEqual(newNotes, pc.GetNotes());
		}

		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			PaymentCard pc1 = new PaymentCard();
			PaymentCard pc2 = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");
			PaymentCard pc3 = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act
			string checksum1 = pc1.GetChecksumAsHex();
			string checksum2 = pc2.GetChecksumAsHex();
			string checksum3 = pc3.GetChecksumAsHex();

			string updatedTitle = pc3.GetTitle() + "A";
			pc3.UpdateTitle(updatedTitle);
			string checksum4 = pc3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ShallowCopyTest()
		{
			// Arrange
			PaymentCard pc1 = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

			// Act
			PaymentCard pc2 = pc1.ShallowCopy();

			string checksum1 = pc1.GetChecksumAsHex();
			string checksum2 = pc2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(pc2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			string title = "Bank of  Dragon";
			string nameOnCard = "Cool Dragon";
			string cardType = "Debit";
			string number = "0000000000001234";
			string securityCode = "111";
			string startDate = "11/20";
			string expirationDate = "05/33";
			string notes = "Super cool card I have here";

			PaymentCard pc1 = new PaymentCard(title, nameOnCard, cardType, number, securityCode, startDate, expirationDate, notes);

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
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			PaymentCard pc1 = new PaymentCard("Bank of  Dragon", "Cool Dragon", "Debit", "0000000000001234", "111", "11/20", "05/33", "Super cool card I have here");

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

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM