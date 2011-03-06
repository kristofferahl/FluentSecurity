using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer
	{
		string ControllerName { get; }
		string ActionName { get; }
		IPolicyAppender PolicyAppender { get; }
		IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IEnumerable<ISecurityPolicy> GetPolicies();
		IEnumerable<PolicyResult> EnforcePolicies();
	}
}