namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class SingletonLifecycle : ILifecycle
	{
		private readonly IObjectCache _cache = new ObjectCache();

		public IObjectCache FindCache()
		{
			return _cache;
		}
	}
}