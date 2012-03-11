using System;
using System.Collections.Concurrent;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	[Serializable]
	internal class ObjectCache : IObjectCache
	{
		private readonly ConcurrentDictionary<Guid, object> _objects = new ConcurrentDictionary<Guid, object>();

		public int Count
		{
			get { return _objects.Count; }
		}

		public object Get(Guid key)
		{
			var hasInstance = Has(key);
			return hasInstance ? _objects[key] : null;
		}

		public void Set(Guid key, object instance)
		{
			if (instance == null) return;
	
			if (Has(key))
			{
				var message = string.Format("An instance for key {0} is already in the cache.", key);
				throw new ArgumentException(message, "key");
			}

			_objects[key] = instance;
		}

		private bool Has(Guid key)
		{
			var containsKey = _objects.ContainsKey(key);
			return containsKey;
		}
	}
}