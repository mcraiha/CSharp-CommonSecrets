using NUnit.Framework;
using CSCommonSecrets;

namespace Tests
{
	public class SymmetricKeyAlgorithmTests
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
		public void ChaCha20Test()
		{
			// Arrange
			SettingsChaCha20 settingsChaCha20 = new SettingsChaCha20(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x4a, 0x00, 0x00, 0x00, 0x00 }, 1);
			SymmetricKeyAlgorithm skaChaCha20 = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.ChaCha20, 256, settingsChaCha20);

			byte[] key = new byte[validChaCha20KeyLength] { 
														0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
														0x08, 0x09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 
														0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 
														0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f 
													};

			const int lengthOfContent2 = 114;
			byte[] content = new byte[lengthOfContent2] { 
														0x4c, 0x61, 0x64, 0x69, 0x65, 0x73, 0x20, 0x61, 
														0x6e, 0x64, 0x20, 0x47, 0x65, 0x6e, 0x74, 0x6c,
														0x65, 0x6d, 0x65, 0x6e, 0x20, 0x6f, 0x66, 0x20, 
														0x74, 0x68, 0x65, 0x20, 0x63, 0x6c, 0x61, 0x73,
														0x73, 0x20, 0x6f, 0x66, 0x20, 0x27, 0x39, 0x39, 
														0x3a, 0x20, 0x49, 0x66, 0x20, 0x49, 0x20, 0x63,
														0x6f, 0x75, 0x6c, 0x64, 0x20, 0x6f, 0x66, 0x66, 
														0x65, 0x72, 0x20, 0x79, 0x6f, 0x75, 0x20, 0x6f,
														0x6e, 0x6c, 0x79, 0x20, 0x6f, 0x6e, 0x65, 0x20, 
														0x74, 0x69, 0x70, 0x20, 0x66, 0x6f, 0x72, 0x20,
														0x74, 0x68, 0x65, 0x20, 0x66, 0x75, 0x74, 0x75, 
														0x72, 0x65, 0x2c, 0x20, 0x73, 0x75, 0x6e, 0x73,
														0x63, 0x72, 0x65, 0x65, 0x6e, 0x20, 0x77, 0x6f, 
														0x75, 0x6c, 0x64, 0x20, 0x62, 0x65, 0x20, 0x69,
														0x74, 0x2e 
														};
			byte[] expected = new byte[lengthOfContent2] { 
															0x6e, 0x2e, 0x35, 0x9a, 0x25, 0x68, 0xf9, 0x80, 0x41, 0xba, 0x07, 0x28, 0xdd, 0x0d, 0x69, 0x81,
															0xe9, 0x7e, 0x7a, 0xec, 0x1d, 0x43, 0x60, 0xc2, 0x0a, 0x27, 0xaf, 0xcc, 0xfd, 0x9f, 0xae, 0x0b,
															0xf9, 0x1b, 0x65, 0xc5, 0x52, 0x47, 0x33, 0xab, 0x8f, 0x59, 0x3d, 0xab, 0xcd, 0x62, 0xb3, 0x57,
															0x16, 0x39, 0xd6, 0x24, 0xe6, 0x51, 0x52, 0xab, 0x8f, 0x53, 0x0c, 0x35, 0x9f, 0x08, 0x61, 0xd8,
															0x07, 0xca, 0x0d, 0xbf, 0x50, 0x0d, 0x6a, 0x61, 0x56, 0xa3, 0x8e, 0x08, 0x8a, 0x22, 0xb6, 0x5e,
															0x52, 0xbc, 0x51, 0x4d, 0x16, 0xcc, 0xf8, 0x06, 0x81, 0x8c, 0xe9, 0x1a, 0xb7, 0x79, 0x37, 0x36,
															0x5a, 0xf9, 0x0b, 0xbf, 0x74, 0xa3, 0x5b, 0xe6, 0xb4, 0x0b, 0x8e, 0xed, 0xf2, 0x78, 0x5e, 0x42,
															0x87, 0x4d
															};

			// Act
			byte[] output = skaChaCha20.EncryptBytes(content, key);

			// Assert
			CollectionAssert.AreEqual(expected, output);
		}

		[Test]
		public void AES_CTR_Test()
		{
			// Arrange
			byte[] key = new byte[16] { 0x2b, 0x7e, 0x15, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0xf7, 0x15, 0x88, 0x09, 0xcf, 0x4f, 0x3c };
			byte[] initialCounter = new byte[] { 0xf0, 0xf1, 0xf2, 0xf3, 0xf4, 0xf5, 0xf6, 0xf7, 0xf8, 0xf9, 0xfa, 0xfb, 0xfc, 0xfd, 0xfe, 0xff };

			SettingsAES_CTR settingsAES_CTR = new SettingsAES_CTR(initialCounter);

			SymmetricKeyAlgorithm skaAES_CTR = new SymmetricKeyAlgorithm(SymmetricEncryptionAlgorithm.AES_CTR, 128, settingsAES_CTR);

			byte[] content = new byte[] { 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a };
			byte[] expected = new byte[] { 0x87, 0x4d, 0x61, 0x91, 0xb6, 0x20, 0xe3, 0x26, 0x1b, 0xef, 0x68, 0x64, 0x99, 0x0d, 0xb6, 0xce };

			// Act
			byte[] output = skaAES_CTR.EncryptBytes(content, key);

			// Assert
			CollectionAssert.AreEqual(expected, output);
		}

		[Test]
		public void GenerateNewTest()
		{
			// Arrange
			byte[] keyAES = new byte[16] { 0x2b, 0x7e, 0x12, 0x16, 0x28, 0xae, 0xd2, 0xa6, 0xab, 0x17, 0x15, 0x88, 0x09, 0xcf, 0x43, 0x3c };
			byte[] keyChaCha20 = new byte[validChaCha20KeyLength] { 
														0x07, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 
														0x08, 0x59, 0x0a, 0x0b, 0x2c, 0x0d, 0x0e, 0x0f, 
														0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17, 
														0x18, 0x19, 0x1a, 0x1b, 0x1c, 0x1d, 0x1e, 0x1f 
													};
			byte[] content = new byte[] { 0x6b, 0xc1, 0xbe, 0xe2, 0x2e, 0x40, 0x9f, 0x96, 0xe9, 0x3d, 0x7e, 0x11, 0x73, 0x93, 0x17, 0x2a };

			SymmetricKeyAlgorithm skaAES = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.AES_CTR);
			SymmetricKeyAlgorithm skaChaCha20 = SymmetricKeyAlgorithm.GenerateNew(SymmetricEncryptionAlgorithm.ChaCha20);

			// Act
			byte[] outputAES = skaAES.EncryptBytes(content, keyAES);
			byte[] outputChaCha20 = skaChaCha20.EncryptBytes(content, keyChaCha20);

			byte[] decryptedAES = skaAES.EncryptBytes(outputAES,keyAES);
			byte[] decryptedChaCha20 = skaChaCha20.EncryptBytes(outputChaCha20, keyChaCha20);

			// Assert
			Assert.IsNotNull(skaAES);
			Assert.IsNotNull(skaAES.settingsAES_CTR);
			Assert.IsNull(skaAES.settingsChaCha20);

			Assert.IsNotNull(skaChaCha20);
			Assert.IsNotNull(skaChaCha20.settingsChaCha20);
			Assert.IsNull(skaChaCha20.settingsAES_CTR);

			CollectionAssert.AreNotEqual(content, outputAES);
			CollectionAssert.AreNotEqual(content, outputChaCha20);

			CollectionAssert.AreEqual(content, decryptedAES);
			CollectionAssert.AreEqual(content, decryptedChaCha20);
		}

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

		[Test]
		public void CreateSettingsChaCha20WithCryptographicRandomNumbersTest()
		{
			// Arrange
			SettingsChaCha20 settingsChaCha20_1 = SettingsChaCha20.CreateWithCryptographicRandomNumbers();
			SettingsChaCha20 settingsChaCha20_2 = SettingsChaCha20.CreateWithCryptographicRandomNumbers();
			SettingsChaCha20 settingsChaCha20_3 = SettingsChaCha20.CreateWithCryptographicRandomNumbers();
			SettingsChaCha20 settingsChaCha20_4 = SettingsChaCha20.CreateWithCryptographicRandomNumbers();

			// Act

			// Assert
			CollectionAssert.AreNotEqual(settingsChaCha20_1.nonce, settingsChaCha20_2.nonce);
			CollectionAssert.AreNotEqual(settingsChaCha20_1.nonce, settingsChaCha20_3.nonce);
			CollectionAssert.AreNotEqual(settingsChaCha20_1.nonce, settingsChaCha20_4.nonce);
			CollectionAssert.AreNotEqual(settingsChaCha20_2.nonce, settingsChaCha20_3.nonce);
			CollectionAssert.AreNotEqual(settingsChaCha20_2.nonce, settingsChaCha20_4.nonce);
			CollectionAssert.AreNotEqual(settingsChaCha20_3.nonce, settingsChaCha20_4.nonce);

			Assert.AreNotEqual(0, settingsChaCha20_1.counter + settingsChaCha20_2.counter + settingsChaCha20_3.counter + settingsChaCha20_4.counter);
		}

		[Test]
		public void CreateSettingsAES_CTRWithCryptographicRandomNumbersTest()
		{
			// Arrange
			SettingsAES_CTR settingsAES_CTR_1 = SettingsAES_CTR.CreateWithCryptographicRandomNumbers();
			SettingsAES_CTR settingsAES_CTR_2 = SettingsAES_CTR.CreateWithCryptographicRandomNumbers();
			SettingsAES_CTR settingsAES_CTR_3 = SettingsAES_CTR.CreateWithCryptographicRandomNumbers();
			SettingsAES_CTR settingsAES_CTR_4 = SettingsAES_CTR.CreateWithCryptographicRandomNumbers();

			// Act

			// Assert
			CollectionAssert.AreNotEqual(settingsAES_CTR_1.initialCounter, settingsAES_CTR_2.initialCounter);
			CollectionAssert.AreNotEqual(settingsAES_CTR_1.initialCounter, settingsAES_CTR_3.initialCounter);
			CollectionAssert.AreNotEqual(settingsAES_CTR_1.initialCounter, settingsAES_CTR_4.initialCounter);
			CollectionAssert.AreNotEqual(settingsAES_CTR_2.initialCounter, settingsAES_CTR_3.initialCounter);
			CollectionAssert.AreNotEqual(settingsAES_CTR_2.initialCounter, settingsAES_CTR_4.initialCounter);
			CollectionAssert.AreNotEqual(settingsAES_CTR_3.initialCounter, settingsAES_CTR_4.initialCounter);
		}
	}
}