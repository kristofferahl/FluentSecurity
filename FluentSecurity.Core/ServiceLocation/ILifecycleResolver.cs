using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.ServiceLocation
{
	public interface ILifecycleResolver
	{
		ILifecycle Resolve(Lifecycle lifecycle);
	}
}