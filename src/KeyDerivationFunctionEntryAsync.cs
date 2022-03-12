#if ASYNC_WITH_CUSTOM
// This file has async methods

using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Class that stores all needed info generating derived passwords from plain text passwords
	/// </summary>
	public sealed partial class KeyDerivationFunctionEntry
	{
		/// <summary>
		/// Private constructor for Key Derivation Function Entry, please use CreateKeyDerivationFunctionEntryAsync!
		/// </summary>
		/// <param name="prf">KeyDerivationPrf to use</param>
		/// <param name="saltBytes">Salt byte array</param>
		/// <param name="iterationsCount">How many iterations</param>
		/// <param name="howManyBytesAreWanted">How many output bytes are wanted</param>
		/// <param name="id">Key identifier</param>
		private KeyDerivationFunctionEntry(KeyDerivationPrf prf, byte[] saltBytes, int iterationsCount, int howManyBytesAreWanted, string id)
		{
			// Block SHA-1
			if (prf == KeyDerivationPrf.HMACSHA1)
			{
				throw new ArgumentException($"{nameof(prf)} cannot be SHA1 for security reasons");
			}

			// Check salt bytes
			if (saltBytes == null)
			{
				throw new ArgumentNullException(nameof(saltBytes));
			}
			else if (saltBytes.Length < saltMinLengthInBytes)
			{
				throw new ArgumentException($"{nameof(saltBytes)} should be at least {saltMinLengthInBytes} bytes!");
			}

			// Check iterations count
			if (iterationsCount < iterationsMin)
			{
				throw new ArgumentException($"{nameof(iterationsCount)} should be at least {iterationsMin}!");
			}

			// Check ID
			if (string.IsNullOrEmpty(id))
			{
				throw new ArgumentException($"{nameof(id)} should contain something!");
			}

			this.algorithm = KDFAlgorithm.PBKDF2.ToString();

			this.pseudorandomFunction = prf.ToString();
			
			this.salt = saltBytes;

			this.iterations = iterationsCount;

			this.derivedKeyLengthInBytes = howManyBytesAreWanted;

			this.keyIdentifier = Encoding.UTF8.GetBytes(id);
		}

		/// <summary>
		/// Construct new KeyDerivationFunctionEntry, async
		/// </summary>
		/// <param name="prf">KeyDerivationPrf to use</param>
		/// <param name="saltBytes">Salt byte array</param>
		/// <param name="iterationsCount">How many iterations</param>
		/// <param name="howManyBytesAreWanted">How many output bytes are wanted</param>
		/// <param name="id">Key identifier</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns></returns>
		public static async Task<KeyDerivationFunctionEntry> CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf prf, byte[] saltBytes, int iterationsCount, int howManyBytesAreWanted, string id, ISecurityAsyncFunctions securityFunctions)
		{
			KeyDerivationFunctionEntry keyDerivationFunctionEntry = new KeyDerivationFunctionEntry(prf, saltBytes, iterationsCount, howManyBytesAreWanted, id);
			// Calculate new checksum
			await keyDerivationFunctionEntry.CalculateAndUpdateChecksum(securityFunctions);
			return keyDerivationFunctionEntry;
		}

		/// <summary>
		/// Generate derived password
		/// </summary>
		/// <param name="regularPassword">"Normal" plaintext password</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns></returns>
		public byte[] GeneratePasswordBytes(string regularPassword, ISecurityAsyncFunctions securityFunctions)
		{
			Enum.TryParse(this.pseudorandomFunction, out KeyDerivationPrf keyDerivationPrf);

			return KeyDerivation.Pbkdf2(regularPassword, this.salt, keyDerivationPrf, this.iterations, this.derivedKeyLengthInBytes);
		}

		#region Checksum

		private async Task<string> CalculateHexChecksum(ISecurityAsyncFunctions securityFunctions)
		{
			return await ChecksumHelper.CalculateHexChecksumAsync(securityFunctions, Encoding.UTF8.GetBytes(this.algorithm), Encoding.UTF8.GetBytes(this.pseudorandomFunction), this.salt,
														BitConverter.GetBytes(this.iterations), BitConverter.GetBytes(this.derivedKeyLengthInBytes), this.keyIdentifier);
		}

		private async Task CalculateAndUpdateChecksum(ISecurityAsyncFunctions securityFunctions)
		{
			this.checksum = await this.CalculateHexChecksum(securityFunctions);
		}

		#endregion // Checksum


		#region Static helpers

		/// <summary>
		/// Create HMACSHA256 based KeyDerivationFunctionEntry with random salt, async
		/// </summary>
		/// <param name="id">Key identifier of this entry</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>KeyDerivationFunctionEntry</returns>
		public static async Task<KeyDerivationFunctionEntry> CreateHMACSHA256KeyDerivationFunctionEntry(string id, ISecurityAsyncFunctions securityFunctions)
		{
			int iterationsToDo = suggestedMinIterationsCount;
			byte[] salt = new byte[saltMinLengthInBytes];
			
			// First add some iterations
			byte[] fourBytes = new byte[4];
			securityFunctions.GenerateSecureRandomBytes(fourBytes);
			iterationsToDo += (int)(BitConverter.ToUInt32(fourBytes, 0) % 4096);

			// Then fill salt
			securityFunctions.GenerateSecureRandomBytes(salt);

			int neededBytes = 32;
			return await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, salt, iterationsToDo, neededBytes, id, securityFunctions);
		}

		/// <summary>
		/// Create HMACSHA512 based KeyDerivationFunctionEntry with random salt, async
		/// </summary>
		/// <param name="id">Key identifier of this entry</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>KeyDerivationFunctionEntry</returns>
		public static async Task<KeyDerivationFunctionEntry> CreateHMACSHA512KeyDerivationFunctionEntry(string id, ISecurityAsyncFunctions securityFunctions)
		{
			int iterationsToDo = suggestedMinIterationsCount;
			byte[] salt = new byte[saltMinLengthInBytes];
			
			// First add some iterations
			byte[] fourBytes = new byte[4];
			securityFunctions.GenerateSecureRandomBytes(fourBytes);
			iterationsToDo += (int)(BitConverter.ToUInt32(fourBytes, 0) % 4096);

			// Then fill salt
			securityFunctions.GenerateSecureRandomBytes(salt);

			int neededBytes = 64;
			return await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA512, salt, iterationsToDo, neededBytes, id, securityFunctions);
		}

		#endregion // Static helpers
	}
}

#endif // ASYNC_WITH_CUSTOM