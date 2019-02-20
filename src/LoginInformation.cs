using System;
using System.Text;

namespace CSCommonSecrets
{
	public sealed class LoginInformation
	{
		public string title { get; set; } = string.Empty;
		public static readonly string titleKey = nameof(title);

		public string url { get; set; } = string.Empty;
		public static readonly string urlKey = nameof(url);

		public string username { get; set; } = string.Empty;
		public static readonly string usernameKey = nameof(username);

		public string password { get; set; } = string.Empty;
		public static readonly string passwordKey = nameof(password);

		public string notes { get; set; } = string.Empty;
		public static readonly string notesKey = nameof(notes);

		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string creationTimeKey = nameof(creationTime);

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;
		public static readonly string modificationTimeKey = nameof(modificationTime);

		public byte[] icon { get; set; } = new byte[0];
		public static readonly string iconKey = nameof(icon);

		public string category { get; set; } = string.Empty;
		public static readonly string categoryKey = nameof(category);

		public string tags { get; set; } = string.Empty;
		public static readonly string tagsKey = nameof(tags);

		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization purposes
		/// </summary>
		public LoginInformation()
		{

		}

		public LoginInformation(string newTitle, string newUrl, string newUsername, string newPassword)
		{
			this.title = newTitle;
			this.url = newUrl;
			this.username = newUsername;
			this.password = newPassword;

			this.CalculateAndUpdateChecksum();
		}

		public LoginInformation ShallowCopy()
		{
			return (LoginInformation) this.MemberwiseClone();
		}

		public void UpdateTitle(string updatedTitle)
		{
			this.title = updatedTitle;

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateUrl(string updatedUrl)
		{
			this.url = updatedUrl;

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateUsername(string updatedUsername)
		{
			this.username = updatedUsername;

			this.CalculateAndUpdateChecksum();
		}

		public void UpdatePassword(string updatedPassword)
		{
			this.password = updatedPassword;

			this.CalculateAndUpdateChecksum();
		}

		private void UpdateModificationTime()
		{
			this.modificationTime = DateTimeOffset.UtcNow;
		}

		public void UpdateNotes(string updatedNotes)
		{
			this.notes = updatedNotes;

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateIcon(byte[] updatedIcon)
		{
			this.icon = updatedIcon;

			this.CalculateAndUpdateChecksum();
		}

		public void UpdateCategory(string updatedCategory)
		{
			this.category = updatedCategory;

			this.CalculateAndUpdateChecksum();
		}

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
			return ChecksumHelper.CalculateHexChecksum(Encoding.UTF8.GetBytes(this.title), Encoding.UTF8.GetBytes(this.url), 
														Encoding.UTF8.GetBytes(this.username), Encoding.UTF8.GetBytes(this.password), 
														Encoding.UTF8.GetBytes(this.notes), 
														BitConverter.GetBytes(this.creationTime.ToUnixTimeSeconds()),
														BitConverter.GetBytes(this.modificationTime.ToUnixTimeSeconds()),
														this.icon,
														Encoding.UTF8.GetBytes(this.category), Encoding.UTF8.GetBytes(this.tags) );
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}
	}

}