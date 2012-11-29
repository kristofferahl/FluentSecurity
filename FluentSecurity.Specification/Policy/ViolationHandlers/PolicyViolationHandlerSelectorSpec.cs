using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_creating_a_PolicyViolationHandlerSelector
	{
		[Test]
		public void Should_throw_when_conventions_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new PolicyViolationHandlerSelector(null));
		}
	}

	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_selecting_a_policy_violation_handler
	{
		[Test]
		public void Should_return_first_handler_returned_by_convention()
		{
			var expectedHandler = new ExceptionPolicyViolationHandler();
			var convention1 = new MockConvention(null);
			var convention2 = new MockConvention(expectedHandler);
			var convention3 = new MockConvention(null);
			var  conventions = new List<IPolicyViolationHandlerConvention>
			{
				convention1,
				convention2,
				convention3
			};

			var policy = new IgnorePolicy();
			var policyResult = PolicyResult.CreateFailureResult(policy, "Access denied");
			var exception = TestDataFactory.CreatePolicyViolationException(policyResult);
			var selector = new PolicyViolationHandlerSelector(conventions);

			// Act
			var handler = selector.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.EqualTo(expectedHandler));
			Assert.That(convention1.WasCalled, Is.True);
			Assert.That(convention2.WasCalled, Is.True);
			Assert.That(convention3.WasCalled, Is.False);
		}

		[Test]
		public void Should_return_null_when_no_convention_returns_handler()
		{
			var convention1 = new MockConvention(null);
			var convention2 = new MockConvention(null);
			var conventions = new List<IPolicyViolationHandlerConvention>
			{
				convention1,
				convention2
			};

			var policy = new IgnorePolicy();
			var policyResult = PolicyResult.CreateFailureResult(policy, "Access denied");
			var exception = TestDataFactory.CreatePolicyViolationException(policyResult);
			var selector = new PolicyViolationHandlerSelector(conventions);

			// Act
			var handler = selector.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
			Assert.That(convention1.WasCalled, Is.True);
			Assert.That(convention2.WasCalled, Is.True);
		}
	}
}