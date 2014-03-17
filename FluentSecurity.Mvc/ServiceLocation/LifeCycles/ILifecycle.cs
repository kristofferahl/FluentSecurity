namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal interface ILifecycle
	{
		IObjectCache FindCache();
	}
}