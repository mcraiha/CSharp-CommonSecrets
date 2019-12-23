using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class LoginInformation
	{
		public byte[] title { get; set; } = new byte[0];
		public static readonly string titleKey = nameof(title);

		public byte[] url { get; set; } = new byte[0];
		public static readonly string urlKey = nameof(url);

		public byte[] email { get; set; } = new byte[0];
		public static readonly string emailKey = nameof(email);

		public byte[] username { get; set; } = new byte[0];
		public static readonly string usernameKey = nameof(username);

		public byte[] password { get; set; } = new byte[0];
		public static readonly string passwordKey = nameof(password);

		public byte[] notes { get; set; } = new byte[0];
		public static readonly string notesKey = nameof(notes);

		public long creationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string creationTimeKey = nameof(creationTime);

		public long modificationTime { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		public static readonly string modificationTimeKey = nameof(modificationTime);

		public byte[] icon { get; set; } = new byte[0];
		public static readonly string iconKey = nameof(icon);

		public byte[] category { get; set; } = new byte[0];
		public static readonly string categoryKey = nameof(category);

		public byte[] tags { get; set; } = new byte[0];
		public static readonly string tagsKey = nameof(tags);

		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public LoginInformation()
		{

		}

		/// <summary>
		/// Default small LoginInformation constructor
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword) : this (newTitle, newUrl, newEmail, newUsername, newPassword, DateTimeOffset.UtcNow)
		{

		}

		/// <summary>
		/// Small LoginInformation constructor
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="time">Creation and modification timestamps</param>
		public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, DateTimeOffset time)
		{
			this.title = Encoding.UTF8.GetBytes(newTitle);
			this.url = Encoding.UTF8.GetBytes(newUrl);
			this.email = Encoding.UTF8.GetBytes(newEmail);
			this.username = Encoding.UTF8.GetBytes(newUsername);
			this.password = Encoding.UTF8.GetBytes(newPassword);
			this.creationTime = time.ToUnixTimeSeconds();
			this.modificationTime = time.ToUnixTimeSeconds();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Default full LoginInformation constructor
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="newNotes">Notes</param>
		/// <param name="newIcon">Icon</param>
		/// <param name="newCategory">Category</param>
		/// <param name="newTags">Tags (as tab separated)</param>
		public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, 
									byte[] newIcon, string newCategory, string newTags) : this (newTitle, newUrl, newEmail, newUsername, newPassword, newNotes, 
									newIcon, newCategory, newTags, DateTimeOffset.UtcNow)
		{

		}

		/// <summary>
		/// Full LoginInformation constructor
		/// </summary>
		/// <param name="newTitle">Title</param>
		/// <param name="newUrl">URL</param>
		/// <param name="newEmail">Email</param>
		/// <param name="newUsername">Username</param>
		/// <param name="newPassword">Password</param>
		/// <param name="newNotes">Notes</param>
		/// <param name="newIcon">Icon</param>
		/// <param name="newCategory">Category</param>
		/// <param name="newTags">Tags (as tab separated)</param>
		/// <param name="time">Creation and modification timestamps</param>
		public LoginInformation(string newTitle, string newUrl, string newEmail, string newUsername, string newPassword, string newNotes, 
									byte[] newIcon, string newCategory, string newTags, DateTimeOffset time)
		{
			this.title = Encoding.UTF8.GetBytes(newTitle);
			this.url = Encoding.UTF8.GetBytes(newUrl);
			this.email = Encoding.UTF8.GetBytes(newEmail);
			this.username = Encoding.UTF8.GetBytes(newUsername);
			this.password = Encoding.UTF8.GetBytes(newPassword);

			this.notes = Encoding.UTF8.GetBytes(newNotes);
			this.icon = newIcon;
			this.category = Encoding.UTF8.GetBytes(newCategory);
			this.tags = Encoding.UTF8.GetBytes(newTags);

			this.creationTime = time.ToUnixTimeSeconds();
			this.modificationTime = time.ToUnixTimeSeconds();

			this.CalculateAndUpdateChecksum();
		}

		public LoginInformation ShallowCopy()
		{
			return (LoginInformation) this.MemberwiseClone();
		}

		#region Updates

		/// <summary>
		/// Update title
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedTitle">Updated title</param>
		public void UpdateTitle(string updatedTitle)
		{
			this.title = Encoding.UTF8.GetBytes(updatedTitle);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}
		
		/// <summary>
		/// Update URL
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedUrl">Updated URL</param>
		public void UpdateUrl(string updatedUrl)
		{
			this.url = Encoding.UTF8.GetBytes(updatedUrl);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update email
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedEmail">Updated email</param>
		public void UpdateEmail(string updatedEmail)
		{
			this.url = Encoding.UTF8.GetBytes(updatedEmail);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update username
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedUsername">Updated username</param>
		public void UpdateUsername(string updatedUsername)
		{
			this.username = Encoding.UTF8.GetBytes(updatedUsername);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update password
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedPassword">Updated password</param>
		public void UpdatePassword(string updatedPassword)
		{
			this.password = Encoding.UTF8.GetBytes(updatedPassword);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update modification time from current UTC timestamp
		/// </summary>
		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		/// <summary>
		/// Update notes
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedNotes">Updated notes</param>
		public void UpdateNotes(string updatedNotes)
		{
			this.notes = Encoding.UTF8.GetBytes(updatedNotes);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update icon
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedIcon">Updated icon</param>
		public void UpdateIcon(byte[] updatedIcon)
		{
			this.icon = updatedIcon;

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update category
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedCategory">Updated category</param>
		public void UpdateCategory(string updatedCategory)
		{
			this.category = Encoding.UTF8.GetBytes(updatedCategory);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Update tags
		/// </summary>
		/// <remarks>Will calculate checksum after update</remarks>
		/// <param name="updatedTags">Updated tags (as tab separated)</param>
		public void UpdateTags(string updatedTags)
		{
			this.tags = Encoding.UTF8.GetBytes(updatedTags);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		#endregion // Updates

		#region Getters

		public string GetTitle()
		{
			return System.Text.Encoding.UTF8.GetString(this.title);
		}

		public string GetURL()
		{
			return System.Text.Encoding.UTF8.GetString(this.url);
		}

		public string GetEmail()
		{
			return System.Text.Encoding.UTF8.GetString(this.email);
		}

		public string GetUsername()
		{
			return System.Text.Encoding.UTF8.GetString(this.username);
		}

		public string GetPassword()
		{
			return System.Text.Encoding.UTF8.GetString(this.password);
		}

		public string GetNotes()
		{
			return System.Text.Encoding.UTF8.GetString(this.notes);
		}

		public byte[] GetIcon()
		{
			return this.icon;
		}

		public string GetCategory()
		{
			return System.Text.Encoding.UTF8.GetString(this.category);
		}

		public string GetTags()
		{
			return System.Text.Encoding.UTF8.GetString(this.tags);
		}

		public DateTimeOffset GetCreationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.creationTime);
		}

		public DateTimeOffset GetModificationTime()
		{
			return DateTimeOffset.FromUnixTimeSeconds(this.modificationTime);
		}

		#endregion // Getters

		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		public bool CheckIfChecksumMatchesContent()
		{
			return checksum == CalculateHexChecksum();
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.title, this.url, this.email, this.username, this.password, this.notes, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime),
														this.icon, this.category, this.tags);
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}

}