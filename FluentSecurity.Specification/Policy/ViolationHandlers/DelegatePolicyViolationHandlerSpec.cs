using System.Web.Mvc;
using FluentSecurity.Policy;
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
			var policy = new DelegatePolicy("Test",
					c => PolicyResult.CreateFailureResult(c.Policy, "Access denied")
				);
			var handler = new DelegatePolicyViolationHandler();
			var exception = new PolicyViolationException(policy, "Access denied");

			// Act & assert
			var caughtException = Assert.Throws<PolicyViolationException>(() => handler.Handle(exception));
			Assert.That(caughtException, Is.EqualTo(exception));
		}

		[Test]
		public void Should_return_action_result_from_DelegatePolicy_ViolationHandler()
		{
			// Arrange
			var expectedResult = new ContentResult { Content = "Some content" };
			var policy = new DelegatePolicy("Test",
					c => PolicyResult.CreateFailureResult(c.Policy, "Access denied"),
					e => expectedResult
				);
			var handler = new DelegatePolicyViolationHandler();
			var exception = new PolicyViolationException(policy, "Access denied");

			// Act
			var result = handler.Handle(exception);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));
		}
	}
}