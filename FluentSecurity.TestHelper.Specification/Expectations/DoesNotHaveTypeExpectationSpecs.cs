using System;
using System.Linq.Expressions;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification.Expectations
{
	[TestFixture]
	[Category("DoesNotHaveTypeExpectationSpecs")]
	public class When_creating_a_DoesNotHaveTypeExpectation
	{
		[Test]
		public void Should_have_type_and_default_predicate()
		{
			var expectation = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
			Assert.That(expectation.IsPredicateExpectation, Is.False);
			Assert.That(expectation.PredicateExpression, Is.Not.Null);
			Assert.That(expectation.Predicate, Is.Not.Null);
			Assert.That(expectation.GetPredicateDescription(), Is.EqualTo(
				"securityPolicy => (securityPolicy.GetPolicyType() == value(FluentSecurity.TestHelper.Expectations.DoesNotHaveTypeExpectation`1[FluentSecurity.TestHelper.Specification.TestData.DenyInternetExplorerPolicy]).Type)"
				));
		}

		[Test]
		public void Should_have_type_and_predicate()
		{
			Expression<Func<DenyInternetExplorerPolicy, bool>> predicate = p => true;
			var expectation = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>(predicate);
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
			Assert.That(expectation.IsPredicateExpectation, Is.True);
			Assert.That(expectation.PredicateExpression, Is.EqualTo(predicate));
			Assert.That(expectation.Predicate, Is.Not.Null);
			Assert.That(expectation.GetPredicateDescription(), Is.EqualTo(
				"p => True"
				));
		}
	}

	[TestFixture]
	[Category("DoesNotHaveTypeExpectationSpecs")]
	public class When_evaluating_a_DoesNotHaveTypeExpectation_on_a_normal_policy
	{
		[Test]
		public void Should_be_match_for_normal_policy()
		{
			// Arrange
			var expectation = new DoesNotHaveTypeExpectation<IgnorePolicy>();
			var policy = new IgnorePolicy();

			// Act
			var isMatch = expectation.IsMatch(policy);

			// Assert
			Assert.That(isMatch, Is.True);
		}

		[Test]
		public void Should_not_be_match_for_normal_policy()
		{
			// Arrange
			var expectation = new DoesNotHaveTypeExpectation<DenyAnonymousAccessPolicy>();
			var policy = new IgnorePolicy();

			// Act
			var isMatch = expectation.IsMatch(policy);

			// Assert
			Assert.That(isMatch, Is.False);
		}
	}

	[TestFixture]
	[Category("DoesNotHaveTypeExpectationSpecs")]
	public class When_evaluating_a_DoesNotHaveTypeExpectation_on_a_lazy_policy
	{
		[SetUp]
		public void SetUp()
		{
			SecurityConfigurator.Configure<MvcConfiguration>(configuration => {});
		}

		[Test]
		public void Should_be_match_for_lazy_policy()
		{
			// Arrange
			var expectation = new DoesNotHaveTypeExpectation<IgnorePolicy>();
			var policy = new LazySecurityPolicy<IgnorePolicy>();

			// Act
			var isMatch = expectation.IsMatch(policy);

			// Assert
			Assert.That(isMatch, Is.True);
		}

		[Test]
		public void Should_not_be_match_for_lazy_policy()
		{
			// Arrange
			var expectation = new DoesNotHaveTypeExpectation<DenyAnonymousAccessPolicy>();
			var policy = new LazySecurityPolicy<IgnorePolicy>();

			// Act
			var isMatch = expectation.IsMatch(policy);

			// Assert
			Assert.That(isMatch, Is.False);
		}
	}
}