using System;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Results;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.Results
{
	[TestFixture]
	[Category("DelegatePolicyResultSpec")]
	public class When_creating_a_DelegatePolicyResult
	{
		[Test]
		public void Should_have_values_from_policyresult_and_violation_handler()
		{
			// Arrange
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");
			Func<PolicyViolationException, ActionResult> violationHandler = e => new EmptyResult();

			// Act
			var delegatePolicyResult = new DelegatePolicyResult(policyResult, violationHandler);

			// Assert
			Assert.That(delegatePolicyResult.Message, Is.EqualTo(policyResult.Message));
			Assert.That(delegatePolicyResult.PolicyType, Is.EqualTo(policyResult.PolicyType));
			Assert.That(delegatePolicyResult.ViolationOccured, Is.EqualTo(policyResult.ViolationOccured));
			Assert.That(delegatePolicyResult.ViolationHandler, Is.EqualTo(violationHandler));
		}

		[Test]
		public void Should_have_values_from_policyresult_but_no_violation_handler()
		{
			// Arrange
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");

			// Act
			var delegatePolicyResult = new DelegatePolicyResult(policyResult, null);

			// Assert
			Assert.That(delegatePolicyResult.Message, Is.EqualTo(policyResult.Message));
			Assert.That(delegatePolicyResult.PolicyType, Is.EqualTo(policyResult.PolicyType));
			Assert.That(delegatePolicyResult.ViolationOccured, Is.EqualTo(policyResult.ViolationOccured));
			Assert.That(delegatePolicyResult.ViolationHandler, Is.Null);
		}
	}
}