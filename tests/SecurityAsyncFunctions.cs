#if ASYNC_WITH_CUSTOM

using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

using System.Threading.Tasks;

namespace Tests
{
	/// <summary>
	/// For async testing purposes only
	/// </summary>
	public class SecurityAsyncFunctions : ISecurityAsyncFunctions
	{
		/// <summary>
		/// Get AES CTR Allowed Counter Length
		/// </summary>
		/// <returns>Length</returns>
		public int AES_CTRAllowedCounterLength()
		{
			return 16;
		}

		/// <summary>
		/// AES encryption in async land
		/// </summary>
		/// <param name="bytesToEncrypt">Bytes to encrypt</param>
		/// <param name="key">Key</param>
		/// <param name="initialCounter">Initial counter</param>
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
		/// Get ChaCha20 Allowed Nonce Length
		/// </summary>
		/// <returns>Length</returns>
		public int ChaCha20AllowedNonceLength()
		{
			return 12;
		}

		/// <summary>
		/// ChaCha20 encryption in async land
		/// </summary>
		/// <param name="bytesToEncrypt">Bytes to encrypt</param>
		/// <param name="key">Key</param>
		/// <param name="nonce">Nonce</param>
		/// <param name="counter">Counter</param>
		/// <returns></returns>
		public async Task<byte[]> ChaCha20_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] nonce, uint counter)
		{
			await Task.Delay(1);

			byte[] returnArray = new byte[bytesToEncrypt.Length];
			using (ChaCha20 forEncrypting = new ChaCha20(key, nonce, counter))
			{
				forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
			}

			return returnArray;
		}

		/// <summary>
		/// Performs key derivation using the PBKDF2 algorithm
		/// </summary>
		/// <param name="password">Password</param>
		/// <param name="salt">Salt</param>
		/// <param name="prf">Key derivation algorithm</param>
		/// <param name="iterationCount">Iteration count</param>
		/// <param name="numBytesRequested">Number of bytes requested</param>
		/// <returns></returns>
		public async Task<byte[]> Pbkdf2(string password, byte[] salt, Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivationPrf prf, int iterationCount, int numBytesRequested)
		{
			await Task.Delay(1);

			return Microsoft.AspNetCore.Cryptography.KeyDerivation.KeyDerivation.Pbkdf2(password, salt, prf, iterationCount, numBytesRequested);
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