using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity.Specification.TestData
{
	public class FakePolicyAppender : IPolicyAppender
	{
		public void UpdatePolicies(ISecurityPolicy securityPolicyToAdd, IList<ISecurityPolicy> policies)
		{
			policies.Add(securityPolicyToAdd);
		}
	}
}