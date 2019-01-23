using System.Collections.Generic;

namespace CSCommonSecrets
{
	public sealed class LoginInformationSecret
	{
		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public LoginInformationSecret()
		{

		}

		public LoginInformationSecret(Dictionary<string, object> loginInformationAsDictionary, SymmetricKeyAlgorithm algorithm,  byte[] derivedPassword)
		{
			// Create AUDALF payload from dictionary

			// Encrypt the AUDALF payload with given algorithm
		}
	}

}