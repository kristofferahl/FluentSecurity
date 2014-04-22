using System;
using FluentSecurity.ServiceLocation;
using FluentSecurity.ServiceLocation.LifeCycles;

namespace FluentSecurity.WebApi
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
					throw new NotImplementedException("Lifecycle HybridHttpContext is not supported");
				case Lifecycle.HybridHttpSession:
					throw new NotImplementedException("Lifecycle HybridHttpSession is not supported");
				default: // Transient
					return Lifecycle<TransientLifecycle>.Instance;
			}
		}
	}
}