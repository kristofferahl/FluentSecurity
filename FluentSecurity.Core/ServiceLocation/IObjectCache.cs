namespace FluentSecurity.ServiceLocation.LifeCycles
{
	public interface IObjectCache
	{
		object Get(object key);
		void Set(object key, object instance);
		void Clear();
	}
}