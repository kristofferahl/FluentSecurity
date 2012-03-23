using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IPolicyContainer
	{
		string ControllerName { get; }
		string ActionName { get; }
		IPolicyAppender PolicyAppender { get; }
		IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy, Cache lifecycle);
		IPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy;
		IEnumerable<ISecurityPolicy> GetPolicies();
		IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context);
	}
}