using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Results;
using FluentSecurity.Policy.ViolationHandlers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	[TestFixture]
	[Category("DelegatePolicyViolationHandlerSpec")]
	public class When_handling_a_policy_violation_with_DelegatePolicyViolationHandler
	{
		[Test]
		public void Should_throw_when_violation_handler_has_not_been_set_for_DelegatePolicy()
		{
			// Arrange
			var failureResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Access denied");
			var policy = new DelegatePolicy("Test", c => failureResult);
			var delegatePolicyResult = new DelegatePolicyResult(failureResult, policy.ViolationHandler);
			var handler = new DelegatePolicyViolationHandler();
			var exception = new PolicyViolationException(delegatePolicyResult);

			// Act & assert
			var caughtException = Assert.Throws<PolicyViolationException>(() => handler.Handle(exception));
			Assert.That(caughtException, Is.EqualTo(exception));
		}

		[Test]
		public void Should_return_action_result_from_DelegatePolicy_ViolationHandler()
		{
			// Arrange
			var expectedResult = new ContentResult { Content = "Some content" };
			var failureResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Access denied");
			var policy = new DelegatePolicy("Test", c => failureResult, e => expectedResult);
			var delegatePolicyResult = new DelegatePolicyResult(failureResult, policy.ViolationHandler);
			var handler = new DelegatePolicyViolationHandler();
			var exception = new PolicyViolationException(delegatePolicyResult);

			// Act
			var result = handler.Handle(exception);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));
		}
	}
}