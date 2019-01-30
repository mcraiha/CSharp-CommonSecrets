using NUnit.Framework;
using CSCommonSecrets; 

namespace Tests
{
	public class ChecksumHelpersTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		private static readonly int sha256LengthInBytes = 32;

		[Test]
		public void CalculateHexChecksumTest()
		{
			// Arrange
			byte[] byteArray1 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray2 = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
			byte[] byteArray3 = new byte[] { };

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

			Assert.AreEqual(hex1, hex2);
			Assert.AreNotEqual(hex2, hex3);
			Assert.AreNotEqual(hex2, hex4);
		}
	}
}