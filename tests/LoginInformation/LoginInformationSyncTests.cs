#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using System.Threading;
using NUnit.Framework;
using CSCommonSecrets;
using Newtonsoft.Json;

namespace Tests
{
	public class LoginInformationSyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern");

			// Act

			// Assert
			Assert.IsNotNull(li1);
			Assert.IsNotNull(li2);
			Assert.IsNotNull(li3);
		}

		[Test]
		public void ModificationTimeTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");

			// Act
			DateTimeOffset modificationTime1 = li.GetModificationTime();
			Thread.Sleep(1100);
			li.UpdateUsername("dragon2");
			DateTimeOffset modificationTime2 = li.GetModificationTime();

			// Assert
			Assert.Greater(modificationTime2, modificationTime1);
		}

		[Test]
		public void SetGetTitleTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			string newTitle = "Not so random forum";

			// Act
			li.UpdateTitle(newTitle);

			// Assert
			Assert.AreEqual(newTitle, li.GetTitle());
		}

		[Test]
		public void SetGetURLTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			string newURL = "https://otherdomain.com";

			// Act
			li.UpdateURL(newURL);

			// Assert
			Assert.AreEqual(newURL, li.GetURL());
		}

		[Test]
		public void SetGetEmailTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			string newEmail = "somey@me.me";

			// Act
			li.UpdateEmail(newEmail);

			// Assert
			Assert.AreEqual(newEmail, li.GetEmail());
		}

		[Test]
		public void SetGetUsernameTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			string newUsername = "Kitten137";

			// Act
			li.UpdateUsername(newUsername);

			// Assert
			Assert.AreEqual(newUsername, li.GetUsername());
		}

		[Test]
		public void SetGetPasswordTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1");
			string newPassword = "joigjo32%&()";

			// Act
			li.UpdatePassword(newPassword);

			// Assert
			Assert.AreEqual(newPassword, li.GetPassword());
		}

		[Test]
		public void SetGetNotesTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies");
			string newNotes = "Even more text that nobody will read";

			// Act
			li.UpdateNotes(newNotes);

			// Assert
			Assert.AreEqual(newNotes, li.GetNotes());
		}

		[Test]
		public void SetGetIconTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies");
			byte[] newIcon = new byte[] { 4, 127, 0, 255, 1, 2, 3, 45 };

			// Act
			li.UpdateIcon(newIcon);

			// Assert
			CollectionAssert.AreEqual(newIcon, li.GetIcon());
		}

		[Test]
		public void SetGetCategoryTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies");
			string newCategory = "Discussions";

			// Act
			li.UpdateCategory(newCategory);

			// Assert
			Assert.AreEqual(newCategory, li.GetCategory());
		}

		[Test]
		public void SetGetTagsTest()
		{
			// Arrange
			LoginInformation li = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
														newNotes: "some boring notes for someone", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] { 1, 2, 3, 45 }, newCategory: "Forums", newTags: "Hobbies");
			string newTags = "Leisure";

			// Act
			li.UpdateTags(newTags);

			// Assert
			Assert.AreEqual(newTags, li.GetTags());
		}

		[Test]
		public void ChecksumChangesTest()
		{
			// Arrange
			DateTimeOffset dto = DateTimeOffset.UtcNow;
			LoginInformation li1 = new LoginInformation();
			LoginInformation li2 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto);			
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", dto);

			// Act
			string checksum1 = li1.GetChecksumAsHex();
			string checksum2 = li2.GetChecksumAsHex();
			string checksum3 = li3.GetChecksumAsHex();

			string updatedPassword = li3.GetPassword() + "A";
			li3.UpdatePassword(updatedPassword);
			string checksum4 = li3.GetChecksumAsHex();

			// Assert
			Assert.AreNotEqual(checksum1, checksum2);
			Assert.AreEqual(checksum3, checksum2);
			Assert.AreNotEqual(checksum3, checksum4);
		}

		[Test]
		public void ShallowCopyTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13");
			
			LoginInformation li3 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon", newPassword: "password1", 
					newNotes: "funny dialog is funny", newMFA: "otpauth://totp/DRAGON?secret=SECRET", newIcon: new byte[] {0, 1, 3, 4, 5, 7}, newCategory: "forums", newTags: "daily\tmodern");

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
		public void DeepCopyTest()
		{
			// Arrange
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

			LoginInformation li1 = new LoginInformation(newTitle: title, newUrl: url, newEmail: email, newUsername: username, newPassword: password, 
					newNotes: notes, newMFA: mfa, newIcon: icon, newCategory: category, newTags: tags);

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
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			LoginInformation li1 = new LoginInformation(newTitle: "Random forum", newUrl: "https://somedomain.com", newEmail: "nobbody@me.me", newUsername: "dragon123", newPassword: "password13");

			// Act
			string checksum1 = li1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(li1, Formatting.Indented);

			LoginInformation li2 = JsonConvert.DeserializeObject<LoginInformation>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, li2.GetChecksumAsHex());
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM