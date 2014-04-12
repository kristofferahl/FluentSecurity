using FluentSecurity.Policy;

namespace FluentSecurity.Core
{
	public interface ITypeFactory
	{
		ILazySecurityPolicy CreateLazySecurityPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy;
	}
}