using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Caching
{
	public static class PolicyResultCacheExtensions
	{
		public static Lifecycle ToLifecycle(this Cache cache)
		{
			switch (cache)
			{
				case Cache.PerHttpRequest:
					return Lifecycle.HybridHttpContext;
				case Cache.PerHttpSession:
					return Lifecycle.HybridHttpSession;
				default: // DoNotCache
					return Lifecycle.Transient;
			}
		}
	}
}