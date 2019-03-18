using System;
using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

namespace CSCommonSecrets
{
	public enum SymmetricEncryptionAlgorithm
	{
		AES_CTR,
		ChaCha20
	}

	public sealed class SymmetricKeyAlgorithm
	{
		public SymmetricEncryptionAlgorithm algorithm { get; set; }

		public int keySizeInBits { get; set; }

		public SettingsAES_CTR settingsAES_CTR { get; set; }

		public SettingsChaCha20 settingsChaCha20 { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SymmetricKeyAlgorithm()
		{

		}

		public SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm algorithm, int keySizeInBits, object settings)
		{
			this.algorithm = algorithm;

			if (this.algorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				if (!Array.Exists(AES_CTR.allowedKeyLengths, allowed => allowed * 8 == keySizeInBits))
				{
					throw new ArgumentException($"{keySizeInBits} is not valid AES-CTR key size!");
				}

				this.settingsAES_CTR = (SettingsAES_CTR)settings;
			}
			else if (this.algorithm == SymmetricEncryptionAlgorithm.ChaCha20)
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

		public byte[] EncryptBytes(byte[] bytesToEncrypt, byte[] key)
		{
			byte[] returnArray = new byte[bytesToEncrypt.Length];

			if (this.algorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				using (AES_CTR forEncrypting = new AES_CTR(key, this.settingsAES_CTR.initialCounter))
				{
					forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
				}
			}
			else if (this.algorithm == SymmetricEncryptionAlgorithm.ChaCha20)
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

		public byte[] GetSettingsAsBytes()
		{
			byte[] returnValue = null;

			if (this.algorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				returnValue = ChecksumHelper.JoinByteArrays(
										BitConverter.GetBytes((int)SymmetricEncryptionAlgorithm.AES_CTR), 
										BitConverter.GetBytes(keySizeInBits), 
										settingsAES_CTR.GetSettingsAsBytes()
										);
			}
			else if (this.algorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				returnValue = ChecksumHelper.JoinByteArrays(
										BitConverter.GetBytes((int)SymmetricEncryptionAlgorithm.ChaCha20), 
										BitConverter.GetBytes(keySizeInBits), 
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

	public sealed class SettingsAES_CTR
	{
		public byte[] initialCounter { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SettingsAES_CTR()
		{

		}

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

		public byte[] GetSettingsAsBytes()
		{
			// Since AES_CTR settings only contains initial counter, return it
			return this.initialCounter;
		}
	}

	public sealed class SettingsChaCha20
	{
		public byte[] nonce { get; set; }
		public uint counter { get; set; }

		/// <summary>
		/// For (de)serialization
		/// </summary>
		public SettingsChaCha20()
		{

		}

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

		public byte[] GetSettingsAsBytes()
		{
			// Join them together
			return ChecksumHelper.JoinByteArrays(this.nonce, BitConverter.GetBytes(counter));
		}

		public static SettingsChaCha20 CreateWithCryptographicRandomNumbers()
		{
			byte[] nonce = new byte[ChaCha20.allowedNonceLength];
			byte[] uintBytes = new byte[4];

			using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
			{
				rngCsp.GetBytes(nonce, 0, 8);
				rngCsp.GetBytes(uintBytes, 0, 2);
			}

			return new SettingsChaCha20(nonce, BitConverter.ToUInt32(uintBytes, 0)); 
		}
	}
}