using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyManager
	{
		void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies);
	}
}