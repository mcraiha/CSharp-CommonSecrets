using NUnit.Framework;
using CSCommonSecrets;

namespace Tests
{
	public class SymmetricKeyAlgorithmCommonTests
	{
		[SetUp]
		public void Setup()
		{
			
		}
		
		[Test]
		public void InvalidValuesTest()
		{
			// Arrange
			string invalidValue = "invalid";
			SymmetricKeyAlgorithm symmetricKeyAlgorithm = new SymmetricKeyAlgorithm();
			symmetricKeyAlgorithm.algorithm = invalidValue;

			// Act

			// Assert
			Assert.Throws<System.Exception>(() => symmetricKeyAlgorithm.GetSymmetricEncryptionAlgorithm());
		}
	}
}