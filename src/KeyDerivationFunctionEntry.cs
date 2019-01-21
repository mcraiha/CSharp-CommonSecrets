
namespace CSCommonSecrets
{
	public enum KDFAlgorithm
	{
		PBKDF2
	}

	public sealed class KeyDerivationFunctionEntry
	{
		public KDFAlgorithm algorithm;

		public byte[] salt;

		public int iterations;

		public string identifier;

		private string checksum = string.Empty;

		public KeyDerivationFunctionEntry(byte[] saltBytes, int iterationsCount, string id)
		{
			// TODO: Check salt bytes

			// TODO: Check iterations count

			// TODO: Check ID

			this.algorithm = KDFAlgorithm.PBKDF2;
			
			this.salt = saltBytes;

			this.iterations = iterationsCount;

			this.identifier = id;
		}
	}

}