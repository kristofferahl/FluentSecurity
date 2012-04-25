using System;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Configuration
{
	[TestFixture]
	[Category("ViolationConfigurationExpressionSpec")]
	public class When_creating_a_ViolationConfigurationExpression
	{
		[Test]
		public void Should_throw_when_conventions_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new ViolationConfigurationExpression(null));
		}
	}

	[TestFixture]
	[Category("ViolationConfigurationExpressionSpec")]
	public class When_adding_a_convention
	{
		[Test]
		public void Should_throw_when_convention_is_null()
		{
			// Arrange
			var conventions = new Conventions();
			var expression = new ViolationConfigurationExpression(conventions);

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expression.AddConvention(null));
		}

		[Test]
		public void Should_add_convention()
		{
			// Arrange
			var expectedConvention = new MockConvention();
			var conventions = new Conventions();
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			expression.AddConvention(expectedConvention);

			// Assert
			Assert.That(conventions.First(), Is.EqualTo(expectedConvention));
		}
	}

	[TestFixture]
	[Category("ViolationConfigurationExpressionSpec")]
	public class When_removing_a_convention
	{
		[Test]
		public void Should_throw_when_predicate_is_null()
		{
			// Arrange
			var expression = new ViolationConfigurationExpression(new Conventions());

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => expression.RemoveConventions(null));
		}
		
		[Test]
		public void Should_remove_conventions_matching_predicate()
		{
			// Arrange
			var conventions = new Conventions { new MockConvention() };
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			expression.RemoveConventions(c => c is MockConvention);

			// Assert
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_remove_conventions_matching_type()
		{
			// Arrange
			var conventions = new Conventions { new MockConvention() };
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			expression.RemoveConventions<MockConvention>();

			// Assert
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_not_remove_conventions_not_matching_predicate()
		{
			// Arrange
			var conventions = new Conventions { new MockConvention() };
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			expression.RemoveConventions(c => c is NonMatchingConvention);

			// Assert
			Assert.That(conventions.Any(), Is.True);
		}

		[Test]
		public void Should_not_remove_conventions_not_matching_type()
		{
			// Arrange
			var conventions = new Conventions { new MockConvention() };
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			expression.RemoveConventions<NonMatchingConvention>();

			// Assert
			Assert.That(conventions.Any(), Is.True);
		}

		public class NonMatchingConvention : MockConvention {}
	}

	[TestFixture]
	[Category("ViolationConfigurationExpressionSpec")]
	public class When_calling_Of_on_ViolationConfigurationExpression
	{
		[Test]
		public void Should_not_add_any_convention_and_return_ViolationHandlerExpression_of_T()
		{
			// Arrange
			var conventions = new Conventions();
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			var result = expression.Of<IgnorePolicy>();

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(conventions.Any(), Is.False);
		}

		[Test]
		public void Should_not_add_any_convention_and_return_ViolationHandlerExpression_with_predicate()
		{
			// Arrange
			Func<PolicyResult, bool> expectedPredicate = pr => pr.PolicyType == typeof(IgnorePolicy);
			var conventions = new Conventions();
			var expression = new ViolationConfigurationExpression(conventions);

			// Act
			var result = expression.Of(expectedPredicate);

			// Assert
			Assert.That(result, Is.Not.Null);
			Assert.That(result.Predicate, Is.EqualTo(expectedPredicate));
			Assert.That(conventions.Any(), Is.False);
		}
	}
}