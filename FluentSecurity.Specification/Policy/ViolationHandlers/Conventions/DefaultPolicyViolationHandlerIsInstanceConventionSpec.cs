using System;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("DefaultPolicyViolationHandlerIsInstanceConventionSpec")]
	public class When_creating_a_DefaultPolicyViolationHandlerIsInstanceConvention
	{
		[Test]
		public void Should_throw_when_factory_method_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new DefaultPolicyViolationHandlerIsInstanceConvention<DefaultPolicyViolationHandler>(null));
		}
	}

	[TestFixture]
	[Category("DefaultPolicyViolationHandlerIsInstanceConventionSpec")]
	public class When_getting_a_PolicyViolationHandler_using_DefaultPolicyViolationHandlerIsInstanceConvention
	{
		[Test]
		public void Should_return_null_when_factory_method_returns_null()
		{
			// Arrange
			var convention = new DefaultPolicyViolationHandlerIsInstanceConvention<Handler1>(() => null);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_create_instance_of_Handler1_and_return_it()
		{
			// Arrange
			var convention = new DefaultPolicyViolationHandlerIsInstanceConvention<Handler1>(() => new Handler1());
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<Handler1>());
		}

		[Test]
		public void Should_create_instance_of_Handler2_and_return_it()
		{
			// Arrange
			var convention = new DefaultPolicyViolationHandlerIsInstanceConvention<Handler2>(() => new Handler2());
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<Handler2>());
		}

		public class Handler1 : DefaultPolicyViolationHandler {}

		public class Handler2 : DefaultPolicyViolationHandler {}
	}
}