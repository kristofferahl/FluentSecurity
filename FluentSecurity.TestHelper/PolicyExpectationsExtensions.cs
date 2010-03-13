using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.TestHelper
{
	public static class PolicyExpectationsExtensions
	{
		public static PolicyExpectations Has<TSecurityPolicy>(this PolicyExpectations expectations) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var keyValuePair in expectations.ExpectationsFor)
			{
				var policyContainer = GetPolicyContainer(expectations.PolicyContainers, keyValuePair.Key, keyValuePair.Value, true);
				if (policyContainer.HasPolicyOfType<TSecurityPolicy>() == false)
				{
					const string messageFormat = "Expected policy of type \"{2}\" for controller \"{0}\", action \"{1}\"!";
					var expectedPolicyType = typeof(TSecurityPolicy).Name;
					var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectedPolicyType);
					throw new AssertionException(message);
				}
			}
			return expectations;
		}

		public static PolicyExpectations DoesNotHave<TSecurityPolicy>(this PolicyExpectations expectations) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var keyValuePair in expectations.ExpectationsFor)
			{
				var policyContainer = GetPolicyContainer(expectations.PolicyContainers, keyValuePair.Key, keyValuePair.Value, false);
				if (policyContainer != null && policyContainer.HasPolicyOfType<TSecurityPolicy>())
				{
					const string messageFormat = "An unexpected policy of type \"{2}\" was found for controller \"{0}\", action \"{1}\".";
					var expectedPolicyType = typeof(TSecurityPolicy).Name;
					var message = string.Format(messageFormat, policyContainer.ControllerName, policyContainer.ActionName, expectedPolicyType);
					throw new AssertionException(message);
				}
			}
			return expectations;
		}

		private static bool HasPolicyOfType<TSecurityPolicy>(this IPolicyContainer policyContainer) where TSecurityPolicy : ISecurityPolicy
		{
			var policies = policyContainer.GetPolicies();
			return policies.OfType<TSecurityPolicy>().Count() > 0;
		}

		private static IPolicyContainer GetPolicyContainer(IEnumerable<IPolicyContainer> policyContainers, string controllerName, string actionName, bool throwIfMissing)
		{
			var policyContainer = policyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer == null && throwIfMissing)
			{
				const string messageFormat = "Expected policycontainer for controller \"{0}\", action \"{1}\" could not be found!";
				var message = string.Format(messageFormat, controllerName, actionName);
				throw new AssertionException(message);
			}
			return policyContainer;
		}
	}
}