using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer
	{
		string ControllerName { get; }
		string ActionName { get; }
		void EnforcePolicies();
		IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IEnumerable<ISecurityPolicy> GetPolicies();
	}
}