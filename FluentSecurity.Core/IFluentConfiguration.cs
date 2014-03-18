using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Core
{
	public interface IFluentConfiguration
	{
		ISecurityRuntime GetRuntime();
		IRegistry GetRegistry();
		ILifecycleResolver GetLifecycleResolver();
	}
}