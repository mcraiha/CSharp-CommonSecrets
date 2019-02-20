using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class CommonSecretPlainJsonTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorTest()
		{
			// Arrange
			CommonSecretsContainer csc = new CommonSecretsContainer();

			// Act
			string json = JsonConvert.SerializeObject(csc, Formatting.Indented);

			// Assert
			Assert.IsNotNull(json);
			Assert.IsTrue(json.Contains("version"));
		}
	}
}