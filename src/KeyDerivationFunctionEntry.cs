using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CSCommonSecrets
{
	/// <summary>
	/// Key derivation function algorithms
	/// </summary>
	public enum KDFAlgorithm
	{
		/// <summary>
		/// Password-Based Key Derivation Function 2
		/// </summary>
		PBKDF2 = 0,
	}

	/// <summary>
	/// Class that stores all needed info generating derived passwords from plain text passwords
	/// </summary>
	public sealed class KeyDerivationFunctionEntry
	{
		/// <summary>
		/// Minimun salt length in bytes
		/// </summary>
		public static readonly int saltMinLengthInBytes = 16;

		/// <summary>
		/// Minimum amount of iterations
		/// </summary>
		public static readonly int iterationsMin = 4000;

		/// <summary>
		/// Suggested amount of iterations
		/// </summary>
		public static readonly int suggestedMinIterationsCount = 100_000;

		/// <summary>
		/// Algorithm can only be "PBKDF2"
		/// </summary>
		public string algorithm { get; set; }

		/// <summary>
		/// Pseudo-random function can be either "HMAC-SHA256" or "HMAC-SHA512". It is casted to enum.
		/// </summary>
		public string pseudorandomFunction { get; set; }

		/// <summary>
		/// Salt bytes
		/// </summary>
		public byte[] salt { get; set; }

		/// <summary>
		/// How many iterations should be done
		/// </summary>
		public int iterations { get; set; }

		/// <summary>
		/// How many bytes should be returned
		/// </summary>
		public int derivedKeyLengthInBytes { get; set; }

		/// <summary>
		/// Key identifier, e.g. "primary" as UTF-8 byte array
		/// </summary>
		public byte[] keyIdentifier { get; set; }

		/// <summary>
		/// Calculated checksum of Key Derivation Function Entry
		/// </summary>
		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public KeyDerivationFunctionEntry()
		{

		}

		/// <summary>
		/// Default constructor for Key Derivation Function Entry
		/// </summary>
		/// <param name="prf">KeyDerivationPrf to use</param>
		/// <param name="saltBytes">Salt byte array</param>
		/// <param name="iterationsCount">How many iterations</param>
		/// <param name="howManyBytesAreWanted">How many output bytes are wanted</param>
		/// <param name="id">Key identifier</param>
		public KeyDerivationFunctionEntry(KeyDerivationPrf prf, byte[] saltBytes, int iterationsCount, int howManyBytesAreWanted, string id)
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

			// Calculate new checksum
			this.CalculateAndUpdateChecksum();
		}

		/// <summary>
		/// Generate derived password
		/// </summary>
		/// <param name="regularPassword">"Normal" plaintext password</param>
		/// <returns></returns>
		public byte[] GeneratePasswordBytes(string regularPassword)
		{
			Enum.TryParse(this.pseudorandomFunction, out KeyDerivationPrf keyDerivationPrf);

			return KeyDerivation.Pbkdf2(regularPassword, this.salt, keyDerivationPrf, this.iterations, this.derivedKeyLengthInBytes);
		}

		/// <summary>
		/// Get key identifier
		/// </summary>
		/// <returns>Key identifier as string</returns>
		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
		}

		#region Checksum

		/// <summary>
		/// Get checksum as hex
		/// </summary>
		/// <returns>Hex string</returns>
		public string GetChecksumAsHex()
		{
			return this.checksum;
		}

		private string CalculateHexChecksum()
		{
			return ChecksumHelper.CalculateHexChecksum(Encoding.UTF8.GetBytes(this.algorithm), Encoding.UTF8.GetBytes(this.pseudorandomFunction), this.salt,
														BitConverter.GetBytes(this.iterations), BitConverter.GetBytes(this.derivedKeyLengthInBytes), this.keyIdentifier);
		}

		private void CalculateAndUpdateChecksum()
		{
			this.checksum = this.CalculateHexChecksum();
		}

		#endregion // Checksum


		#region Static helpers

		/// <summary>
		/// Create HMACSHA256 based KeyDerivationFunctionEntry with random salt
		/// </summary>
		/// <param name="id">Key identifier of this entry</param>
		/// <returns>KeyDerivationFunctionEntry</returns>
		public static KeyDerivationFunctionEntry CreateHMACSHA256KeyDerivationFunctionEntry(string id)
		{
			int iterationsToDo = suggestedMinIterationsCount;
			byte[] salt = new byte[saltMinLengthInBytes];
			using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
			{
				// First add some iterations
				byte[] fourBytes = new byte[4];
				rngCsp.GetBytes(fourBytes);
				iterationsToDo += (int)(BitConverter.ToUInt32(fourBytes, 0) % 4096);

				// Then fill salt
				rngCsp.GetBytes(salt);
			}

			int neededBytes = 32;
			return new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, salt, iterationsToDo, neededBytes, id);
		}

		/// <summary>
		/// Create HMACSHA512 based KeyDerivationFunctionEntry with random salt
		/// </summary>
		/// <param name="id">Key identifier of this entry</param>
		/// <returns>KeyDerivationFunctionEntry</returns>
		public static KeyDerivationFunctionEntry CreateHMACSHA512KeyDerivationFunctionEntry(string id)
		{
			int iterationsToDo = suggestedMinIterationsCount;
			byte[] salt = new byte[saltMinLengthInBytes];
			using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
			{
				// First add some iterations
				byte[] fourBytes = new byte[4];
				rngCsp.GetBytes(fourBytes);
				iterationsToDo += (int)(BitConverter.ToUInt32(fourBytes, 0) % 4096);

				// Then fill salt
				rngCsp.GetBytes(salt);
			}

			int neededBytes = 64;
			return new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA512, salt, iterationsToDo, neededBytes, id);
		}

		#endregion // Static helpers
	}
}