using System;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationExpressionSpec")]
	public class When_creating_an_expectation_expression_for_AdminController
	{
		private ExpectationExpression<AdminController> _expectationExpression;

		[SetUp]
		public void SetUp()
		{
			_expectationExpression = new ExpectationExpression<AdminController>();
		}

		[Test]
		public void Should_have_type_set_to_AdminController()
		{
			Assert.That(_expectationExpression.Controller, Is.EqualTo(typeof(AdminController)));
		}

		[Test]
		public void Should_have_action_set_to_null()
		{
			Assert.That(_expectationExpression.Action, Is.Null);
		}

		[Test]
		public void Should_have_0_expectations()
		{
			Assert.That(_expectationExpression.Expectations.Count(), Is.EqualTo(0));
		}
	}

	[TestFixture]
	[Category("ExpectationExpressionSpec")]
	public class When_creating_an_expectation_expression_for_AdminController_Login
	{
		private ExpectationExpression<AdminController> _expectationExpression;

		[SetUp]
		public void SetUp()
		{
			_expectationExpression = new ExpectationExpression<AdminController>(x => x.Login());
		}

		[Test]
		public void Should_have_type_set_to_AdminController()
		{
			Assert.That(_expectationExpression.Controller, Is.EqualTo(typeof(AdminController)));
		}

		[Test]
		public void Should_have_action_set_to_null()
		{
			Assert.That(_expectationExpression.Action, Is.EqualTo("Login"));
		}

		[Test]
		public void Should_have_0_expectations()
		{
			Assert.That(_expectationExpression.Expectations.Count(), Is.EqualTo(0));
		}
	}

	[TestFixture]
	[Category("ExpectationExpressionSpec")]
	public class When_adding_expectations_to_an_expectation_expression_for_AdminController_Login
	{
		private ExpectationExpression<AdminController> _expectationExpression;

		[SetUp]
		public void SetUp()
		{
			_expectationExpression = new ExpectationExpression<AdminController>(x => x.Login());
			
		}

		[Test]
		public void Should_throw_when_expectation_is_null()
		{
			// Arrange
			IExpectation expectation = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => _expectationExpression.Add(expectation));
		}

		[Test]
		public void Should_have_1_expectations()
		{
			// Arrange
			var expectation = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			
			// Act
			_expectationExpression.Add(expectation);

			// Assert
			Assert.That(_expectationExpression.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_2_expectations()
		{
			// Arrange
			var expectation1 = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();

			// Act
			_expectationExpression.Add(expectation1).Add(expectation2);

			// Assert
			Assert.That(_expectationExpression.Expectations.Count(), Is.EqualTo(2));
		}
	}
	
	[TestFixture]
	[Category("ExpectationExpressionSpec")]
	public class When_creating_an_expectation_expression_for_SampleController_AliasedAction
	{
		private ExpectationExpression<SampleController> _expectationExpression;

		[SetUp]
		public void SetUp()
		{
			_expectationExpression = new ExpectationExpression<SampleController>(x => x.ActualAction());
		}

		[Test]
		public void Should_resolve_actual_action_to_aliased_action()
		{
			Assert.That(_expectationExpression.Action, Is.EqualTo("AliasedAction"));
		}
	}
}