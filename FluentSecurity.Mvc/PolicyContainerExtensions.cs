using System;
using System.Web.Mvc;
using FluentSecurity.Configuration;
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

		public static IPolicyContainerConfiguration<DenyAnonymousAccessPolicy> DenyAnonymousAccess(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new DenyAnonymousAccessPolicy());
			return new PolicyContainerConfigurationWrapper<DenyAnonymousAccessPolicy>(policyContainer);
		}

		public static IPolicyContainerConfiguration<DenyAuthenticatedAccessPolicy> DenyAuthenticatedAccess(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new DenyAuthenticatedAccessPolicy());
			return new PolicyContainerConfigurationWrapper<DenyAuthenticatedAccessPolicy>(policyContainer);
		}

		public static IPolicyContainerConfiguration Ignore(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new IgnorePolicy());
			return policyContainer;
		}

		[Obsolete("Use RequireAnyRole instead.")]
		public static IPolicyContainerConfiguration<RequireRolePolicy> RequireRole(this IPolicyContainerConfiguration policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireRolePolicy(roles));
			return new PolicyContainerConfigurationWrapper<RequireRolePolicy>(policyContainer);
		}

		public static IPolicyContainerConfiguration<RequireAnyRolePolicy> RequireAnyRole(this IPolicyContainerConfiguration policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireAnyRolePolicy(roles));
			return new PolicyContainerConfigurationWrapper<RequireAnyRolePolicy>(policyContainer);
		}

		public static IPolicyContainerConfiguration<RequireAllRolesPolicy> RequireAllRoles(this IPolicyContainerConfiguration policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireAllRolesPolicy(roles));
			return new PolicyContainerConfigurationWrapper<RequireAllRolesPolicy>(policyContainer);
		}
	}
}