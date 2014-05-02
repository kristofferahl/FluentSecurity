using System;
using System.Web.Mvc;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;

namespace FluentSecurity
{
	public static class MvcPolicyContainerConfigurationExtensions
	{
		public static IPolicyContainerConfiguration DelegatePolicy(this IPolicyContainerConfiguration policyContainer, string uniqueName, Func<DelegateSecurityContext, PolicyResult> policyDelegate, Func<PolicyViolationException, ActionResult> violationHandlerDelegate = null)
		{
			policyContainer.AddPolicy(new DelegatePolicy(uniqueName, policyDelegate, violationHandlerDelegate));
			return policyContainer;
		}

		public static IPolicyContainerConfiguration DelegatePolicy(this IPolicyContainerConfiguration policyContainer, string uniqueName, Func<DelegateSecurityContext, bool> policyDelegate, Func<PolicyViolationException, ActionResult> violationHandlerDelegate = null, string failureMessage = "Access denied")
		{
			Func<DelegateSecurityContext, PolicyResult> booleanPolicyDelegate =
				context => policyDelegate.Invoke(context)
					? PolicyResult.CreateSuccessResult(context.Policy)
					: PolicyResult.CreateFailureResult(context.Policy, failureMessage);

			policyContainer.AddPolicy(new DelegatePolicy(uniqueName, booleanPolicyDelegate, violationHandlerDelegate));
			return policyContainer;
		}
	}
}