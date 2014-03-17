using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer
	{
		string ControllerName { get; }
		string ActionName { get; }
		IEnumerable<ISecurityPolicy> GetPolicies();
		IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context);
	}
}