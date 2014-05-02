using System;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class PolicyContainerConfigurationExtensions
	{
		public static IPolicyContainerConfiguration AllowAny(this IPolicyContainerConfiguration policyContainer)
		{
			policyContainer.AddPolicy(new IgnorePolicy());
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