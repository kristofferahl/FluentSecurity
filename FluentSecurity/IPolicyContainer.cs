using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer
	{
		string ControllerName { get; }
		string ActionName { get; }
		IPolicyManager Manager { get; }
		IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IEnumerable<ISecurityPolicy> GetPolicies();
		void EnforcePolicies();
	}
}