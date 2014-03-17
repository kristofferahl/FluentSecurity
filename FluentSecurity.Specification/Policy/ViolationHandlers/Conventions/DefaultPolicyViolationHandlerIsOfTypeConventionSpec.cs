using System.Collections.Generic;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("DefaultPolicyViolationHandlerIsOfTypeConventionSpec")]
	public class When_getting_a_PolicyViolationHandler_using_DefaultPolicyViolationHandlerIsOfTypeConvention
	{
		[Test]
		public void Should_resolve_instances_from_container()
		{
			// Arrange
			var expectedHandler = new Handler1();
			FakeIoC.GetAllInstancesProvider = () => new List<object> { expectedHandler };
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => configuration.ResolveServicesUsing(FakeIoC.GetAllInstances));
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler1>();
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		[Test]
		public void Should_resolve_instance_of_Handler1_and_return_it()
		{
			// Arrange
			var expectedHandler = new Handler1();
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler1>();
			convention.PolicyViolationHandlerProvider = t => expectedHandler;
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		[Test]
		public void Should_resolve_instance_of_Handler2_and_return_it()
		{
			// Arrange
			var expectedHandler = new Handler2();
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler2>();
			convention.PolicyViolationHandlerProvider = t => expectedHandler;
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		public class Handler1 : DefaultPolicyViolationHandler {}

		public class Handler2 : DefaultPolicyViolationHandler {}
	}
}