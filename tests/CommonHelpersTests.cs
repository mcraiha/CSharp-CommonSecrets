using NUnit.Framework;
using Assert = NUnit.Framework.Legacy.ClassicAssert;
using CSCommonSecrets;
using System.Collections;
using System;

#if ASYNC_WITH_CUSTOM
using System.Threading.Tasks;
#endif // ASYNC_WITH_CUSTOM

namespace Tests
{
	public class CommonHelpersTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void CheckAUDALFbytesTest()
		{
			// Arrange
			byte[] nullInput = null;
			byte[] zeroLengthInput = new byte[0];

			// Act
			(bool valid1, Exception exception1) = Helpers.CheckAUDALFbytes(nullInput);
			(bool valid2, Exception exception2) = Helpers.CheckAUDALFbytes(zeroLengthInput);

			// Assert
			Assert.IsFalse(valid1);
			Assert.IsFalse(valid2);
			Assert.AreEqual(typeof(ArgumentNullException), exception1.GetType());
			Assert.AreEqual(typeof(ArgumentException), exception2.GetType());
		}
	}
}