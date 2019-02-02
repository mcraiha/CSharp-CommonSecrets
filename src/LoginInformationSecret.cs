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


		#region Checksum

		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(this.audalfData, algorithm.GetSettingsAsBytes());
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum
	}

}