using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper
{
	public abstract class SecurityExpectationHelper<TController> where TController : IController
	{
		protected abstract ISecurityConfiguration ConfigurationToTest();

		protected IEnumerable<ExpectationResult> VerifyExpectations(Action<PolicyExpectationsExpression<TController>> expectationExpression)
		{
			var policyExpectationsExpression = new PolicyExpectationsExpression<TController>();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			return policyExpectations.VerifyAll(ConfigurationToTest());
		}
	}

	public abstract class SecurityExpectationHelper
	{
		protected abstract ISecurityConfiguration ConfigurationToTest();

		protected IEnumerable<ExpectationResult> VerifyExpectations(Action<PolicyExpectationsExpression> expectationExpression)
		{
			var policyExpectationsExpression = new PolicyExpectationsExpression();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			return policyExpectations.VerifyAll(ConfigurationToTest());
		}
	}
}