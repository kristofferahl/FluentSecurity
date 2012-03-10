using System;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
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
		private IPolicyViolationHandlerSelector _violationHandler;

		[SetUp]
		public void SetUp()
		{
			var violationHandlers = Helpers.TestDataFactory.CreatePolicyViolationHandlers();
			_violationHandler = new PolicyViolationHandlerSelector(violationHandlers);
		}

		[Test]
		public void Should_return_handler_for_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			var policyResult = PolicyResult.CreateFailureResult(policy, "Anonymous access denied");
			var exception = new PolicyViolationException(policyResult);

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

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

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

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

			// Act
			var handler = _violationHandler.FindHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}
	}
}