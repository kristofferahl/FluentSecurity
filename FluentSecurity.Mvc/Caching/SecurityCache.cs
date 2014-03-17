using System;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Caching
{
	public class SecurityCache : ISecurityCache
	{
		internal static Func<ISecurityCache> CacheProvider = () => new SecurityCache();
		
		public static void ClearCache(Lifecycle lifecycle)
		{
			CacheProvider.Invoke().Clear(lifecycle);
		}
		
		public T Get<T>(string cacheKey, Lifecycle lifecycle)
		{
			var objectCache = lifecycle.Get().FindCache();
			return (T) objectCache.Get(cacheKey);
		}

		public void Store<T>(T item, string cacheKey, Lifecycle lifecycle)
		{
			var cache = lifecycle.Get().FindCache();
			cache.Set(cacheKey, item);
		}

		public void Clear(Lifecycle lifecycle)
		{
			var objectCache = lifecycle.Get().FindCache();
			objectCache.Clear();
		}
	}
}