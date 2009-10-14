using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class ConventionPolicyContainerExtensions
	{
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
	}
}