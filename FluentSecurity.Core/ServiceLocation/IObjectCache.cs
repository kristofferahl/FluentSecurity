namespace FluentSecurity.ServiceLocation.LifeCycles
{
	public interface IObjectCache
	{
		object Get(object key);
		void Add(object key, object instance);
		void Set(object key, object instance);
		void Clear();
	}
}