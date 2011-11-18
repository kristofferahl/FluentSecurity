using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public interface IConventionPolicyContainer
	{
		IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy);
		IConventionPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy;
	}
}