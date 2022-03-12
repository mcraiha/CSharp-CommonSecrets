#if ASYNC_WITH_CUSTOM

using NUnit.Framework;
using CSCommonSecrets;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Text;
using Newtonsoft.Json;

using System.Threading.Tasks;

namespace Tests
{
	public class KeyDerivationFunctionEntryAsyncTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public async Task ConstructorAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			KeyDerivationFunctionEntry kdfe1 = new KeyDerivationFunctionEntry();

			KeyDerivationFunctionEntry kdfe2 = await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, "master_key", securityAsyncFunctions);

			// Act

			// Assert
			Assert.IsNotNull(kdfe1);
			Assert.IsNotNull(kdfe2);
		}

		[Test]
		public async Task ConstructorExceptionsASyncTest()
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

			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			// Act

			// Assert
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA1, new byte[16], 100_000, 32, "master_key", securityAsyncFunctions ));

			Assert.ThrowsAsync<ArgumentNullException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, invalidSalt1, 100_000, 32, "master_key", securityAsyncFunctions ));
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, invalidSalt2, 100_000, 32, "master_key", securityAsyncFunctions ));
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, invalidSalt3, 100_000, 32, "master_key", securityAsyncFunctions ));

			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount1, 32, "master_key", securityAsyncFunctions ));
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount2, 32, "master_key", securityAsyncFunctions ));
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], invalidIterationsCount3, 32, "master_key", securityAsyncFunctions ));

			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, invalidId1, securityAsyncFunctions ));
			Assert.ThrowsAsync<ArgumentException>(async () => await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA256, new byte[16], 100_000, 32, invalidId2, securityAsyncFunctions ));
		}

		[Test]
		public async Task ChecksumSurvivesRoundtrip()
		{
			// Arrange
			byte[] salt = Encoding.UTF8.GetBytes("saltKEYbcTcXHCBxtjD");
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();
			KeyDerivationFunctionEntry kdfe1 = await KeyDerivationFunctionEntry.CreateKeyDerivationFunctionEntryAsync(KeyDerivationPrf.HMACSHA512, salt, 100000, 64, "master_key", securityAsyncFunctions );

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

#endif // ASYNC_WITH_CUSTOM