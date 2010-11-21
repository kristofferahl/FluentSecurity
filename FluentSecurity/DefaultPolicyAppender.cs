using System;
using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class DefaultPolicyAppender : IPolicyAppender
	{
		public void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies)
		{
			if (securityPolicyToAdd == null) throw new ArgumentNullException("securityPolicyToAdd");
			if (policies == null) throw new ArgumentNullException("policies");

			if (securityPolicyToAdd is IgnorePolicy)
				PrepareForIgnorePolicy(policies);
			else if (securityPolicyToAdd is DenyAnonymousAccessPolicy)
				PrepareForDenyAnonymousAccessPolicy(policies);
			else if (securityPolicyToAdd is DenyAuthenticatedAccessPolicy)
				PrepareForDenyAuthenticatedAccessPolicy(policies);
			else if (securityPolicyToAdd is RequireRolePolicy)
				PrepareForRequireRolePolicy(policies);
			
			policies.Add(securityPolicyToAdd);
		}

		private static void PrepareForRequireRolePolicy(IList<ISecurityPolicy> policies)
		{
			policies.Clear();
		}

		private static void PrepareForIgnorePolicy(IList<ISecurityPolicy> policies)
		{
			policies.Clear();
		}

		private static void PrepareForDenyAnonymousAccessPolicy(IList<ISecurityPolicy> policies)
		{
			policies.Clear();
		}

		private static void PrepareForDenyAuthenticatedAccessPolicy(IList<ISecurityPolicy> policies)
		{
			policies.Clear();
		}
	}
}