using System;
using System.Text;


namespace CSCommonSecrets
{
	/// <summary>
	/// Symmetric Encryption Algorithm enum
	/// </summary>
	public enum SymmetricEncryptionAlgorithm
	{
		/// AES in CTR mode
		AES_CTR = 0,
		/// ChaCha20
		ChaCha20
	}

	/// <summary>
	/// Symmetric key algorithm contains all needed info for encryption/decryption
	/// </summary>
	public sealed partial class SymmetricKeyAlgorithm
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

		private static readonly int[] AES_CTR_AllowedKeyLengths = new int[3] { 16, 24, 32 };

		private static readonly int ChaCha20_AllowedKeyLength = 32;

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
				if (!Array.Exists(AES_CTR_AllowedKeyLengths, allowed => allowed * 8 == keySizeInBits))
				{
					throw new ArgumentException($"{keySizeInBits} is not valid AES-CTR key size!");
				}

				this.settingsAES_CTR = (SettingsAES_CTR)settings;
			}
			else if (algorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				if (ChaCha20_AllowedKeyLength * 8 != keySizeInBits)
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
		/// Deep copy constructor
		/// </summary>
		/// <param name="copyThis">SymmetricKeyAlgorithm to copy</param>
		public SymmetricKeyAlgorithm(SymmetricKeyAlgorithm copyThis)
		{
			this.algorithm = copyThis.algorithm;
			this.keySizeInBits = copyThis.keySizeInBits;

			if (copyThis.settingsAES_CTR != null)
			{
				this.settingsAES_CTR = SettingsAES_CTR.CreateDeepCopy(copyThis.settingsAES_CTR.initialCounter);
			}

			if (copyThis.settingsChaCha20 != null)
			{
				this.settingsChaCha20 = SettingsChaCha20.CreateDeepCopy(copyThis.settingsChaCha20.nonce, copyThis.settingsChaCha20.counter);
			}
		}

		/// <summary>
		/// Get symmetric encryption algorithm
		/// </summary>
		/// <returns>SymmetricEncryptionAlgorithm enum</returns>
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
	}

	/// <summary>
	/// AES CTR settings
	/// </summary>
	public sealed partial class SettingsAES_CTR
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
		/// Create deep copy of existing SettingsAES_CTR
		/// </summary>
		/// <param name="initialCounter">Initial counter</param>
		/// <returns>Copy of SettingsAES_CTR</returns>
		public static SettingsAES_CTR CreateDeepCopy(byte[] initialCounter)
		{
			return new SettingsAES_CTR() { initialCounter = initialCounter };
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
	}

	/// <summary>
	/// ChaCha20 settings
	/// </summary>
	public sealed partial class SettingsChaCha20
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
		/// Create deep copy of existing SettingsChaCha20
		/// </summary>
		/// <param name="nonce">Nonce as byte array</param>
		/// <param name="counter">Counter</param>
		/// <returns>Copy of SettingsChaCha20</returns>
		public static SettingsChaCha20 CreateDeepCopy(byte[] nonce, uint counter)
		{
			return new SettingsChaCha20() { nonce = nonce, counter = counter };
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
	}
}