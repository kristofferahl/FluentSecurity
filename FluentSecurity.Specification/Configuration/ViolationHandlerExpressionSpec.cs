using System;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("ViolationHandlerExpressionSpec")]
	public class When_creating_a_ViolationHandlerExpression
	{
		[Test]
		public void Should_throw_when_ViolationExpression_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerExpression<IgnorePolicy>(null));
		}
	}

	[TestFixture]
	[Category("ViolationHandlerExpressionSpec")]
	public class When_specifying_a_handler_factory_for_a_ViolationHandlerExpression
	{
		[Test]
		public void Should_throw_when_factory_method_is_null()
		{
			// Arrange
			var expression = new ViolationHandlerExpression<IgnorePolicy>(new ViolationsExpression(new Conventions()));

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expression.IsHandledBy<DefaultPolicyViolationHandler>(null));
		}
	}
}