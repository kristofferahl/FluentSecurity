namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class NullObjectCache : IObjectCache
	{
		public object Get(object key)
		{
			return null;
		}

		public void Set(object key, object instance) {}
		
		public void Clear() {}
	}
}