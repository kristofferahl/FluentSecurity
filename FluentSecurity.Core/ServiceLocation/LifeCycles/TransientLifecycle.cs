namespace FluentSecurity.ServiceLocation.LifeCycles
{
	public class TransientLifecycle : ILifecycle
	{
		public IObjectCache FindCache()
		{
			return new NullObjectCache();
		}
	}
}