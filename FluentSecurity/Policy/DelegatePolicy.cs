using System;
using System.Web.Mvc;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Policy.Results;

namespace FluentSecurity.Policy
{
	public class DelegatePolicy : ISecurityPolicy
	{
		public string Name { get; private set; }
		public Func<DelegateSecurityContext, PolicyResult> Policy { get; private set; }
		public Func<PolicyViolationException, ActionResult> ViolationHandler { get; private set; }

		public DelegatePolicy(string uniqueName, Func<DelegateSecurityContext, PolicyResult> policyDelegate, Func<PolicyViolationException, ActionResult> violationHandlerDelegate = null)
		{
			if (String.IsNullOrWhiteSpace(uniqueName))
				throw new ArgumentException("uniqueName");

			if (policyDelegate == null)
				throw new ArgumentNullException("policyDelegate");

			Name = uniqueName;
			Policy = policyDelegate;
			ViolationHandler = violationHandlerDelegate;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			var wrappedContext = new DelegateSecurityContext(this, context);
			var policyResult = Policy.Invoke(wrappedContext);
			return new DelegatePolicyResult(policyResult, Name, ViolationHandler);
		}
	}
}