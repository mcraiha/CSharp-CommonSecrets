
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
	}
}