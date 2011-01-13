using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper
{
	public abstract class SecurityExpectationHelperBase<TController> where TController : IController
	{
		protected SecurityExpectationHelperBase()
		{
			ExpectationViolationHandler = Settings.DefaultExpectationViolationHandler;
		}

		protected IExpectationViolationHandler ExpectationViolationHandler { get; set; }

		protected abstract ISecurityConfiguration ConfigurationToTest();

		protected IEnumerable<ExpectationResult> VerifyExpectations(Action<PolicyExpectationsExpression<TController>> expectationExpression)
		{
			var policyExpectationsExpression = new PolicyExpectationsExpression<TController>();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			policyExpectations.SetExpectationViolationHandler(ExpectationViolationHandler);
			return policyExpectations.VerifyAll(ConfigurationToTest());
		}
	}

	public abstract class SecurityExpectationHelperBase
	{
		protected SecurityExpectationHelperBase()
		{
			ExpectationViolationHandler = Settings.DefaultExpectationViolationHandler;
		}

		protected IExpectationViolationHandler ExpectationViolationHandler { get; set; }

		protected abstract ISecurityConfiguration ConfigurationToTest();

		protected IEnumerable<ExpectationResult> VerifyExpectations(Action<PolicyExpectationsExpression> expectationExpression)
		{
			var policyExpectationsExpression = new PolicyExpectationsExpression();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			policyExpectations.SetExpectationViolationHandler(ExpectationViolationHandler);
			return policyExpectations.VerifyAll(ConfigurationToTest());
		}
	}
}