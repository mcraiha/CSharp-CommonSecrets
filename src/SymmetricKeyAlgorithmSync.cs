#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using System;
using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

namespace CSCommonSecrets
{

	public sealed partial class SymmetricKeyAlgorithm
	{
		/// <summary>
		/// Encrypt given bytes with given key. Returns new array with encrypted bytes
		/// </summary>
		/// <param name="bytesToEncrypt">Byte array to encrypt</param>
		/// <param name="key">Key</param>
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
		/// Decrypt given bytes with given key. Returns new array with decrypted bytes. Same result as EncryptBytes
		/// </summary>
		/// <param name="bytesToDecrypt">Byte array to decrypt</param>
		/// <param name="key">Key</param>
		/// <returns>Decrypted bytes in new array</returns>
		public byte[] DecryptBytes(byte[] bytesToDecrypt, byte[] key)
		{
			return this.EncryptBytes(bytesToDecrypt, key);
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
	public sealed partial class SettingsAES_CTR
	{
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
	public sealed partial class SettingsChaCha20
	{
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

#endif // ASYNC_WITH_CUSTOM && WITH_CUSTOM