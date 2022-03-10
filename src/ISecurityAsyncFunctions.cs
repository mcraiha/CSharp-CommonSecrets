// This file is only used with ASYNC_WITH_CUSTOM definition

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;

/// <summary>
/// Interface for security related functions
/// </summary>
public interface ISecurityAsyncFunctions
{
	Task<byte[]> AES_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] initialCounter);

	Task<byte[]> ChaCha20_Encrypt(byte[] bytesToEncrypt, byte[] key, byte[] nonce, uint counter);

	Task<byte[]> SHA256_Hash(byte[] bytesToHash);

	void GenerateSecureRandomBytes(byte[] byteArray);

	void GenerateSecureRandomBytes(byte[] byteArray, int offset, int count);
}

#endif // ASYNC_WITH_CUSTOM