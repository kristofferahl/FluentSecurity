using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Core
{
	public interface IFluentConfiguration
	{
		ISecurityRuntime GetRuntime();
		IContainer GetContainer();
	}
}