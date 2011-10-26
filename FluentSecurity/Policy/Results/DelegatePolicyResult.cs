using System;
using System.Web.Mvc;

namespace FluentSecurity.Policy.Results
{
	public class DelegatePolicyResult : PolicyResult
	{
		public Func<PolicyViolationException, ActionResult> ViolationHandler { get; private set; }

		public DelegatePolicyResult(PolicyResult policyResult, Func<PolicyViolationException, ActionResult> violationHandler)
			: base(policyResult.Message, policyResult.ViolationOccured, policyResult.PolicyType)
		{
			ViolationHandler = violationHandler;
		}
	}
}