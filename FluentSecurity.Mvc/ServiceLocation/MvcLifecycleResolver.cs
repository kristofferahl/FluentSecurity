using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.ServiceLocation
{
	public class MvcLifecycleResolver : ILifecycleResolver
	{
		public ILifecycle Resolve(Lifecycle lifecycle)
		{
			return lifecycle.Get();
		}
	}
}