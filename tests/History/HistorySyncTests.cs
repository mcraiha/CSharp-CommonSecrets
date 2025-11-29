#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class HistorySyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			History history1 = new History() { eventType= "" }; // Do NOT use this code in your own projects!!!
			History history2 = new History(HistoryEventType.Delete, "No meaning");

			// Act

			// Assert
			Assert.IsNotNull(history1);
			Assert.IsNotNull(history2);
		}

		[Test]
		public void GetValuesTest()
		{
			// Arrange
			HistoryEventType historyEventType = HistoryEventType.Read;
			string description = "Dragon copied the password";
			History history = new History(historyEventType, description);

			// Act

			// Assert
			Assert.AreEqual(historyEventType, history.GetEventType());
			Assert.AreEqual(description, history.GetDescription());
		}
		
		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			History history1 = new History() { eventType= "" }; // Do NOT use this code in your own projects!!!
			History history2 = new History(HistoryEventType.Update, "Some text here, yes.", dto);
			History history3 = new History(HistoryEventType.Update, "Some text here, yes.", dto);

			// Act
			string checksum1 = history1.GetChecksumAsHex();
			string checksum2 = history2.GetChecksumAsHex();
			string checksum3 = history3.GetChecksumAsHex();

			history3.UpdateHistory(HistoryEventType.Update, "Some text here, yes." + "a", dto);
			string checksum4 = history3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ShallowCopyTest()
		{
			// Arrange
			History history1 = new History(HistoryEventType.Create, "Some text here, yes.");

			// Act
			History history2 = history1.ShallowCopy();

			string checksum1 = history1.GetChecksumAsHex();
			string checksum2 = history2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(history2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public void DeepCopyTest()
		{
			// Arrange
			History history1 = new History(HistoryEventType.Create, "Some text here, yes.");

			// Act
			History history2 = new History(history1);

			// Assert
			Assert.AreNotSame(history1.eventType, history2.eventType);
			Assert.AreEqual(history1.eventType, history2.eventType);

			Assert.AreNotSame(history1.descriptionText, history2.descriptionText);
			CollectionAssert.AreEqual(history1.descriptionText, history2.descriptionText);

			Assert.AreEqual(history1.occurenceTime, history2.occurenceTime);

			Assert.AreEqual(history1.checksum, history2.checksum);
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			History history1 = new History(HistoryEventType.Create, "Some text here, yes.");

			// Act
			string checksum1 = history1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(history1, Formatting.Indented);

			History history2 = JsonConvert.DeserializeObject<History>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, history2.GetChecksumAsHex());
		}

		[Test]
		public void CheckIfChecksumMatchesContentTest()
		{
			// Arrange
			History history1 = new History(HistoryEventType.Create, "Some text here, yes.");

			// Act
			bool shouldBeTrue = history1.CheckIfChecksumMatchesContent();
			history1.checksum = history1.checksum.Remove(0, 1);
			bool shouldBeFalse = history1.CheckIfChecksumMatchesContent();

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM