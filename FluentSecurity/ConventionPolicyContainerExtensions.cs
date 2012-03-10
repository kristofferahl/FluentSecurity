using System;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;

namespace FluentSecurity
{
	public static class ConventionPolicyContainerExtensions
	{
		public static IConventionPolicyContainer AllowAny(this IConventionPolicyContainer conventionPolicyContainer)
		{
			conventionPolicyContainer.AddPolicy(new IgnorePolicy());
			return conventionPolicyContainer;
		}

		public static IConventionPolicyContainer DelegatePolicy(this IConventionPolicyContainer policyContainer, string uniqueName, Func<DelegateSecurityContext, PolicyResult> policyDelegate, Func<PolicyViolationException, ActionResult> violationHandlerDelegate = null)
		{
			policyContainer.AddPolicy(new DelegatePolicy(uniqueName, policyDelegate, violationHandlerDelegate));
			return policyContainer;
		}

		public static IConventionPolicyContainer DelegatePolicy(this IConventionPolicyContainer policyContainer, string uniqueName, Func<DelegateSecurityContext, bool> policyDelegate, Func<PolicyViolationException, ActionResult> violationHandlerDelegate = null, string failureMessage = "Access denied")
		{
			Func<DelegateSecurityContext, PolicyResult> booleanPolicyDelegate =
				context => policyDelegate.Invoke(context)
					? PolicyResult.CreateSuccessResult(context.Policy)
					: PolicyResult.CreateFailureResult(context.Policy, failureMessage);

			policyContainer.AddPolicy(new DelegatePolicy(uniqueName, booleanPolicyDelegate, violationHandlerDelegate));
			return policyContainer;
		}

		public static IConventionPolicyContainer DenyAnonymousAccess(this IConventionPolicyContainer conventionPolicyContainer)
		{
			conventionPolicyContainer.AddPolicy(new DenyAnonymousAccessPolicy());
			return conventionPolicyContainer;
		}

		public static IConventionPolicyContainer DenyAuthenticatedAccess(this IConventionPolicyContainer conventionPolicyContainer)
		{
			conventionPolicyContainer.AddPolicy(new DenyAuthenticatedAccessPolicy());
			return conventionPolicyContainer;
		}

		public static IConventionPolicyContainer Ignore(this IConventionPolicyContainer conventionPolicyContainer)
		{
			conventionPolicyContainer.AddPolicy(new IgnorePolicy());
			return conventionPolicyContainer;
		}

		public static IConventionPolicyContainer RequireRole(this IConventionPolicyContainer conventionPolicyContainer, params object[] roles)
		{
			conventionPolicyContainer.AddPolicy(new RequireRolePolicy(roles));
			return conventionPolicyContainer;
		}

		public static IConventionPolicyContainer RequireAllRoles(this IConventionPolicyContainer policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireAllRolesPolicy(roles));
			return policyContainer;
		}
	}
}