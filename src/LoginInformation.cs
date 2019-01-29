using System;
using System.Text;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	public sealed class LoginInformation
	{
		public string title { get; set; } = string.Empty;

		public string url { get; set; } = string.Empty;

		public string username { get; set; } = string.Empty;

		public string password { get; set; } = string.Empty;

		public string notes { get; set; } = string.Empty;

		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;

		public byte[] icon { get; set; } = new byte[0];

		public string category { get; set; } = string.Empty;

		public string tags { get; set; } = string.Empty;

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

		public string GetChecksumAsBase64()
		{
			return this.checksum;
		}

		public bool CheckIfChecksumMatchesContent()
		{
			return checksum == CalculateBase64Checksum();
		}

		private string CalculateBase64Checksum()
		{
			using (SHA256 mySHA256 = SHA256.Create())
			{
				string iconAsBase64 = Convert.ToBase64String(icon);
				string forCalculatingHash = $"{this.title}{this.url}{this.username}{this.password}{this.notes}{this.creationTime.ToUnixTimeSeconds()}{this.modificationTime.ToUnixTimeSeconds()}{iconAsBase64}{this.category}{this.tags}";
				byte[] checksumBytes = mySHA256.ComputeHash(Encoding.UTF8.GetBytes(forCalculatingHash));
				return Convert.ToBase64String(checksumBytes);
			}
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateBase64Checksum();
		}
	}

}