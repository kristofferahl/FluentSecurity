using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer : IPolicyContainerConfiguration
	{
		string ControllerName { get; }
		string ActionName { get; }
		IEnumerable<ISecurityPolicy> GetPolicies();
		IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context);
	}
}