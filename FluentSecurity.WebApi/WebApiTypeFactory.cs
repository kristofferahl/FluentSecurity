using FluentSecurity.Core;
using FluentSecurity.Policy;

namespace FluentSecurity.WebApi
{
	public class WebApiTypeFactory : ITypeFactory
	{
		public ILazySecurityPolicy CreateLazySecurityPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new LazySecurityPolicy<TSecurityPolicy>();
		}
	}
}