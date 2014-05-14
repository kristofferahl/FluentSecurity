using System;
using FluentSecurity.Core.Policy.ViolationHandlers;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.ServiceLocation;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
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
			var serviceLocator = new Mock<IServiceLocator>();
			serviceLocator.Setup(x => x.Resolve(It.IsAny<Type>())).Returns(expectedHandler);
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler1>();
			convention.Inject(serviceLocator.Object);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor<IPolicyViolationHandler>(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		[Test]
		public void Should_resolve_instance_of_Handler1_and_return_it()
		{
			// Arrange
			var expectedHandler = new Handler1();
			var serviceLocator = new Mock<IServiceLocator>();
			serviceLocator.Setup(x => x.Resolve(It.IsAny<Type>())).Returns(expectedHandler);
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler1>();
			convention.Inject(serviceLocator.Object);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor<IPolicyViolationHandler>(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		[Test]
		public void Should_resolve_instance_of_Handler2_and_return_it()
		{
			// Arrange
			var expectedHandler = new Handler2();
			var serviceLocator = new Mock<IServiceLocator>();
			serviceLocator.Setup(x => x.Resolve(It.IsAny<Type>())).Returns(expectedHandler);
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler2>();
			convention.Inject(serviceLocator.Object);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor<IPolicyViolationHandler>(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
		}

		[Test]
		public void Should_resolve_instance_of_Handler3_and_throw()
		{
			// Arrange
			var expectedHandler = new Handler3();
			var serviceLocator = new Mock<IServiceLocator>();
			serviceLocator.Setup(x => x.Resolve(It.IsAny<Type>())).Returns(expectedHandler);
			var convention = new DefaultPolicyViolationHandlerIsOfTypeConvention<Handler3>();
			convention.Inject(serviceLocator.Object);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act & Assert
			var result = Assert.Throws<PolicyViolationHandlerConversionException>(() => convention.GetHandlerFor<IPolicyViolationHandler>(exception));
			Assert.That(result.Message, Is.EqualTo("The violation handler FluentSecurity.Specification.Policy.ViolationHandlers.Conventions.When_getting_a_PolicyViolationHandler_using_DefaultPolicyViolationHandlerIsOfTypeConvention+Handler3 does not implement the interface FluentSecurity.IPolicyViolationHandler!"));
		}

		public class Handler1 : DefaultPolicyViolationHandler {}

		public class Handler2 : DefaultPolicyViolationHandler {}

		public class Handler3 : ISecurityPolicyViolationHandler {}
	}
}