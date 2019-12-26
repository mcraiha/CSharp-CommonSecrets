using System;
using System.Text;
using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	/// <summary>
	/// Symmetric Encryption Algorithm enum
	/// </summary>
	public enum SymmetricEncryptionAlgorithm
	{
		/// AES in CTR mode
		AES_CTR,
		/// ChaCha20
		ChaCha20
	}

	/// <summary>
	/// Symmetric key algorithm contains all needed info for encryption/decryption
	/// </summary>
	public sealed class SymmetricKeyAlgorithm
	{
		/// <summary>
		/// Symmetric Encryption Algorithm as string
		/// </summary>
		public string algorithm { get; set; }

		/// <summary>
		/// Key size in bits
		/// </summary>
		public int keySizeInBits { get; set; }

		/// <summary>
		/// AES_CTR settings, might be null
		/// </summary>
		public SettingsAES_CTR settingsAES_CTR { get; set; }

		/// <summary>
		/// ChaCha20 settings, might be null
		/// </summary>
		public SettingsChaCha20 settingsChaCha20 { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SymmetricKeyAlgorithm()
		{

		}

		/// <summary>
		/// Default constructor for Symmetric key algorithm
		/// </summary>
		/// <param name="algorithm">Algorithm to use</param>
		/// <param name="keySizeInBits">Key size in bits, e.g. 256</param>
		/// <param name="settings">Settings for chosen algorithm</param>
		public SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm algorithm, int keySizeInBits, object settings)
		{
			this.algorithm = algorithm.ToString();

			if (algorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				if (!Array.Exists(AES_CTR.allowedKeyLengths, allowed => allowed * 8 == keySizeInBits))
				{
					throw new ArgumentException($"{keySizeInBits} is not valid AES-CTR key size!");
				}

				this.settingsAES_CTR = (SettingsAES_CTR)settings;
			}
			else if (algorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				if (ChaCha20.allowedKeyLength * 8 != keySizeInBits)
				{
					throw new ArgumentException($"{keySizeInBits} is not valid ChaCha20 key size!");
				}

				this.settingsChaCha20 = (SettingsChaCha20)settings;
			}
			else
			{
				throw new NotImplementedException($"{algorithm} constructor not implemented yet!");
			}

			this.keySizeInBits = keySizeInBits;
		}

		/// <summary>
		/// Encrypt given bytes with given key. Returns new array with encrypted bytes
		/// </summary>
		/// <param name="bytesToEncrypt">Byte array to encrypu</param>
		/// <param name="key"></param>
		/// <returns>Encrypted bytes in new array</returns>
		public byte[] EncryptBytes(byte[] bytesToEncrypt, byte[] key)
		{
			byte[] returnArray = new byte[bytesToEncrypt.Length];

			Enum.TryParse(this.algorithm, out SymmetricEncryptionAlgorithm actualAlgorithm);

			if (actualAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				using (AES_CTR forEncrypting = new AES_CTR(key, this.settingsAES_CTR.initialCounter))
				{
					forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
				}
			}
			else if (actualAlgorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				using (ChaCha20 forEncrypting = new ChaCha20(key, this.settingsChaCha20.nonce, settingsChaCha20.counter))
				{
					forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
				}
			}
			else
			{
				throw new NotImplementedException();
			}

			return returnArray;
		}

		/// <summary>
		/// Get symmetric encryption algorithm
		/// </summary>
		/// <returns></returns>
		public SymmetricEncryptionAlgorithm GetSymmetricEncryptionAlgorithm()
		{
			if (Enum.TryParse(this.algorithm, out SymmetricEncryptionAlgorithm actualAlgorithm))
			{
				return actualAlgorithm;
			}

			throw new Exception("Cannot parse algorithm");
		}

		/// <summary>
		/// Get settings as byte array
		/// </summary>
		/// <returns>Byte array</returns>
		public byte[] GetSettingsAsBytes()
		{
			byte[] returnValue = null;

			Enum.TryParse(this.algorithm, out SymmetricEncryptionAlgorithm actualAlgorithm);

			if (actualAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				returnValue = ChecksumHelper.JoinByteArrays(
										Encoding.UTF8.GetBytes(this.algorithm), 
										BitConverter.GetBytes(this.keySizeInBits), 
										settingsAES_CTR.GetSettingsAsBytes()
										);
			}
			else if (actualAlgorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				returnValue = ChecksumHelper.JoinByteArrays(
										Encoding.UTF8.GetBytes(this.algorithm), 
										BitConverter.GetBytes(this.keySizeInBits), 
										settingsChaCha20.GetSettingsAsBytes()
										);
			}
			else
			{
				throw new NotImplementedException();
			}

			return returnValue;
		}

		/// <summary>
		/// Generate new SymmetricKeyAlgorithm, you should use this instead of constructor
		/// </summary>
		/// <param name="symmetricEncryptionAlgorithm">Wanted Symmetric encryption algorithm</param>
		/// <returns>SymmetricKeyAlgorithm</returns>
		public static SymmetricKeyAlgorithm GenerateNew(SymmetricEncryptionAlgorithm symmetricEncryptionAlgorithm)
		{
			return new SymmetricKeyAlgorithm(symmetricEncryptionAlgorithm, 256, (symmetricEncryptionAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR ) ? (object)SettingsAES_CTR.CreateWithCryptographicRandomNumbers() : (object)SettingsChaCha20.CreateWithCryptographicRandomNumbers() );
		}
	}

	/// <summary>
	/// AES CTR settings
	/// </summary>
	public sealed class SettingsAES_CTR
	{
		/// <summary>
		/// Initial counter as byte array
		/// </summary>
		public byte[] initialCounter { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SettingsAES_CTR()
		{

		}

		/// <summary>
		/// Default constructor for SettingsAES_CTR
		/// </summary>
		/// <param name="initialCounter">Byte array of initial counter</param>
		public SettingsAES_CTR(byte[] initialCounter)
		{
			if (initialCounter == null)
			{
				throw new NullReferenceException("Initial counter cannot be null!");
			}
			else if (initialCounter.Length != AES_CTR.allowedCounterLength)
			{
				throw new ArgumentException($"Initial counter only allows length of {AES_CTR.allowedCounterLength} bytes!");
			}

			this.initialCounter = initialCounter;
		}

		/// <summary>
		/// Get settings as byte array
		/// </summary>
		/// <returns>Byte array</returns>
		public byte[] GetSettingsAsBytes()
		{
			// Since AES_CTR settings only contains initial counter, return it
			return this.initialCounter;
		}

		/// <summary>
		/// Create SettingsAES_CTR with Cryptographic random numbers, you should use this instead of constructor
		/// </summary>
		/// <returns>SettingsAES_CTR</returns>
		public static SettingsAES_CTR CreateWithCryptographicRandomNumbers()
		{
			byte[] initialCounter = new byte[AES_CTR.allowedCounterLength];

			using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
			{
				rngCsp.GetBytes(initialCounter);
			}

			return new SettingsAES_CTR(initialCounter); 
		}
	}

	/// <summary>
	/// ChaCha20 settings
	/// </summary>
	public sealed class SettingsChaCha20
	{
		/// <summary>
		/// Nonce byte array
		/// </summary>
		public byte[] nonce { get; set; }

		/// <summary>
		/// Counter
		/// </summary>
		public uint counter { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SettingsChaCha20()
		{

		}

		/// <summary>
		/// Default constructor for SettingsChaCha20
		/// </summary>
		/// <param name="nonce">Nonce as byte array</param>
		/// <param name="counter">Counter</param>
		public SettingsChaCha20(byte[] nonce, uint counter)
		{
			if (nonce == null)
			{
				throw new NullReferenceException("Nonce cannot be null!");
			}
			else if (nonce.Length != ChaCha20.allowedNonceLength)
			{
				throw new ArgumentException($"Nonce only allows length of {ChaCha20.allowedNonceLength} bytes!");
			}

			this.nonce = nonce;
			this.counter = counter;
		}

		/// <summary>
		/// Increase nonce
		/// </summary>
		public void IncreaseNonce()
		{
			int index = 0;
			bool done = false;
			while (!done && index < this.nonce.Length)
			{
				if (this.nonce[index] < byte.MaxValue)
				{
					this.nonce[index]++;
					done = true;
				}
				else
				{
					this.nonce[index] = 0;
				}
				index++;
			}
		}

		/// <summary>
		/// Get settings as byte array
		/// </summary>
		/// <returns>Byte array</returns>
		public byte[] GetSettingsAsBytes()
		{
			// Join them together
			return ChecksumHelper.JoinByteArrays(this.nonce, BitConverter.GetBytes(counter));
		}

		/// <summary>
		/// Create SettingsChaCha20 with Cryptographic random numbers, you should use this instead of constructor
		/// </summary>
		/// <returns>SettingsChaCha20</returns>
		public static SettingsChaCha20 CreateWithCryptographicRandomNumbers()
		{
			byte[] nonce = new byte[ChaCha20.allowedNonceLength];
			byte[] uintBytes = new byte[4];

			using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
			{
				rngCsp.GetBytes(nonce, 0, 8);
				rngCsp.GetBytes(uintBytes, 0, 3);
			}

			return new SettingsChaCha20(nonce, BitConverter.ToUInt32(uintBytes, 0)); 
		}
	}
}