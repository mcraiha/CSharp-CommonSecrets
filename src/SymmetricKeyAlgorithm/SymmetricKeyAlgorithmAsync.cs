#if ASYNC_WITH_CUSTOM
using System;
using System.Text;
using System.Security.Cryptography;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Symmetric key algorithm contains all needed info for encryption/decryption
	/// </summary>
	public sealed partial class SymmetricKeyAlgorithm
	{
		/// <summary>
		/// Encrypt given bytes with given key. Returns new array with encrypted bytes, async
		/// </summary>
		/// <param name="bytesToEncrypt">Byte array to encrypt</param>
		/// <param name="key">Key</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Encrypted bytes in new array</returns>
		public async Task<byte[]> EncryptBytesAsync(byte[] bytesToEncrypt, byte[] key, ISecurityAsyncFunctions securityFunctions)
		{
			Enum.TryParse(this.algorithm, out SymmetricEncryptionAlgorithm actualAlgorithm);

			if (actualAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				return await securityFunctions.AES_Encrypt(bytesToEncrypt, key, this.settingsAES_CTR.initialCounter);
			}
			else if (actualAlgorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				return await securityFunctions.ChaCha20_Encrypt(bytesToEncrypt, key, this.settingsChaCha20.nonce, this.settingsChaCha20.counter);
			}

			throw new NotImplementedException();
		}

		/// <summary>
		/// Decrypt given bytes with given key. Returns new array with decrypted bytes, async
		/// </summary>
		/// <param name="bytesToDecrypt">Byte array to decrypt</param>
		/// <param name="key">Key</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>Decrypted bytes in new array</returns>
		public async Task<byte[]> DecryptBytesAsync(byte[] bytesToDecrypt, byte[] key, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.EncryptBytesAsync(bytesToDecrypt, key, securityFunctions);
		}

		/// <summary>
		/// Generate new SymmetricKeyAlgorithm, you should use this instead of constructor
		/// </summary>
		/// <param name="symmetricEncryptionAlgorithm">Wanted Symmetric encryption algorithm</param>
		/// <param name="securityFunctions">Security functions</param>
		/// <returns>SymmetricKeyAlgorithm</returns>
		public static SymmetricKeyAlgorithm GenerateNew(SymmetricEncryptionAlgorithm symmetricEncryptionAlgorithm, ISecurityAsyncFunctions securityFunctions)
		{
			return new SymmetricKeyAlgorithm(symmetricEncryptionAlgorithm, 256, (symmetricEncryptionAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR ) ? (object)SettingsAES_CTR.CreateWithCryptographicRandomNumbers(securityFunctions) : (object)SettingsChaCha20.CreateWithCryptographicRandomNumbers(securityFunctions) );
		}
	}

	/// <summary>
	/// AES CTR settings
	/// </summary>
	public sealed partial class SettingsAES_CTR
	{
		/// <summary>
		/// Default constructor for SettingsAES_CTR
		/// </summary>
		/// <param name="initialCounter">Byte array of initial counter</param>
		/// <param name="securityFunctions">Security functions</param>
		public SettingsAES_CTR(byte[] initialCounter, ISecurityAsyncFunctions securityFunctions)
		{
			if (initialCounter == null)
			{
				throw new ArgumentNullException("Initial counter cannot be null!");
			}
			else if (initialCounter.Length != securityFunctions.AES_CTRAllowedCounterLength())
			{
				throw new ArgumentException($"Initial counter only allows length of {securityFunctions.AES_CTRAllowedCounterLength()} bytes!");
			}

			this.initialCounter = initialCounter;
		}

		/// <summary>
		/// Create SettingsAES_CTR with Cryptographic random numbers, you should use this instead of constructor
		/// </summary>
		/// <returns>SettingsAES_CTR</returns>
		public static SettingsAES_CTR CreateWithCryptographicRandomNumbers(ISecurityAsyncFunctions securityFunctions)
		{
			byte[] initialCounter = new byte[securityFunctions.AES_CTRAllowedCounterLength()];

			securityFunctions.GenerateSecureRandomBytes(initialCounter);

			return new SettingsAES_CTR(initialCounter, securityFunctions); 
		}
	}

	/// <summary>
	/// ChaCha20 settings
	/// </summary>
	public sealed partial class SettingsChaCha20
	{
		/// <summary>
		/// Default constructor for SettingsChaCha20
		/// </summary>
		/// <param name="nonce">Nonce as byte array</param>
		/// <param name="counter">Counter</param>
		/// <param name="securityFunctions">Security functions</param>
		public SettingsChaCha20(byte[] nonce, uint counter, ISecurityAsyncFunctions securityFunctions)
		{
			if (nonce == null)
			{
				throw new ArgumentNullException("Nonce cannot be null!");
			}
			else if (nonce.Length != securityFunctions.ChaCha20AllowedNonceLength())
			{
				throw new ArgumentException($"Nonce only allows length of {securityFunctions.ChaCha20AllowedNonceLength()} bytes!");
			}

			this.nonce = nonce;
			this.counter = counter;
		}

		/// <summary>
		/// Create SettingsChaCha20 with Cryptographic random numbers, you should use this instead of constructor
		/// </summary>
		/// <returns>SettingsChaCha20</returns>
		public static SettingsChaCha20 CreateWithCryptographicRandomNumbers(ISecurityAsyncFunctions securityFunctions)
		{
			byte[] nonce = new byte[securityFunctions.ChaCha20AllowedNonceLength()];
			byte[] uintBytes = new byte[4];

			securityFunctions.GenerateSecureRandomBytes(nonce, 0, 8);
			securityFunctions.GenerateSecureRandomBytes(uintBytes, 0, 3);

			return new SettingsChaCha20(nonce, BitConverter.ToUInt32(uintBytes, 0), securityFunctions); 
		}
	}
}

#endif // ASYNC_WITH_CUSTOM