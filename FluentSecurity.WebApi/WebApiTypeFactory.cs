using FluentSecurity.Core;
using FluentSecurity.Policy;

namespace FluentSecurity.WebApi
{
	public class WebApiTypeFactory : ITypeFactory
	{
		public ILazySecurityPolicy CreateLazySecurityPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			throw new System.NotImplementedException();
		}
	}
}