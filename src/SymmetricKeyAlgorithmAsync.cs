#if ASYNC_WITH_CUSTOM
using System;
using System.Text;
using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

using System.Threading.Tasks;

namespace CSCommonSecrets
{
	/// <summary>
	/// Symmetric key algorithm contains all needed info for encryption/decryption
	/// </summary>
	public sealed partial class SymmetricKeyAlgorithm
	{
		public async Task<byte[]> EncryptBytesAsync(byte[] bytesToEncrypt, byte[] key, ISecurityAsyncFunctions securityFunctions)
		{
			Enum.TryParse(this.algorithm, out SymmetricEncryptionAlgorithm actualAlgorithm);

			if (actualAlgorithm == SymmetricEncryptionAlgorithm.AES_CTR)
			{
				return await securityFunctions.AES_Encrypt(bytesToEncrypt, key, this.settingsAES_CTR.initialCounter);
			}
			else if (actualAlgorithm == SymmetricEncryptionAlgorithm.ChaCha20)
			{
				/*using (ChaCha20 forEncrypting = new ChaCha20(key, this.settingsChaCha20.nonce, settingsChaCha20.counter))
				{
					forEncrypting.EncryptBytes(returnArray, bytesToEncrypt, bytesToEncrypt.Length);
				}*/
			}
			else
			{
				throw new NotImplementedException();
			}

			return null;
		}

		public async Task<byte[]> DecryptBytesAsync(byte[] bytesToDecrypt, byte[] key, ISecurityAsyncFunctions securityFunctions)
		{
			return await this.EncryptBytesAsync(bytesToDecrypt, key, securityFunctions);
		}

		/// <summary>
		/// Generate new SymmetricKeyAlgorithm, you should use this instead of constructor
		/// </summary>
		/// <param name="symmetricEncryptionAlgorithm">Wanted Symmetric encryption algorithm</param>
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
		/// Create SettingsAES_CTR with Cryptographic random numbers, you should use this instead of constructor
		/// </summary>
		/// <returns>SettingsAES_CTR</returns>
		public static SettingsAES_CTR CreateWithCryptographicRandomNumbers(ISecurityAsyncFunctions securityFunctions)
		{
			byte[] initialCounter = new byte[AES_CTR.allowedCounterLength];

			securityFunctions.GenerateSecureRandomBytes(initialCounter);

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
		public static SettingsChaCha20 CreateWithCryptographicRandomNumbers(ISecurityAsyncFunctions securityFunctions)
		{
			byte[] nonce = new byte[ChaCha20.allowedNonceLength];
			byte[] uintBytes = new byte[4];

			securityFunctions.GenerateSecureRandomBytes(nonce, 0, 8);
			securityFunctions.GenerateSecureRandomBytes(uintBytes, 0, 3);

			return new SettingsChaCha20(nonce, BitConverter.ToUInt32(uintBytes, 0)); 
		}
	}
}

#endif // ASYNC_WITH_CUSTOM