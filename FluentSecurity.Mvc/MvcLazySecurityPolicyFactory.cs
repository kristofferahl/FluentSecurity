using FluentSecurity.Core;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class MvcLazySecurityPolicyFactory : ILazySecurityPolicyFactory
	{
		public ILazySecurityPolicy Create<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new LazySecurityPolicy<TSecurityPolicy>();
		}
	}
}