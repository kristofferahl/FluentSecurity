using System;
using System.Collections.Generic;
using FluentSecurity.Core.Policy.ViolationHandlers;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("PolicyViolationHandlerTypeConventionSpec")]
	public class When_getting_a_PolicyViolationHandler_using_PolicyViolationHandlerTypeConvention
	{
		[Test]
		public void Should_return_null_when_derrived_convention_returns_null_from_GetHandlerTypeFor()
		{
			// Arrange
			var convention = new DerrivedPolicyViolationHandlerTypeConvention(null);
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor<IPolicyViolationHandler>(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_return_null_when_derrived_convention_returns_type_not_assignable_to_ISecurityPolicyViolationHandler_from_GetHandlerTypeFor()
		{
			// Arrange
			var convention = new DerrivedPolicyViolationHandlerTypeConvention(typeof(Exception));
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());
			convention.Inject(TestDataFactory.CreateServiceLocator(
				t => new Exception(),
				t => new List<Exception> { new Exception() }
				));

			// Act
			var handler = convention.GetHandlerFor<IPolicyViolationHandler>(exception);
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_throw_when_derrived_convention_returns_type_not_assignable_to_IPolicyViolationHandler_from_GetHandlerTypeFor()
		{
			// Arrange
			var convention = new DerrivedPolicyViolationHandlerTypeConvention(typeof(Handler));
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());
			convention.Inject(TestDataFactory.CreateServiceLocator(
				t => new Handler(),
				t => new List<Handler> { new Handler() }
				));

			// Act & Assert
			var result = Assert.Throws<PolicyViolationHandlerConversionException>(() => convention.GetHandlerFor<IPolicyViolationHandler>(exception));
			Assert.That(result.Message, Is.EqualTo("The violation handler FluentSecurity.Specification.Policy.ViolationHandlers.Conventions.When_getting_a_PolicyViolationHandler_using_PolicyViolationHandlerTypeConvention+Handler does not implement the interface FluentSecurity.IPolicyViolationHandler!"));
		}

		public class Handler : ISecurityPolicyViolationHandler {}

		public class DerrivedPolicyViolationHandlerTypeConvention : PolicyViolationHandlerTypeConvention
		{
			public Type ReturnType { get; private set; }

			public DerrivedPolicyViolationHandlerTypeConvention(Type returnType)
			{
				ReturnType = returnType;
			}

			public override Type GetHandlerTypeFor(PolicyViolationException exception)
			{
				return ReturnType;
			}
		}
	}
}