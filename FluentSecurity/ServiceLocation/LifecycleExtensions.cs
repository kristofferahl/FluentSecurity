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
				default: // Transient
					return Lifecycle<TransientLifecycle>.Instance;
			}
		}
	}
}