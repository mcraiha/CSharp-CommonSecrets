#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using CS_AES_CTR;
using CSChaCha20;
using System.Security.Cryptography;

using System.Threading.Tasks;

namespace Tests
{
	public class SymmetricKeyAlgorithmAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		/// <summary>
		/// 32 bytes equals 256 bits
		/// </summary>
		private const int validChaCha20KeyLength = 32;

		[Test]
		public async Task AES_CTR_AsyncTest()
		{
			// Arrange
			byte[] key = new byte[16] { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			byte[] content = new byte[] { 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a };
			byte[] expected = new byte[] { 0x87, 0x4d, 0x61, 0x91, 0xb6, 0x20, 0xe3, 0x26, 0x1b, 0xef, 0x68, 0x64, 0x99, 0x0d, 0xb6, 0xce };

			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			// Act
			byte[] output1 = await skaAES_CTR.EncryptBytesAsync(content, key, securityAsyncFunctions);
			byte[] output2 = await skaAES_CTR.DecryptBytesAsync(content, key, securityAsyncFunctions);

			// Assert
			CollectionAssert.AreEqual(expected, output1);
			CollectionAssert.AreEqual(expected, output2);
		}
	
	}
}

#endif