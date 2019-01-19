using System;

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
	}

}