#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class KeyDerivationFunctionEntrySyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			KeyDerivationFunctionEntry kdfe1 = new KeyDerivationFunctionEntry();

			KeyDerivationFunctionEntry kdfe2 = new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, "master_key" );

			// Act

			// Assert
			Assert.IsNotNull(kdfe1);
			Assert.IsNotNull(kdfe2);
		}

		[Test]
		public void ConstructorExceptionsTest()
		{
			// Arrange
			byte[] invalidSalt1 = null;
			byte[] invalidSalt2 = new byte[0];
			byte[] invalidSalt3 = new byte[15];

			int invalidIterationsCount1 = -100;
			int invalidIterationsCount2 =  0;
			int invalidIterationsCount3 = 100;

			string invalidId1 = null;
			string invalidId2 = "";

			// Act

			// Assert
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA1, new byte[16], 100_000, 32, "master_key" ));

			Assert.Throws<ArgumentNullException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, invalidSalt1, 100_000, 32, "master_key" ));
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, invalidSalt2, 100_000, 32, "master_key" ));
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, invalidSalt3, 100_000, 32, "master_key" ));

			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount1, 32, "master_key" ));
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount2, 32, "master_key" ));
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount3, 32, "master_key" ));

			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, invalidId1 ));
			Assert.Throws<ArgumentException>(() => new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, invalidId2 ));
		}

		[Test]
		public void GeneratePasswordBytesTest()
		{
			// Arrange
			byte[] salt1 = Encoding.UTF8.GetBytes("saltSALTsaltSALTsaltSALTsaltSALTsalt");
			KeyDerivationFunctionEntry kdfe1 = new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA256, salt1, 4096, 25, "master_key" );
			byte[] bytesShouldBe1 = new byte[25] { 0x34, 0x8c, 0x89, 0xdb, 0xcb, 0xd3, 0x2b, 0x2f, 0x32, 0xd8, 0x14, 0xb8, 0x11, 0x6e, 0x84, 0xcf, 0x2b, 0x17, 0x34, 0x7e, 0xbc, 0x18, 0x00, 0x18, 0x1c };

			byte[] salt2 = Encoding.UTF8.GetBytes("saltKEYbcTcXHCBxtjD");
			KeyDerivationFunctionEntry kdfe2 = new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA512, salt2, 100000, 64, "master_key" );
			byte[] bytesShouldBe2 = new byte[64] { 0xAC,0xCD,0xCD,0x87,0x98,0xAE,0x5C,0xD8,0x58,0x04,0x73,0x90,0x15,0xEF,0x2A,0x11,0xE3,0x25,0x91,0xB7,0xB7,0xD1,0x6F,0x76,0x81,0x9B,0x30,0xB0,0xD4,0x9D,0x80,0xE1,0xAB,0xEA,0x6C,0x98,0x22,0xB8,0x0A,0x1F,0xDF,0xE4,0x21,0xE2,0x6F,0x56,0x03,0xEC,0xA8,0xA4,0x7A,0x64,0xC9,0xA0,0x04,0xFB,0x5A,0xF8,0x22,0x9F,0x76,0x2F,0xF4,0x1F };

			// Act
			byte[] bytes1 = kdfe1.GeneratePasswordBytes("passwordPASSWORDpassword");
			byte[] bytes2 = kdfe2.GeneratePasswordBytes("passDATAb00AB7YxDTT");

			// Assert
			CollectionAssert.AreEqual(bytesShouldBe1, bytes1);
			CollectionAssert.AreEqual(bytesShouldBe2, bytes2);
		}

		[Test]
		public void CreateHMACSHA256KeyDerivationFunctionEntryTest()
		{
			// Arrange
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA256KeyDerivationFunctionEntry("does not matter");
			string password = "tooeasy";

			// Act
			byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

			// Assert
			Assert.GreaterOrEqual(kdfe.iterations, KeyDerivationFunctionEntry.iterationsMin);
			Assert.AreEqual(32, derivedPassword.Length);
			Assert.Greater(CalculateEntropy.ShannonEntropy(derivedPassword), 4.0);
		}

		[Test]
		public void CreateHMACSHA512KeyDerivationFunctionEntryTest()
		{
			// Arrange
			KeyDerivationFunctionEntry kdfe = KeyDerivationFunctionEntry.CreateHMACSHA512KeyDerivationFunctionEntry("does not matter anymore");
			string password = "tooeasypart2";

			// Act
			byte[] derivedPassword = kdfe.GeneratePasswordBytes(password);

			// Assert
			Assert.GreaterOrEqual(kdfe.iterations, KeyDerivationFunctionEntry.iterationsMin);
			Assert.AreEqual(64, derivedPassword.Length);
			Assert.Greater(CalculateEntropy.ShannonEntropy(derivedPassword), 4.0);
		}

		[Test]
		public void ChecksumSurvivesRoundtripTest()
		{
			// Arrange
			byte[] salt = Encoding.UTF8.GetBytes("saltKEYbcTcXHCBxtjD");
			KeyDerivationFunctionEntry kdfe1 = new KeyDerivationFunctionEntry(KeyDerivationPrf.HMACSHA512, salt, 100000, 64, "master_key" );

			// Act
			string checksum1 = kdfe1.GetChecksumAsHex();

			string json = JsonConvert.SerializeObject(kdfe1, Formatting.Indented);

			KeyDerivationFunctionEntry kdfe2 = JsonConvert.DeserializeObject<KeyDerivationFunctionEntry>(json);

			// Assert
			Assert.AreEqual(64, checksum1.Length);
			Assert.AreEqual(checksum1, kdfe2.GetChecksumAsHex());
		}
	}
}

#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM