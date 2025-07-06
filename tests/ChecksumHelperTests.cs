using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System.Collections;
using System.Linq;

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;
#endif // ASYNC_WITH_CUSTOM

namespace Tests
{
	public class ChecksumHelpersTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		#if !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

		private static readonly int sha256LengthInBytes = 32;

		[Test, Description("Calculate hex checksum test")]
		public void CalculateHexChecksumTest()
		{
			// Arrange
			byte[] byteArray1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray3 = new byte[] { };

			string expectedHex = "66840DDA154E8A113C31DD0AD32F7F3A366A80E8136979D8F5A101D3D29D6F72";

			// Act
			string hex1 = ChecksumHelper.CalculateHexChecksum(byteArray1);
			string hex2 = ChecksumHelper.CalculateHexChecksum(byteArray2);
			string hex3 = ChecksumHelper.CalculateHexChecksum(byteArray3);
			string hex4 = ChecksumHelper.CalculateHexChecksum(byteArray1, byteArray2);

			// Assert
			Assert.IsNotNull(hex1);
			Assert.IsNotNull(hex2);
			Assert.IsNotNull(hex3);
			Assert.IsNotNull(hex4);

			Assert.AreEqual(sha256LengthInBytes * 2, hex1.Length, "Every byte should convert to two Hex chars");

			Assert.AreEqual(expectedHex, hex1);
			Assert.AreEqual(hex1, hex2);
			Assert.AreNotEqual(hex2, hex3);
			Assert.AreNotEqual(hex2, hex4);
		}

		#endif // !ASYNC_WITH_CUSTOM && !WITH_CUSTOM

		#if ASYNC_WITH_CUSTOM

		private static readonly int sha256LengthInBytes = 32;

		[Test, Description("Calculate hex checksum async test")]
		public async Task CalculateHexChecksumAsyncTest()
		{
			// Arrange
			ISecurityAsyncFunctions securityAsyncFunctions = new SecurityAsyncFunctions();

			byte[] byteArray1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray3 = new byte[] { };

			string expectedHex = "66840DDA154E8A113C31DD0AD32F7F3A366A80E8136979D8F5A101D3D29D6F72";

			// Act
			string hex1 = await ChecksumHelper.CalculateHexChecksumAsync(securityAsyncFunctions, byteArray1);
			string hex2 = await ChecksumHelper.CalculateHexChecksumAsync(securityAsyncFunctions, byteArray2);
			string hex3 = await ChecksumHelper.CalculateHexChecksumAsync(securityAsyncFunctions, byteArray3);
			string hex4 = await ChecksumHelper.CalculateHexChecksumAsync(securityAsyncFunctions, byteArray1, byteArray2);

			// Assert
			Assert.IsNotNull(hex1);
			Assert.IsNotNull(hex2);
			Assert.IsNotNull(hex3);
			Assert.IsNotNull(hex4);

			Assert.AreEqual(sha256LengthInBytes * 2, hex1.Length, "Every byte should convert to two Hex chars");

			Assert.AreEqual(expectedHex, hex1);
			Assert.AreEqual(hex1, hex2);
			Assert.AreNotEqual(hex2, hex3);
			Assert.AreNotEqual(hex2, hex4);
		}

		#endif // ASYNC_WITH_CUSTOM

		[Test, Description("Join byte arrays test")]
		public void JoinByteArraysTest()
		{
			// Arrange
			byte[] byteArray1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray2 = new byte[] { 123, 255, 13, 3, 33 };
			byte[] byteArray3 = new byte[0];
			byte[] byteArray4 = new byte[] { 100, 100, 100, 100};

			// Act
			byte[] joined1 = ChecksumHelper.JoinByteArrays(byteArray1, byteArray2);

			// Assert
			Assert.IsNotNull(joined1);
			Assert.AreEqual(byteArray1.Length + byteArray2.Length, joined1.Length);
			Assert.IsTrue(CheckSequence(joined1, byteArray1));
			Assert.IsTrue(CheckSequence(joined1, byteArray2));
			Assert.IsFalse(CheckSequence(joined1, byteArray4));
		}

		private static bool CheckSequence(byte[] ba1, byte[] ba2)
		{
			for (int i = 0; i <= ba1.Count(); i++)
			{
				if (ba2.SequenceEqual(ba1.Skip(i).Take(ba2.Count())))
				{
					return true;
				}
			}

			return false;
		}
	}
}