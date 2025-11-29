#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class HistoryAsyncTests
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

			History history1 = new History() { eventType= "" }; // Do NOT use this code in your own projects!!!
			History history2 = await History.CreateHistoryAsync(HistoryEventType.Delete, "No meaning", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(history1);
			Assert.IsNotNull(history2);
		}

		[Test]
		public async Task GetValuesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			HistoryEventType historyEventType = HistoryEventType.Read;
			string description = "Dragon copied the password";
			History history = await History.CreateHistoryAsync(historyEventType, description, securityAsyncFunctions);

			// Act

			// Assert
			Assert.AreEqual(historyEventType, history.GetEventType());
			Assert.AreEqual(description, history.GetDescription());
		}

		
		[Test]
		public async Task ChecksumChangesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			History history1 = new History() { eventType= "" }; // Do NOT use this code in your own projects!!!
			History history2 = await History.CreateHistoryAsync(HistoryEventType.Update, "Some text here, yes.", dto, securityAsyncFunctions);
			History history3 = await History.CreateHistoryAsync(HistoryEventType.Update, "Some text here, yes.", dto, securityAsyncFunctions);

			// Act
			string checksum1 = history1.GetChecksumAsHex();
			string checksum2 = history2.GetChecksumAsHex();
			string checksum3 = history3.GetChecksumAsHex();

			await history3.UpdateHistoryAsync(HistoryEventType.Update, "Some text here, yes." + "a", dto, securityAsyncFunctions);
			string checksum4 = history3.GetChecksumAsHex();

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

			History history1 = await History.CreateHistoryAsync(HistoryEventType.Create, "Some text here, yes.", securityAsyncFunctions);

			// Act
			History history2 = history1.ShallowCopy();

			string checksum1 = history1.GetChecksumAsHex();
			string checksum2 = history2.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(history2);
			Assert.AreEqual(checksum1, checksum2);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			History history1 = await History.CreateHistoryAsync(HistoryEventType.Create, "Some text here, yes.", securityAsyncFunctions);

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
		public async Task ChecksumSurvivesRoundtripAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			History history1 = await History.CreateHistoryAsync(HistoryEventType.Create, "Some text here, yes.", securityAsyncFunctions);

			// Act
			string checksum1 = history1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(history1, Formatting.Indented);

			History history2 = JsonConvert.DeserializeObject<History>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, history2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			History history1 = await History.CreateHistoryAsync(HistoryEventType.Create, "Some text here, yes.", securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await history1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			history1.checksum = history1.checksum.Remove(0, 1);
			bool shouldBeFalse = await history1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM