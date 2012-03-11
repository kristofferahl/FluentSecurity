using System;

namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal interface IObjectCache
	{
		object Get(Guid key);
		void Set(Guid key, object instance);
	}
}