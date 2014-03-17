using System;
using System.Web.Mvc;

namespace FluentSecurity.Policy.Results
{
	public class DelegatePolicyResult : PolicyResult
	{
		public string PolicyName { get; private set; }
		public Func<PolicyViolationException, ActionResult> ViolationHandler { get; private set; }

		public DelegatePolicyResult(PolicyResult policyResult, string policyName, Func<PolicyViolationException, ActionResult> violationHandler)
			: base(policyResult.Message, policyResult.ViolationOccured, policyResult.PolicyType)
		{
			if (String.IsNullOrWhiteSpace(policyName))
				throw new ArgumentException("policyName");

			PolicyName = policyName;
			ViolationHandler = violationHandler;
		}
	}
}