using FluentSecurity.ServiceLocation;
using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.WebApi
{
	public class WebApiLifecycleResolver : ILifecycleResolver
	{
		public ILifecycle Resolve(Lifecycle lifecycle)
		{
			return lifecycle.Get();
		}
	}
}