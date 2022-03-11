// This file is only used with ASYNC_WITH_CUSTOM definition

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;

/// <summary>
/// Interface for security related functions
/// </summary>
public interface ISecurityAsyncFunctions
{
	/// <summary>
	/// Encrypt/decrypt AES bytes with given key and initial counter, async
	/// </summary>
	/// <param name="bytesToEncrypt">Bytes to encrypt</param>
	/// <param name="key">Key</param>
	/// <param name="initialCounter">Initial counter</param>
	/// <returns>Encrypted/decrypted Byte array</returns>
	Task<byte[]> AES_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] initialCounter);

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