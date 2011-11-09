using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public static class PolicyContainerExtensions
	{
		public static bool HasPolicyMatching(this IPolicyContainer policyContainer, HasTypeExpectation predicateExpectation)
		{
			var policies = policyContainer.GetPolicies();
			return policies.Any(predicateExpectation.IsMatch);
		}

		public static bool HasPolicyMatching(this IPolicyContainer policyContainer, DoesNotHaveTypeExpectation predicateExpectation)
		{
			var policies = policyContainer.GetPolicies();
			return policies.Any(predicateExpectation.IsMatch);
		}

		public static bool HasPolicy<TSecurityPolicy>(this IPolicyContainer policyContainer, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			var policies = policyContainer.GetPolicies();
			return policies.Contains(instance);
		}
	}
}