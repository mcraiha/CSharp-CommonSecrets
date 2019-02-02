
namespace CSCommonSecrets
{
	public sealed class FileEntrySecret
	{
		public byte[] audalfData { get; set; } = new byte[0];

		public SymmetricKeyAlgorithm algorithm { get; set; }
		private string checksum = string.Empty;

		/// <summary>
		/// For deserialization
		/// </summary>
		public FileEntrySecret()
		{

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