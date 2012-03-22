using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration : IAdvancedConfiguration
	{
		internal AdvancedConfiguration()
		{
			DoNotCacheCacheResults();
		}

		public Cache DefaultResultsCacheLifecycle { get; private set; }

		public void CacheResultsPerHttpRequest()
		{
			DefaultResultsCacheLifecycle = Cache.PerHttpRequest;
		}

		public void DoNotCacheCacheResults()
		{
			DefaultResultsCacheLifecycle = Cache.DoNotCache;
		}

		public void CacheResultsPerHttpSession()
		{
			DefaultResultsCacheLifecycle = Cache.PerHttpSession;
		}
	}
}