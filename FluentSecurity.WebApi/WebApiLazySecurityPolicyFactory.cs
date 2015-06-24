using FluentSecurity.Core;
using FluentSecurity.Policy;
using FluentSecurity.WebApi.Policy;

namespace FluentSecurity.WebApi
{
	public class WebApiLazySecurityPolicyFactory : ILazySecurityPolicyFactory
	{
		public ILazySecurityPolicy Create<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new LazySecurityPolicy<TSecurityPolicy>();
		}
	}
}