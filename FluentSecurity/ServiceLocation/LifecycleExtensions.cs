using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.ServiceLocation
{
	internal static class LifecycleExtensions
	{
		public static ILifecycle Get(this Lifecycle lifecycle)
		{
			switch (lifecycle)
			{
				case Lifecycle.Singleton:
					return Lifecycle<SingletonLifecycle>.Instance;
				case Lifecycle.HybridHttpContext:
					return Lifecycle<HybridHttpContextLifecycle>.Instance;
				case Lifecycle.HybridHttpSession:
					return Lifecycle<HybridHttpContextLifecycle>.Instance;
				default: // Transient
					return Lifecycle<TransientLifecycle>.Instance;
			}
		}
	}
}