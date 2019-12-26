using System.Text;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	public static class ChecksumHelper
	{
		/// <summary>
		/// Calculate SHA256 checksum from given byte arrays
		/// </summary>
		/// <param name="arrays">Byte arrays</param>
		/// <returns>Uppercase hex string</returns>
		public static string CalculateHexChecksum(params byte[][] arrays)
		{
			byte[] joinedArray = JoinByteArrays(arrays);

			using (SHA256 mySHA256 = SHA256.Create())
			{
				var hash = mySHA256.ComputeHash(joinedArray);
				return ByteArrayChecksumToHexString(hash);
			}
		}

		/// <summary>
		/// Join byte arrays to single byte array
		/// </summary>
		/// <param name="arrays">Byte arrays to join</param>
		/// <returns>Byte array</returns>
		public static byte[] JoinByteArrays(params byte[][] arrays)
		{
			int joinedLength = 0;
			foreach (byte[] byteArray in arrays)
			{
				joinedLength += byteArray.Length;
			}

			byte[] returnArray = new byte[joinedLength];
			int offset = 0;
			foreach (byte[] array in arrays) {
				System.Buffer.BlockCopy(array, 0, returnArray, offset, array.Length);
				offset += array.Length;
			}
			return returnArray;
		}

		private static string ByteArrayChecksumToHexString(byte[] byteArray)
		{
			var sb = new StringBuilder(byteArray.Length * 2);

			foreach (byte b in byteArray)
			{
				// Use uppercase
				sb.Append(b.ToString("X2"));
			}

			return sb.ToString();
		}
	}
}