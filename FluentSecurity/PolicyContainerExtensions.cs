using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class PolicyContainerExtensions
	{
		public static IPolicyContainer DenyAnonymousAccess(this IPolicyContainer policyContainer)
		{
			policyContainer.AddPolicy(new DenyAnonymousAccessPolicy());
			return policyContainer;
		}

		public static IPolicyContainer DenyAuthenticatedAccess(this IPolicyContainer policyContainer)
		{
			policyContainer.AddPolicy(new DenyAuthenticatedAccessPolicy());
			return policyContainer;
		}

		public static IPolicyContainer Ignore(this IPolicyContainer policyContainer)
		{
			policyContainer.AddPolicy(new IgnorePolicy());
			return policyContainer;
		}

		public static IPolicyContainer RequireRole(this IPolicyContainer policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireRolePolicy(roles));
			return policyContainer;
		}

		public static IPolicyContainer RequireAllRoles(this IPolicyContainer policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireAllRolesPolicy(roles));
			return policyContainer;
		}
	}
}