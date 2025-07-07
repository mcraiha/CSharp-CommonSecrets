using System;
using System.Security.Cryptography;

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;
#endif

namespace CSCommonSecrets;

/// <summary>
/// Static class for calculating needed checksums
/// </summary>
public static class ChecksumHelper
{
	#if ASYNC_WITH_CUSTOM

	/// <summary>
	/// Calculate SHA256 checksum from given byte arrays, async
	/// </summary>
	/// <param name="securityFunctions">Security functions</param>
	/// <param name="arrays">Byte arrays</param>
	/// <returns>Uppercase hex string</returns>
	public static async Task<string> CalculateHexChecksumAsync(ISecurityAsyncFunctions securityFunctions, params byte[][] arrays)
	{
		byte[] joinedArray = JoinByteArrays(arrays);
		return Convert.ToHexString(await securityFunctions.SHA256_Hash(joinedArray));
	}

	#elif WITH_CUSTOM

	#else // regular mode

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
			return Convert.ToHexString(hash);
		}
	}

	#endif

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
}