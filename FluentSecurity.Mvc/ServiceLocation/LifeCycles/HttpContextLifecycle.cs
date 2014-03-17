using System;
using System.Collections;
using System.Web;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class HttpContextLifecycle : ILifecycle
	{
		public const string CacheKey = "FluentSecurity-HttpContextCache";

		public static Func<bool> HasContextResolver;
		public static Func<IDictionary> DictionaryResolver;

		public HttpContextLifecycle()
		{
			HasContextResolver = () => HttpContext.Current != null;
			DictionaryResolver = () => HttpContext.Current.Items;
		}

		public IObjectCache FindCache()
		{
			var items = FindHttpDictionary();
			if (!items.Contains(CacheKey))
			{
				lock (items.SyncRoot)
				{
					if (!items.Contains(CacheKey))
					{
						var cache = new ObjectCache();
						items.Add(CacheKey, cache);
						return cache;
					}
				}
			}

			return (ObjectCache)items[CacheKey];
		}

		public static bool HasContext()
		{
			return HasContextResolver.Invoke();
		}

		private IDictionary FindHttpDictionary()
		{
			if (!HasContext()) throw new InvalidOperationException("HttpContext is not available.");
			return DictionaryResolver.Invoke();
		}
	}
}