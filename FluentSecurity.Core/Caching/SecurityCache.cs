using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Caching
{
	public class SecurityCache : ISecurityCache
	{
		private readonly ILifecycleResolver _lifecycleResolver;

		public SecurityCache(ILifecycleResolver lifecycleResolver)
		{
			_lifecycleResolver = lifecycleResolver;
		}
		
		public T Get<T>(string cacheKey, Lifecycle lifecycle)
		{
			var objectCache = _lifecycleResolver.Resolve(lifecycle).FindCache();
			return (T) objectCache.Get(cacheKey);
		}

		public void Store<T>(T item, string cacheKey, Lifecycle lifecycle)
		{
			var cache = _lifecycleResolver.Resolve(lifecycle).FindCache();
			cache.Set(cacheKey, item);
		}

		public void Clear(Lifecycle lifecycle)
		{
			var objectCache = _lifecycleResolver.Resolve(lifecycle).FindCache();
			objectCache.Clear();
		}
	}
}