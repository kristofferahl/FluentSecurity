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

			if (securityPolicyToAdd.IsPolicyOf<IgnorePolicy>())
				policies.Clear();
			else if (securityPolicyToAdd.IsPolicyOf<DenyAnonymousAccessPolicy>())
				policies.Clear();
			else if (securityPolicyToAdd.IsPolicyOf<DenyAuthenticatedAccessPolicy>())
				policies.Clear();
			else if (securityPolicyToAdd.IsPolicyOf<RequireAnyRolePolicy>())
				policies.Clear();
			else if (securityPolicyToAdd.IsPolicyOf<RequireAllRolesPolicy>())
				policies.Clear();
			
			policies.Add(securityPolicyToAdd);
		}
	}
}