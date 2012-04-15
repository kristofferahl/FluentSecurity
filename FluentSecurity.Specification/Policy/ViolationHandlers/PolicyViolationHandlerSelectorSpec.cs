using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_creating_a_PolicyViolationHandlerSelector
	{
		[Test]
		public void Should_throw_when_violation_handlers_is_null()
		{
			Assert.Throws<ArgumentNullException>(() => new PolicyViolationHandlerSelector(null));
		}
	}

	[TestFixture]
	[Category("PolicyViolationHandlerSelectorSpec")]
	public class When_selecting_a_policy_violation_handler
	{
		[Test]
		public void Should_return_handler_for_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			var policyResult = PolicyResult.CreateFailureResult(policy, "Anonymous access denied");
			var exception = new PolicyViolationException(policyResult);
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
			var selector = new PolicyViolationHandlerSelector(violationHandlers);

			// Act
			var handler = selector.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DenyAnonymousAccessPolicyViolationHandler)));
		}

		[Test]
		public void Should_return_handler_for_DenyAuthenticatedAccessPolicy()
		{
			// Arrange
			var policy = new DenyAuthenticatedAccessPolicy();
			var policyResult = PolicyResult.CreateFailureResult(policy, "Authenticated access denied");
			var exception = new PolicyViolationException(policyResult);
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
			var selector = new PolicyViolationHandlerSelector(violationHandlers);

			// Act
			var handler = selector.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.TypeOf(typeof(DenyAuthenticatedAccessPolicyViolationHandler)));
		}

		[Test]
		public void Should_not_return_handler_for_RequireRolePolicy()
		{
			// Arrange
			var policy = new RequireRolePolicy("Role");
			var policyResult = PolicyResult.CreateFailureResult(policy, "Access denied");
			var exception = new PolicyViolationException(policyResult);
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
			var selector = new PolicyViolationHandlerSelector(violationHandlers);

			// Act
			var handler = selector.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_return_DefaultPolicyViolationhandler_for_RequireRolePolicy()
		{
			// Arrange
			var policyResult1 = PolicyResult.CreateFailureResult(new DenyAnonymousAccessPolicy(), "Access denied");
			var exception1 = new PolicyViolationException(policyResult1);

			var policyResult2 = PolicyResult.CreateFailureResult(new RequireRolePolicy("Role"), "Access denied");
			var exception2 = new PolicyViolationException(policyResult2);

			var expectedHandler1 = new DenyAnonymousAccessPolicyViolationHandler(new EmptyResult());
			var expectedHandler2 = new DefaultPolicyViolationHandler();
			var violationHandlers = new List<IPolicyViolationHandler>
			{
				expectedHandler1,
				expectedHandler2
			};
			var selector = new PolicyViolationHandlerSelector(violationHandlers);

			// Act
			var handler1 = selector.FindHandlerFor(exception1);
			var handler2 = selector.FindHandlerFor(exception2);

			// Assert
			Assert.That(handler1, Is.EqualTo(expectedHandler1));
			Assert.That(handler2, Is.EqualTo(expectedHandler2));
		}

		public class DefaultPolicyViolationHandler : IPolicyViolationHandler
		{
			public ActionResult Handle(PolicyViolationException exception)
			{
				throw new NotImplementedException();
			}
		}
	}
}