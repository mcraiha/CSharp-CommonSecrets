using System;
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

		public static readonly int iterationsMin = 5000;

		public KDFAlgorithm algorithm;

		public KeyDerivationPrf pseudorandomFunction;

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
		/// Identifier, e.g. "primary"
		/// </summary>
		public string identifier;

		private string checksum = string.Empty;

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

			this.algorithm = KDFAlgorithm.PBKDF2;

			this.pseudorandomFunction = prf;
			
			this.salt = saltBytes;

			this.iterations = iterationsCount;

			this.derivedKeyLengthInBytes = howManyBytesAreWanted;

			this.identifier = id;
		}

		public byte[] GeneratePasswordBytes(string regularPassword)
		{
			return KeyDerivation.Pbkdf2(regularPassword, this.salt, this.pseudorandomFunction, this.iterations, this.derivedKeyLengthInBytes);
		}
	}

}