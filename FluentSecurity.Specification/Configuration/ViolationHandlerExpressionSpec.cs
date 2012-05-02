using System;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("ViolationHandlerExpressionSpec")]
	public class When_creating_a_ViolationHandlerExpression
	{
		private readonly ViolationConfigurationExpression _validExpression = new ViolationConfigurationExpression(new Conventions());
		private readonly Func<PolicyResult, bool> _validPredicate = x => true;

		[Test]
		public void Should_throw_when_expression_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerExpression(null, _validPredicate));
		}

		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerExpression(_validExpression, null));
		}

		[Test]
		public void Should_not_throw_when_ViolationExpression_is_not_null()
		{
			Assert.DoesNotThrow(() => new ViolationHandlerExpression(_validExpression, _validPredicate));
		}
	}

	public class When_creating_a_ViolationHandlerExpression_of_T
	{
		private readonly ViolationConfigurationExpression _validExpression = new ViolationConfigurationExpression(new Conventions());

		[Test]
		public void Should_throw_when_expression_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationHandlerExpression<IgnorePolicy>(null));
		}

		[Test]
		public void Should_not_throw_when_expression_is_not_null()
		{
			Assert.DoesNotThrow(() => new ViolationHandlerExpression<IgnorePolicy>(_validExpression));
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
			var expression = new ViolationHandlerExpression(new ViolationConfigurationExpression(new Conventions()), x => true);

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expression.IsHandledBy<DefaultPolicyViolationHandler>(null));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_type()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = x => true;
			var conventions = new Conventions();
			var expression = new ViolationHandlerExpression(new ViolationConfigurationExpression(conventions), expectedPredicate);

			// Act
			expression.IsHandledBy<DefaultPolicyViolationHandler>();

			// Assert
			Assert.That(conventions.Single().As<PredicateToPolicyViolationHandlerTypeConvention<DefaultPolicyViolationHandler>>().Predicate, Is.EqualTo(expectedPredicate));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_instance()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = x => true;
			var conventions = new Conventions();
			var expression = new ViolationHandlerExpression(new ViolationConfigurationExpression(conventions), expectedPredicate);

			// Act
			expression.IsHandledBy(() => new DefaultPolicyViolationHandler());

			// Assert
			Assert.That(conventions.Single().As<PredicateToPolicyViolationHandlerInstanceConvention<DefaultPolicyViolationHandler>>().Predicate, Is.EqualTo(expectedPredicate));
		}
	}

	[TestFixture]
	[Category("ViolationHandlerExpressionSpec")]
	public class When_specifying_a_handler_factory_for_a_ViolationHandlerExpression_of_T
	{
		[Test]
		public void Should_throw_when_factory_method_is_null()
		{
			// Arrange
			var expression = new ViolationHandlerExpression<IgnorePolicy>(new ViolationConfigurationExpression(new Conventions()));

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expression.IsHandledBy<DefaultPolicyViolationHandler>(null));
		}

		[Test]
		public void Should_add_convention_for_predicate_to_type()
		{
			// Arrange
			var conventions = new Conventions();
			var expression = new ViolationHandlerExpression<IgnorePolicy>(new ViolationConfigurationExpression(conventions));

			// Act
			expression.IsHandledBy<DefaultPolicyViolationHandler>();

			// Assert
			Assert.That(conventions.Single(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerTypeConvention<IgnorePolicy, DefaultPolicyViolationHandler>>());
		}

		[Test]
		public void Should_add_convention_for_predicate_to_instance()
		{
			// Arrange
			var conventions = new Conventions();
			var expression = new ViolationHandlerExpression<IgnorePolicy>(new ViolationConfigurationExpression(conventions));

			// Act
			expression.IsHandledBy(() => new DefaultPolicyViolationHandler());

			// Assert
			Assert.That(conventions.Single(), Is.InstanceOf<PolicyTypeToPolicyViolationHandlerInstanceConvention<IgnorePolicy, DefaultPolicyViolationHandler>>());
		}
	}
}