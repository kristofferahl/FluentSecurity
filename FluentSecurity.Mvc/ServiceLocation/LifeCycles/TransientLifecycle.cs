namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal class TransientLifecycle : ILifecycle
	{
		public IObjectCache FindCache()
		{
			return new NullObjectCache();
		}
	}
}