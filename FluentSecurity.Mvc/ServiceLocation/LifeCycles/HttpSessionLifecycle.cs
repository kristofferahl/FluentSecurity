using System;
using System.Collections;
using System.Web;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class HttpSessionLifecycle : ILifecycle
	{
		public const string CacheKey = "FluentSecurity-HttpSessionCache";

		public static Func<bool> HasSessionResolver;
		public static Func<IDictionary> DictionaryResolver;

		public HttpSessionLifecycle()
		{
			HasSessionResolver = () => HttpContext.Current != null && HttpContext.Current.Session != null;
			DictionaryResolver = () => new SessionWrapper(new HttpSessionStateWrapper(HttpContext.Current.Session));
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

		public bool HasSession()
		{
			return HasSessionResolver.Invoke();
		}

		private IDictionary FindHttpDictionary()
		{
			if (!HasSession()) throw new InvalidOperationException("HttpContext.Current.Session is not available.");
			return DictionaryResolver.Invoke();
		}

		// Based on the Structuremap SessionWrapper.
		// https://github.com/structuremap/structuremap/blob/master/Source/StructureMap/Pipeline/SessionWrapper.cs
		private class SessionWrapper : IDictionary
		{
			private readonly HttpSessionStateBase _session;

			public SessionWrapper(HttpSessionStateBase session)
			{
				_session = session;
			}

			#region IDictionary Members

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public void CopyTo(Array array, int index)
			{
				_session.CopyTo(array, index);
			}

			public int Count
			{
				get { return _session.Count; }
			}

			public object SyncRoot
			{
				get { return _session.SyncRoot; }
			}

			public bool IsSynchronized
			{
				get { return _session.IsSynchronized; }
			}

			public bool Contains(object key)
			{
				return _session[key.ToString()] != null;
			}

			public void Add(object key, object value)
			{
				_session.Add(key.ToString(), value);
			}

			public void Clear()
			{
				_session.Clear();
			}

			public IDictionaryEnumerator GetEnumerator()
			{
				throw new NotImplementedException("Not supported by SessionWrapper.");
			}

			public void Remove(object key)
			{
				_session.Remove(key.ToString());
			}

			public object this[object key]
			{
				get { return _session[key.ToString()]; }
				set { _session[key.ToString()] = value; }
			}

			public ICollection Keys
			{
				get { return _session.Keys; }
			}

			public ICollection Values
			{
				get { throw new NotImplementedException("Not supported by SessionWrapper."); }
			}

			public bool IsReadOnly
			{
				get { return _session.IsReadOnly; }
			}

			public bool IsFixedSize
			{
				get { return false; }
			}

			#endregion
		}
	}
}