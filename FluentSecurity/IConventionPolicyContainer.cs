using System;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IConventionPolicyContainer
	{
		IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy, Cache lifecycle);
		IConventionPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy;
	}
}