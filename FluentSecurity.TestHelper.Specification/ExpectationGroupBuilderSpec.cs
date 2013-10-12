using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationGroupBuilderSpec")]
	public class When_building_expecations
	{
		[Test]
		public void Should_throw_when_expressions_are_null()
		{
			var expectationBuilder = new ExpectationGroupBuilder();
			Assert.Throws<ArgumentNullException>(() => expectationBuilder.Build(null));
		}

		[Test]
		public void Should_return_0_expectation_groups_when_expressions_are_empty()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression>();
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_return_1_expectation_groups_for_1_expression()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression> { new ExpectationExpression<AdminController>(x => x.Login()) };
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_return_4_expectation_groups_for_1_expression()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression> { new ExpectationExpression<AdminController>() };
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(4));
		}

		[Test]
		public void Should_return_4_expectation_groups_for_2_expressions()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression>
			{
				new ExpectationExpression<AdminController>(),
				new ExpectationExpression<AdminController>(x => x.Login())
			};
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(4));
		}

		[Test]
		public void Should_return_5_expectation_groups_for_2_expressions()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression>
			{
				new ExpectationExpression<AdminController>(),
				new ExpectationExpression<SampleController>(x => x.Index())
			};
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(5));
		}

		[Test]
		public void Should_return_9_expectation_groups_for_2_expressions()
		{
			// Arrange
			var expecatationExpressions = new List<ExpectationExpression>
			{
				new ExpectationExpression<AdminController>(),
				new ExpectationExpression<SampleController>()
			};
			var expecationBuilder = new ExpectationGroupBuilder();

			// Act
			var expectationGroups = expecationBuilder.Build(expecatationExpressions);

			// Assert
			Assert.That(expectationGroups.Count(), Is.EqualTo(10));
		}
	}
}