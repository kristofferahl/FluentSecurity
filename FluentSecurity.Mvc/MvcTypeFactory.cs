using FluentSecurity.Core;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class MvcTypeFactory : ITypeFactory
	{
		public ILazySecurityPolicy CreateLazySecurityPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new LazySecurityPolicy<TSecurityPolicy>();
		}
	}
}