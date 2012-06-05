using System.Configuration;
using FluentSecurity.Internals;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Internals
{
	[TestFixture]
	[Category("ExceptionFactorySpec")]
	public class When_using_the_ExceptionFactory
	{
		[TearDown]
		public void TearDown()
		{
			HttpContextRequestDescription.HttpContextProvider = HttpContextRequestDescription.DefaultHttpContextProvider;
		}

		[Test]
		public void Should_have_request_description_provider()
		{
			// Arrange
			SecurityConfigurator.SetConfiguration(TestDataFactory.CreateValidSecurityConfiguration());
			HttpContextRequestDescription.HttpContextProvider = MvcMockHelpers.FakeHttpContext;
			ExceptionFactory.Reset();

			// Act
			var requestDescription = ExceptionFactory.RequestDescriptionProvider();
			
			// Assert
			Assert.That(requestDescription, Is.Not.Null);
		}

		[Test]
		public void Should_create_ConfigurationErrorException()
		{
			// Arrange
			ExceptionFactory.RequestDescriptionProvider = () => TestDataFactory.CreateRequestDescription();

			// Act
			var result = ExceptionFactory.CreateConfigurationErrorsException("Message");

			// Assert
			Assert.That(result, Is.TypeOf(typeof(ConfigurationErrorsException)));
		}

		[Test]
		public void Should_create_request_description_string_with_arename()
		{
			// Arrange
			var requestDescription = TestDataFactory.CreateRequestDescription();

			// Act
			var result = ExceptionFactory.CreateRequestDescriptionString(requestDescription);

			// Assert
			Assert.That(result, Is.EqualTo("\r\nArea: Area\r\nController: Controller\r\nAction: Action"));
		}

		[Test]
		public void Should_create_request_description_string_with_no_arename()
		{
			// Arrange
			var requestDescription = TestDataFactory.CreateRequestDescription(null);

			// Act
			var result = ExceptionFactory.CreateRequestDescriptionString(requestDescription);

			// Assert
			Assert.That(result, Is.EqualTo("\r\nArea: (not set)\r\nController: Controller\r\nAction: Action"));
		}
	}
}