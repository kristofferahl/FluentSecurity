namespace FluentSecurity.ServiceLocation.LifeCycles
{
	internal interface IObjectCache
	{
		object Get(object key);
		void Set(object key, object instance);
		void Clear();
	}
}