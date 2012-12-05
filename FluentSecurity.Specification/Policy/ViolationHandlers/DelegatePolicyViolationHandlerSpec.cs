using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Results;
using FluentSecurity.Policy.ViolationHandlers;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	[TestFixture]
	[Category("DelegatePolicyViolationHandlerSpec")]
	public class When_handling_a_policy_violation_with_DelegatePolicyViolationHandler
	{
		[Test]
		public void Should_throw_when_no_violation_handler_has_been_set_and_no_violation_handler_match_name()
		{
			// Arrange
			var violationHandlers = Enumerable.Empty<IPolicyViolationHandler>();
			var failureResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Access denied");
			var policy = new DelegatePolicy("Test", c => failureResult);
			var delegatePolicyResult = new DelegatePolicyResult(failureResult, policy.Name, policy.ViolationHandler);
			var exception = TestDataFactory.CreatePolicyViolationException(delegatePolicyResult);
			var handler = new DelegatePolicyViolationHandler(violationHandlers);

			// Act & assert
			var caughtException = Assert.Throws<PolicyViolationException>(() => handler.Handle(exception));
			Assert.That(caughtException, Is.EqualTo(exception));
		}

		[Test]
		public void Should_return_action_result_from_explicitly_set_violation_handler()
		{
			// Arrange
			var violationHandlers = Enumerable.Empty<IPolicyViolationHandler>();
			var expectedResult = new ContentResult { Content = "Some content" };
			var failureResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Access denied");
			var policy = new DelegatePolicy("Test", c => failureResult, e => expectedResult);
			var delegatePolicyResult = new DelegatePolicyResult(failureResult, policy.Name, policy.ViolationHandler);
			var exception = TestDataFactory.CreatePolicyViolationException(delegatePolicyResult);
			var handler = new DelegatePolicyViolationHandler(violationHandlers);

			// Act
			var result = handler.Handle(exception);

			// Assert
			Assert.That(result, Is.EqualTo(expectedResult));
		}

		[Test]
		public void Should_return_action_result_from_violation_handler_that_match_name()
		{
			// Arrange
			var nonMatchingNameViolationHandler = new NonMatchingNameViolationHandler();
			var matchingNameViolationHandler = new MatchingNameViolationHandler();
			var violationHandlers = new List<IPolicyViolationHandler>
			{
				nonMatchingNameViolationHandler,
				matchingNameViolationHandler
			};
			var failureResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Access denied");
			var policy = new DelegatePolicy("MatchingName", c => failureResult);
			var delegatePolicyResult = new DelegatePolicyResult(failureResult, policy.Name, policy.ViolationHandler);
			var exception = TestDataFactory.CreatePolicyViolationException(delegatePolicyResult);
			var handler = new DelegatePolicyViolationHandler(violationHandlers);

			// Act
			var result = handler.Handle(exception);

			// Assert
			Assert.That(result, Is.EqualTo(matchingNameViolationHandler.ActionResult));
		}

		public class MatchingNameViolationHandler : IPolicyViolationHandler
		{
			public ActionResult ActionResult = new EmptyResult();

			public ActionResult Handle(PolicyViolationException exception)
			{
				return ActionResult;
			}
		}

		public class NonMatchingNameViolationHandler : IPolicyViolationHandler
		{
			public ActionResult ActionResult = new EmptyResult();

			public ActionResult Handle(PolicyViolationException exception)
			{
				return ActionResult;
			}
		}
	}
}