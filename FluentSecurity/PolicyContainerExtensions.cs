using System;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;

namespace FluentSecurity
{
	public static class PolicyContainerExtensions
	{
		public static IPolicyContainerConfiguration AllowAny(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new IgnorePolicy());
			return policyContainer;
		}

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

		public static IPolicyContainerConfiguration DenyAnonymousAccess(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new DenyAnonymousAccessPolicy());
			return policyContainer;
		}

		public static IPolicyContainerConfiguration DenyAuthenticatedAccess(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new DenyAuthenticatedAccessPolicy());
			return policyContainer;
		}

		public static IPolicyContainerConfiguration Ignore(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new IgnorePolicy());
			return policyContainer;
		}

		public static IPolicyContainerConfiguration RequireRole(this IPolicyContainerConfiguration policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireRolePolicy(roles));
			return policyContainer;
		}

		public static IPolicyContainerConfiguration RequireAllRoles(this IPolicyContainerConfiguration policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireAllRolesPolicy(roles));
			return policyContainer;
		}
	}
}