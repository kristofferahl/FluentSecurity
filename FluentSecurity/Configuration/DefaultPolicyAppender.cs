using System;
using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity.Configuration
{
	public class DefaultPolicyAppender : IPolicyAppender
	{
		public void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies)
		{
			if (securityPolicyToAdd == null) throw new ArgumentNullException("securityPolicyToAdd");
			if (policies == null) throw new ArgumentNullException("policies");

			if (securityPolicyToAdd is IgnorePolicy)
				policies.Clear();
			else if (securityPolicyToAdd is DenyAnonymousAccessPolicy)
				policies.Clear();
			else if (securityPolicyToAdd is DenyAuthenticatedAccessPolicy)
				policies.Clear();
			else if (securityPolicyToAdd is RequireRolePolicy)
				policies.Clear();
			else if (securityPolicyToAdd is RequireAllRolesPolicy)
				policies.Clear();
			
			policies.Add(securityPolicyToAdd);
		}
	}
}