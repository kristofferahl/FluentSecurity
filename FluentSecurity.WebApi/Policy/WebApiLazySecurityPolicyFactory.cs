using FluentSecurity.Core;
using FluentSecurity.Policy;

namespace FluentSecurity.WebApi.Policy
{
	public class WebApiLazySecurityPolicyFactory : ILazySecurityPolicyFactory
	{
		public ILazySecurityPolicy Create<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new LazySecurityPolicy<TSecurityPolicy>();
		}
	}
}