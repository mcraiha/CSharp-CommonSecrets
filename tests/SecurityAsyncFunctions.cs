#if ASYNC_WITH_CUSTOM

using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

using System.Threading.Tasks;

namespace Tests
{
	/// <summary>
	/// For async testing purposes only
	/// </summary>
	public class SecurityAsyncFunctions : ISecurityAsyncFunctions
	{
		/// <summary>
		/// AES Encryption in async land
		/// </summary>
		/// <param name="bytesToEncrypt"></param>
		/// <param name="key"></param>
		/// <param name="initialCounter"></param>
		/// <returns></returns>
		public async Task<byte[]> AES_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] initialCounter)
		{
			await Task.Delay(1);

			byte[] returnArray = new byte[bytesToEncrypt.Length];
			using (AES_CTR forEncrypting = new AES_CTR(key, initialCounter))
			{
				forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
			}

			return returnArray;
		}

		/// <summary>
		/// SHA256 hash async land
		/// </summary>
		/// <param name="bytesToHash"></param>
		/// <returns></returns>
		public async Task<byte[]> SHA256_Hash(byte[] bytesToHash)
		{
			await Task.Delay(1);

			using (SHA256 mySHA256 = SHA256.Create())
			{
				return mySHA256.ComputeHash(bytesToHash);
			}
		}

		private static System.Security.Cryptography.RandomNumberGenerator rng = System.Security.Cryptography.RandomNumberGenerator.Create();

		/// <summary>
		/// Generate random bytes
		/// </summary>
		/// <param name="byteArray"></param>
		public void GenerateSecureRandomBytes(byte[] byteArray)
		{
			rng.GetBytes(byteArray);
		}

		/// <summary>
		/// Generate random bytes
		/// </summary>
		/// <param name="byteArray"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		public void GenerateSecureRandomBytes(byte[] byteArray, int offset, int count)
		{
			rng.GetBytes(byteArray, offset, count);
		}
	}
}

#endif // ASYNC_WITH_CUSTOM