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
			const string policyName = "PolicyName1";
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");
			Func<PolicyViolationException, ActionResult> violationHandler = e => new EmptyResult();

			// Act
			var delegatePolicyResult = new DelegatePolicyResult(policyResult, policyName, violationHandler);

			// Assert
			Assert.That(delegatePolicyResult.Message, Is.EqualTo(policyResult.Message));
			Assert.That(delegatePolicyResult.PolicyType, Is.EqualTo(policyResult.PolicyType));
			Assert.That(delegatePolicyResult.PolicyName, Is.EqualTo(policyName));
			Assert.That(delegatePolicyResult.ViolationOccured, Is.EqualTo(policyResult.ViolationOccured));
			Assert.That(delegatePolicyResult.ViolationHandler, Is.EqualTo(violationHandler));
		}

		[Test]
		public void Should_have_values_from_policyresult_but_no_violation_handler()
		{
			// Arrange
			const string policyName = "PolicyName2";
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");

			// Act
			var delegatePolicyResult = new DelegatePolicyResult(policyResult, policyName, null);

			// Assert
			Assert.That(delegatePolicyResult.Message, Is.EqualTo(policyResult.Message));
			Assert.That(delegatePolicyResult.PolicyType, Is.EqualTo(policyResult.PolicyType));
			Assert.That(delegatePolicyResult.PolicyName, Is.EqualTo(policyName));
			Assert.That(delegatePolicyResult.ViolationOccured, Is.EqualTo(policyResult.ViolationOccured));
			Assert.That(delegatePolicyResult.ViolationHandler, Is.Null);
		}

		[Test]
		public void Should_throw_when_policyname_is_null()
		{
			// Arrange
			const string nullPolicyName = null;
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");

			// Act & assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicyResult(policyResult, nullPolicyName, null));
		}

		[Test]
		public void Should_throw_when_policyname_is_empty()
		{
			// Arrange
			const string emptyPolicyName = "";
			var policyResult = PolicyResult.CreateFailureResult(new IgnorePolicy(), "Failure message");

			// Act & assert
			Assert.Throws<ArgumentException>(() => new DelegatePolicyResult(policyResult, emptyPolicyName, null));
		}
	}
}