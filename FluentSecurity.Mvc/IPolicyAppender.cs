using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyAppender
	{
		void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies);
	}
}