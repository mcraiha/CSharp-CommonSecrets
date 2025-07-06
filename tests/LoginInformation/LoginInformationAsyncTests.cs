#if ASYNC_WITH_CUSTOM

using System;
using System.Threading;
using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CollectionAssert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class LoginInformationAsyncTests
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

			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			LoginInformation li3 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(li1);
			Assert.IsNotNull(li2);
			Assert.IsNotNull(li3);
		}

		[Test]
		public async Task ModificationTimeAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);

			// Act
			DateTimeOffset modificationTime1 = li.GetModificationTime();
			await Task.Delay(1100);
			await li.UpdateUsernameAsync("dragon2", securityAsyncFunctions);
			DateTimeOffset modificationTime2 = li.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public async Task SetGetTitleAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			string newTitle = "Not so random forum";

			// Act
			await li.UpdateTitleAsync(newTitle, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newTitle, li.GetTitle());
		}

		[Test]
		public async Task SetGetURLAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			string newURL = "https://otherdomain.com";

			// Act
			await li.UpdateURLAsync(newURL, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newURL, li.GetURL());
		}

		[Test]
		public async Task SetGetEmailAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			string newEmail = "somey@me.me";

			// Act
			await li.UpdateEmailAsync(newEmail, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newEmail, li.GetEmail());
		}

		[Test]
		public async Task SetGetUsernameAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			string newUsername = "Kitten137";

			// Act
			await li.UpdateUsernameAsync(newUsername, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newUsername, li.GetUsername());
		}

		[Test]
		public async Task SetGetPasswordAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", securityAsyncFunctions);
			string newPassword = "joigjo32%&()";

			// Act
			await li.UpdatePasswordAsync(newPassword, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newPassword, li.GetPassword());
		}

		[Test]
		public async Task SetGetNotesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies", securityAsyncFunctions);
			string newNotes = "Even more text that nobody will read";

			// Act
			await li.UpdateNotesAsync(newNotes, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newNotes, li.GetNotes());
		}

		[Test]
		public async Task SetGetIconAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies", securityAsyncFunctions);
			byte[] newIcon = new byte[] { 4, 127, 0, 255, 1, 2, 3, 45 };

			// Act
			await li.UpdateIconAsync(newIcon, securityAsyncFunctions);

			// Assert
			CollectionAssert.AreEqual(newIcon, li.GetIcon());
		}

		[Test]
		public async Task SetGetCategoryAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies", securityAsyncFunctions);
			string newCategory = "Discussions";

			// Act
			await li.UpdateCategoryAsync(newCategory, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newCategory, li.GetCategory());
		}

		[Test]
		public async Task SetGetTagsAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies", securityAsyncFunctions);
			string newTags = "Leisure";

			// Act
			await li.UpdateTagsAsync(newTags, securityAsyncFunctions);

			// Assert
			Assert.AreEqual(newTags, li.GetTags());
		}

		[Test]
		public async Task ChecksumChangesAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			DateTimeOffset dto = DateTimeOffset.UtcNow;
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto, securityAsyncFunctions);			
			LoginInformation li3 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto, securityAsyncFunctions);

			// Act
			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();
			string checksum3 = li3.GetChecksumAsHex();

			string updatedPassword = li3.GetPassword() + "A";
			await li3.UpdatePasswordAsync(updatedPassword, securityAsyncFunctions);
			string checksum4 = li3.GetChecksumAsHex();

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

			LoginInformation li1 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13", securityAsyncFunctions);
			
			LoginInformation li3 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern", securityAsyncFunctions);

			// Act
			LoginInformation li2 = li1.ShallowCopy();

			LoginInformation li4 = li3.ShallowCopy();

			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();

			string checksum3 = li3.GetChecksumAsHex();
			string checksum4 = li4.GetChecksumAsHex();

			// Assert
			Assert.IsNotNull(li2);
			Assert.IsNotNull(li4);
			Assert.AreEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum4);
		}

		[Test]
		public async Task DeepCopyAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			string title = "Random forum";
			string url = "https://somedomain.com";
			string email = "nobbody@me2.me";
			string username = "dragon1337";
			string password = "password1!%";
			string notes = "funny dialog is funny";
			string mfa = "otpauth://totp/DRAGON?secret=SECRET";
			byte[] icon = new byte[] { 0, 1, 3, 4, 5, 7 };
			string category = "forums";
			string tags = "daily\tmodern";

			LoginInformation li1 = await LoginInformation.CreateLoginInformationAsync(newTitle: title, newUrl: url, newEmail: email, newUsername: username, newPassword: password, 
					newNotes: notes, newMFA: mfa, newIcon: icon, newCategory: category, newTags: tags, securityAsyncFunctions);

			// Act
			LoginInformation li2 = new LoginInformation(li1);

			// Assert
			Assert.AreNotSame(li1.title, li2.title);
			CollectionAssert.AreEqual(li1.title, li2.title);

			Assert.AreNotSame(li1.url, li2.url);
			CollectionAssert.AreEqual(li1.url, li2.url);

			Assert.AreNotSame(li1.email, li2.email);
			CollectionAssert.AreEqual(li1.email, li2.email);

			Assert.AreNotSame(li1.username, li2.username);
			CollectionAssert.AreEqual(li1.username, li2.username);

			Assert.AreNotSame(li1.password, li2.password);
			CollectionAssert.AreEqual(li1.password, li2.password);

			Assert.AreNotSame(li1.notes, li2.notes);
			CollectionAssert.AreEqual(li1.notes, li2.notes);

			Assert.AreNotSame(li1.mfa, li2.mfa);
			CollectionAssert.AreEqual(li1.mfa, li2.mfa);

			Assert.AreNotSame(li1.icon, li2.icon);
			CollectionAssert.AreEqual(li1.icon, li2.icon);

			Assert.AreNotSame(li1.category, li2.category);
			CollectionAssert.AreEqual(li1.category, li2.category);

			Assert.AreNotSame(li1.tags, li2.tags);
			CollectionAssert.AreEqual(li1.tags, li2.tags);

			Assert.AreEqual(li1.modificationTime, li2.modificationTime);
			Assert.AreEqual(li1.creationTime, li2.creationTime);

			Assert.AreEqual(li1.checksum, li2.checksum);
		}

		[Test]
		public async Task ChecksumSurvivesRoundtripAsyncTests()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li1 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13", securityAsyncFunctions);

			// Act
			string checksum1 = li1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(li1, Formatting.Indented);

			LoginInformation li2 = JsonConvert.DeserializeObject<LoginInformation>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, li2.GetChecksumAsHex());
		}

		[Test]
		public async Task CheckIfChecksumMatchesContentAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			LoginInformation li1 = await LoginInformation.CreateLoginInformationAsync(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13", securityAsyncFunctions);

			// Act
			bool shouldBeTrue = await li1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);
			li1.checksum = li1.checksum.Remove(0, 1);
			bool shouldBeFalse = await li1.CheckIfChecksumMatchesContentAsync(securityAsyncFunctions);

			// Assert
			Assert.IsTrue(shouldBeTrue);
			Assert.IsFalse(shouldBeFalse);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM