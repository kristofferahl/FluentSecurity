using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration : IAdvancedConfiguration
	{
		internal AdvancedConfiguration()
		{
			DoNotCacheCacheResults();
		}

		public Cache DefaultResultsCacheLevel { get; private set; }

		public void CacheResultsPerHttpRequest()
		{
			DefaultResultsCacheLevel = Cache.PerHttpRequest;
		}

		public void DoNotCacheCacheResults()
		{
			DefaultResultsCacheLevel = Cache.DoNotCache;
		}

		public void CacheResultsPerHttpSession()
		{
			DefaultResultsCacheLevel = Cache.PerHttpSession;
		}
	}
}