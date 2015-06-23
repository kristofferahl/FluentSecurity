using FluentSecurity.Policy;

namespace FluentSecurity.Core
{
	public interface ILazySecurityPolicyFactory
	{
		ILazySecurityPolicy Create<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy;
	}
}