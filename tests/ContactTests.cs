using NUnit.Framework;
using CSCommonSecrets;
using System;
using System.Text;
using Newtonsoft.Json;

namespace Tests
{
	public class ContactTests
	{
		[SetUp]
		public void Setup()
		{
			
		}

		[Test]
		public void ConstructorSimpleTest()
		{
			// Arrange
			Contact c1 = new Contact();
			Contact c2 = new Contact("first", "last", "middle");

			// Act

			// Assert
			Assert.IsNotNull(c1);
			Assert.IsNotNull(c2);
		}
	}
}