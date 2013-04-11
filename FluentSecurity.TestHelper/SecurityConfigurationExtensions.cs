using System;
using System.Collections.Generic;

namespace FluentSecurity.TestHelper
{
	public static class SecurityConfigurationExtensions
	{
		public static IEnumerable<ExpectationResult> Verify(this ISecurityConfiguration configuration, Action<PolicyExpectationsExpression> expectationExpression)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			if (expectationExpression == null) throw new ArgumentNullException("expectationExpression");
			
			var policyExpectationsExpression = new PolicyExpectationsExpression();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			return policyExpectations.VerifyAll(configuration);
		}

		public static IEnumerable<ExpectationResult> Verify<TController>(this ISecurityConfiguration configuration, Action<PolicyExpectationsExpression<TController>> expectationExpression)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			if (expectationExpression == null) throw new ArgumentNullException("expectationExpression");

			var policyExpectationsExpression = new PolicyExpectationsExpression<TController>();
			expectationExpression(policyExpectationsExpression);
			var policyExpectations = policyExpectationsExpression.Expectations;
			return policyExpectations.VerifyAll(configuration);
		}
	}
}