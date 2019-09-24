using System;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace CSCommonSecrets
{
	public enum KDFAlgorithm
	{
		PBKDF2
	}

	public sealed class KeyDerivationFunctionEntry
	{
		public static readonly int saltMinLengthInBytes = 16;

		public static readonly int iterationsMin = 4000;

		public static readonly int suggestedMinIterationsCount = 100_000;

		/// <summary>
		/// Algorithm can only be "PBKDF2"
		/// </summary>
		public string algorithm;

		/// <summary>
		/// Pseudo-random function can be either "HMAC-SHA256" or "HMAC-SHA512". It is casted to enum.
		/// </summary>
		public string pseudorandomFunction;

		/// <summary>
		/// Salt bytes
		/// </summary>
		public byte[] salt;

		/// <summary>
		/// How many iterations should be done
		/// </summary>
		public int iterations;

		/// <summary>
		/// How many bytes should be returned
		/// </summary>
		public int derivedKeyLengthInBytes;

		/// <summary>
		/// Key identifier, e.g. "primary" as byte array
		/// </summary>
		public byte[] keyIdentifier;

		public string checksum { get; set; } = string.Empty;

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public KeyDerivationFunctionEntry()
		{

		}

		public KeyDerivationFunctionEntry(KeyDerivationPrf prf, byte[] saltBytes, int iterationsCount, int howManyBytesAreWanted, string id)
		{
			// Check salt bytes
			if (saltBytes == null)
			{
				throw new ArgumentNullException(nameof(saltBytes));
			}
			else if (saltBytes.Length < 16)
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

		public byte[] GeneratePasswordBytes(string regularPassword)
		{
			Enum.TryParse(this.pseudorandomFunction, out KeyDerivationPrf keyDerivationPrf);

			return KeyDerivation.Pbkdf2(regularPassword, this.salt, keyDerivationPrf, this.iterations, this.derivedKeyLengthInBytes);
		}

		public string GetKeyIdentifier()
		{
			return System.Text.Encoding.UTF8.GetString(this.keyIdentifier);
		}

		#region Checksum

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
		/// <param name="id">Id of the entry</param>
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
		/// <param name="id">Id of the entry</param>
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