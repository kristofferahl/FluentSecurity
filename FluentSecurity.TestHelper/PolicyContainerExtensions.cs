using System;
using System.Linq;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper
{
	public static class PolicyContainerExtensions
	{
		public static bool HasPolicyOfType(this IPolicyContainer policyContainer, Type policyType)
		{
			var policies = policyContainer.GetPolicies();
			return policies.Any(x => x.GetType() == policyType);
		}

		public static bool HasPolicy<TSecurityPolicy>(this IPolicyContainer policyContainer, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			var policies = policyContainer.GetPolicies();
			return policies.Contains(instance);
		}
	}
}