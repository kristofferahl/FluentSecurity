using System;
using System.Collections.Concurrent;
using System.Linq;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	[Serializable]
	public class ObjectCache : IObjectCache
	{
		private readonly ConcurrentDictionary<object, object> _objects = new ConcurrentDictionary<object, object>();

		public int Count
		{
			get { return _objects.Count; }
		}

		public object Get(object key)
		{
			var hasInstance = Has(key);
			return hasInstance ? _objects[key] : null;
		}

		public void Add(object key, object instance)
		{
			if (instance == null) return;

			if (Has(key))
			{
				var message = string.Format("An instance of type {1} is already in the cache with the key {0}.", key, instance.GetType().FullName);
				throw new ArgumentException(message, "key");
			}

			_objects[key] = instance;
		}

		public void Set(object key, object instance)
		{
			if (instance == null) return;

			_objects[key] = instance;
		}

		public void Clear()
		{
			_objects.Values.ToList().ForEach(TryDispose);
			_objects.Clear();
		}

		private static void TryDispose(object cachedObject)
		{
			var disposable = cachedObject as IDisposable;
			if (disposable != null) disposable.Dispose();
		}

		private bool Has(object key)
		{
			var containsKey = _objects.ContainsKey(key);
			return containsKey;
		}
	}
}