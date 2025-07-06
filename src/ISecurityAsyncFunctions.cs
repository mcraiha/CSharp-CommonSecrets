// This file is only used with ASYNC_WITH_CUSTOM definition

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;

namespace CSCommonSecrets;

/// <summary>
/// Interface for security related functions
/// </summary>
public interface ISecurityAsyncFunctions
{
	/// <summary>
	/// Get AES CTR Allowed Counter Length
	/// </summary>
	/// <returns>Length</returns>
	int AES_CTRAllowedCounterLength();

	/// <summary>
	/// Encrypt/decrypt AES bytes with given key and initial counter, async
	/// </summary>
	/// <param name="bytesToEncrypt">Bytes to encrypt</param>
	/// <param name="key">Key</param>
	/// <param name="initialCounter">Initial counter</param>
	/// <returns>Encrypted/decrypted Byte array</returns>
	Task<byte[]> AES_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] initialCounter);

	/// <summary>
	/// Get ChaCha20 Allowed Nonce Length
	/// </summary>
	/// <returns>Length</returns>
	int ChaCha20AllowedNonceLength();

	/// <summary>
	/// Encrypt/decrypt ChaCha20 bytes with given key, nonce and counter, async
	/// </summary>
	/// <param name="bytesToEncrypt">Bytes to encrypt</param>
	/// <param name="key">Key</param>
	/// <param name="nonce">Nonce</param>
	/// <param name="counter">Counter</param>
	/// <returns></returns>
	Task<byte[]> ChaCha20_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] nonce, uint counter);

	/// <summary>
	/// Password-Based Key Derivation Function 2, async
	/// </summary>
	/// <param name="password">Password as string</param>
	/// <param name="salt">Salt</param>
	/// <param name="prf">Key derivation algorithm</param>
	/// <param name="iterationCount">Iteration count</param>
	/// <param name="numBytesRequested">Number of bytes requested</param>
	/// <returns></returns>
	Task<byte[]> Pbkdf2(string password, byte[] salt, KeyDerivationPseudoRandomFunction prf, int iterationCount, int numBytesRequested);

	/// <summary>
	/// Hash byte array with SHA256, async
	/// </summary>
	/// <param name="bytesToHash">Bytes to hash</param>
	/// <returns></returns>
	Task<byte[]> SHA256_Hash(byte[] bytesToHash);

	/// <summary>
	/// Generate random bytes to given byte array
	/// </summary>
	/// <param name="byteArray">Byte array</param>
	void GenerateSecureRandomBytes(byte[] byteArray);

	/// <summary>
	/// Generate random bytes to given byte array
	/// </summary>
	/// <param name="byteArray">Byte array</param>
	/// <param name="offset">Offset to byte array</param>
	/// <param name="count">How many bytes should be replaced</param>
	void GenerateSecureRandomBytes(byte[] byteArray, int offset, int count);
}

#endif // ASYNC_WITH_CUSTOM