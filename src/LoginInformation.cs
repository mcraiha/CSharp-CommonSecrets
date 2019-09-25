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

		public LoginInformation(string newTitle, string newUrl, string newUsername, string newPassword)
		{
			this.title = Encoding.UTF8.GetBytes(newTitle);
			this.url = Encoding.UTF8.GetBytes(newUrl);
			this.username = Encoding.UTF8.GetBytes(newUsername);
			this.password = Encoding.UTF8.GetBytes(newPassword);

			this.CalculateAndUpdateChecksum();
		}

		public LoginInformation ShallowCopy()
		{
			return (LoginInformation) this.MemberwiseClone();
		}

		#region Updates
		public void UpdateTitle(string updatedTitle)
		{
			this.title = Encoding.UTF8.GetBytes(updatedTitle);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateUrl(string updatedUrl)
		{
			this.url = Encoding.UTF8.GetBytes(updatedUrl);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateUsername(string updatedUsername)
		{
			this.username = Encoding.UTF8.GetBytes(updatedUsername);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		public void UpdatePassword(string updatedPassword)
		{
			this.password = Encoding.UTF8.GetBytes(updatedPassword);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
		}

		public void UpdateNotes(string updatedNotes)
		{
			this.notes = Encoding.UTF8.GetBytes(updatedNotes);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateIcon(byte[] updatedIcon)
		{
			this.icon = updatedIcon;

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateCategory(string updatedCategory)
		{
			this.category = Encoding.UTF8.GetBytes(updatedCategory);

			this.UpdateModificationTime();

			this.CalculateAndUpdateChecksum();
		}

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
			return ChecksumHelper.CalculateHexChecksum(this.title, this.url, this.username, this.password, this.notes, BitConverter.GetBytes(this.creationTime), BitConverter.GetBytes(this.modificationTime),
														this.icon, this.category, this.tags);
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}

}