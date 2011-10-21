using System;
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
			Assert.That(expectation.Predicate, Is.Not.Null);
		}

		[Test]
		public void Should_have_type_and_predicate()
		{
			Func<DenyInternetExplorerPolicy, bool> predicate = p => true;
			var expectation = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>(predicate);
			Assert.That(expectation.Type, Is.EqualTo(typeof(DenyInternetExplorerPolicy)));
			Assert.That(expectation.Predicate, Is.EqualTo(predicate));
		}
	}
}