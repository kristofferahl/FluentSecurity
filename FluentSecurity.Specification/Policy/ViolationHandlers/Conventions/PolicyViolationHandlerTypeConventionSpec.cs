using System;
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
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_return_null_when_derrived_convention_returns_type_not_assignable_to_IPolicyViolationHandler_from_GetHandlerTypeFor()
		{
			// Arrange
			var convention = new DerrivedPolicyViolationHandlerTypeConvention(typeof(Exception));
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

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