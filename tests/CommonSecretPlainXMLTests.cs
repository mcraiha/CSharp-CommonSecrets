using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.IO;
using System.Text;
using System.Xml;  
using System.Xml.Serialization; 

namespace Tests
{
	public class CommonSecretPlainXMLTests
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
			string xml = null;

			var xmlserializer = new XmlSerializer(typeof(CommonSecretsContainer));
			var stringWriter = new StringWriter();
			using (var writer = XmlWriter.Create(stringWriter))
			{
				xmlserializer.Serialize(writer, csc);
				xml = stringWriter.ToString();
			}

			// Assert
			Assert.IsNotNull(xml);
			Assert.IsTrue(xml.Contains("version"));
		}
	}
}