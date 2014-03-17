namespace FluentSecurity.ServiceLocation.LifeCycles
{
	public interface ILifecycle
	{
		IObjectCache FindCache();
	}
}