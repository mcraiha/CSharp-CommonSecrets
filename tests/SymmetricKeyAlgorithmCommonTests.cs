using NUnit.Framework;
using CSCommonSecrets;

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;
#endif

namespace Tests
{
	public class SymmetricKeyAlgorithmCommonTests
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
		public void SymmetricKeyAlgorithmGetAsBytesTest()
		{
			// Arrange
			byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			byte[] initialCounter2 = new byte[] { 0xf1, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR1 = new SettingsAES_CTR(initialCounter1);
			SettingsAES_CTR settingsAES_CTR2 = new SettingsAES_CTR(initialCounter2);

			SymmetricKeyAlgorithm skaAES_CTR1 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR1);
			SymmetricKeyAlgorithm skaAES_CTR2 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR1);
			SymmetricKeyAlgorithm skaAES_CTR3 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 256, settingsAES_CTR1);

			SymmetricKeyAlgorithm skaAES_CTR4 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR2);

			// Act
			byte[] skaAES_CTR1bytes = skaAES_CTR1.GetSettingsAsBytes();
			byte[] skaAES_CTR2bytes = skaAES_CTR2.GetSettingsAsBytes();
			byte[] skaAES_CTR3bytes = skaAES_CTR3.GetSettingsAsBytes();
			byte[] skaAES_CTR4bytes = skaAES_CTR4.GetSettingsAsBytes();

			// Assert
			Assert.IsNotNull(skaAES_CTR1bytes);
			Assert.IsNotNull(skaAES_CTR2bytes);
			Assert.IsNotNull(skaAES_CTR3bytes);
			Assert.IsNotNull(skaAES_CTR4bytes);

			CollectionAssert.AreEqual(skaAES_CTR1bytes, skaAES_CTR2bytes);
			CollectionAssert.AreNotEqual(skaAES_CTR1bytes, skaAES_CTR3bytes);
			CollectionAssert.AreNotEqual(skaAES_CTR1bytes, skaAES_CTR4bytes);
		}

		[Test]
		public void SettingsAES_CTRGetAsBytesTest()
		{
			// Arrange
			byte[] initialCounter1 = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };
			byte[] initialCounter2 = new byte[] { 0xf0, 0xff, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR_1 = new SettingsAES_CTR(initialCounter1);
			SettingsAES_CTR settingsAES_CTR_2 = new SettingsAES_CTR(initialCounter2);

			// Act
			byte[] bytes1 = settingsAES_CTR_1.GetSettingsAsBytes();
			byte[] bytes2 = settingsAES_CTR_2.GetSettingsAsBytes();

			// Assert
			Assert.IsNotNull(bytes1);
			Assert.IsNotNull(bytes2);
			CollectionAssert.AreNotEqual(bytes1, bytes2);
		}

		[Test]
		public void SettingsChaCha20GetAsBytesTest()
		{
			// Arrange
			SettingsChaCha20 settingsChaCha20_1 = new SettingsChaCha20(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 }, 1);
			SettingsChaCha20 settingsChaCha20_2 = new SettingsChaCha20(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x5a, 0x00, 0x00, 0x00, 0x00 }, 1);
			SettingsChaCha20 settingsChaCha20_3 = new SettingsChaCha20(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 }, 2);

			// Act
			byte[] bytes1 = settingsChaCha20_1.GetSettingsAsBytes();
			byte[] bytes2 = settingsChaCha20_2.GetSettingsAsBytes();
			byte[] bytes3 = settingsChaCha20_3.GetSettingsAsBytes();

			// Assert
			Assert.IsNotNull(bytes1);
			Assert.IsNotNull(bytes2);
			Assert.IsNotNull(bytes3);

			CollectionAssert.AreNotEqual(bytes1, bytes2);
			CollectionAssert.AreNotEqual(bytes1, bytes3);
		}

		[Test]
		public void SettingsChaCha20IncreaseNonceTest()
		{
			// Arrange
			SettingsChaCha20 settingsChaCha20_1 = new SettingsChaCha20(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 }, 1);
			byte[] expected1 = new byte[] { 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 };

			SettingsChaCha20 settingsChaCha20_2 = new SettingsChaCha20(new byte[] { 0xFF, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0xFF }, 1);
			byte[] expected2 = new byte[] { 0x00, 0x01, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0xFF };

			SettingsChaCha20 settingsChaCha20_3 = new SettingsChaCha20(new byte[] { 0xFF, 0xFF, 0x01, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF }, 1);
			byte[] expected3 = new byte[] { 0x00, 0x00, 0x02, 0x00, 0x00, 0x00, 0x00, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };

			// Act
			settingsChaCha20_1.IncreaseNonce();
			settingsChaCha20_2.IncreaseNonce();
			settingsChaCha20_3.IncreaseNonce();

			// Assert
			CollectionAssert.AreEqual(expected1, settingsChaCha20_1.nonce);
			CollectionAssert.AreEqual(expected2, settingsChaCha20_2.nonce);
			CollectionAssert.AreEqual(expected3, settingsChaCha20_3.nonce);
		}
	}
}