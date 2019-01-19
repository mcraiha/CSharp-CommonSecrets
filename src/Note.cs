using System;
using System.Text;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	public sealed class Note
	{
		public string noteText { get; set; } = string.Empty;

		public DateTimeOffset creationTime { get; set; } = DateTimeOffset.UtcNow;

		public DateTimeOffset modificationTime { get; set; } = DateTimeOffset.UtcNow;

		private string checksum = string.Empty;

		public Note()
		{
			
		}

		public Note(string newNoteText)
		{
			this.UpdateNote(newNoteText);
		}

		public void UpdateNote(string updatedNoteText)
		{
			this.noteText = updatedNoteText;
			this.modificationTime = DateTimeOffset.UtcNow;
			this.CalculateAndUpdateChecksum();
		}

		public bool CheckIfChecksumMatchesContent()
		{
			return checksum == CalculateBase64Checksum();
		}

		private string CalculateBase64Checksum()
		{
			using (SHA256 mySHA256 = SHA256.Create())
			{
				string forCalculatingHash = $"{noteText}!{creationTime.ToUnixTimeSeconds()}!{modificationTime}";
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