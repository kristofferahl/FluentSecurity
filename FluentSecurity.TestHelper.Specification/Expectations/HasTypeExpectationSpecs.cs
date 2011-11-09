using System;
using System.Linq.Expressions;
using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification.Expectations
{
	[TestFixture]
	[Category("HasTypeExpectationSpecs")]
	public class When_creating_a_HasTypeExpectation
	{
		[Test]
		public void Should_have_type_and_default_predicate()
		{
			var expectation = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
			Assert.That(expectation.IsPredicateExpectation, Is.False);
			Assert.That(expectation.PredicateExpression, Is.Not.Null);
			Assert.That(expectation.Predicate, Is.Not.Null);
			Assert.That(expectation.GetPredicateDescription(), Is.EqualTo(
				"securityPolicy => (securityPolicy.GetType() == value(FluentSecurity.TestHelper.Expectations.HasTypeExpectation`1[FluentSecurity.TestHelper.Specification.TestData.DenyInternetExplorerPolicy]).Type)"
				));
		}

		[Test]
		public void Should_have_type_and_predicate()
		{
			Expression<Func<DenyInternetExplorerPolicy, bool>> predicate = p => true;
			var expectation = new HasTypeExpectation<DenyInternetExplorerPolicy>(predicate);
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
			Assert.That(expectation.IsPredicateExpectation, Is.True);
			Assert.That(expectation.PredicateExpression, Is.EqualTo(predicate));
			Assert.That(expectation.Predicate, Is.Not.Null);
			Assert.That(expectation.GetPredicateDescription(), Is.EqualTo(
				"p => True"
				));
		}
	}
}