using System;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class NullObjectCache : IObjectCache
	{
		public object Get(Guid key)
		{
			return null;
		}

		public void Set(Guid key, object instance) {}
	}
}