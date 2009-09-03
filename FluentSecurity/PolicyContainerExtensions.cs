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

		public static IPolicyContainer RequireRole(this IPolicyContainer policyContainer, params object[] roles)
		{
			policyContainer.AddPolicy(new RequireRolePolicy(roles));
			return policyContainer;
		}
	}
}