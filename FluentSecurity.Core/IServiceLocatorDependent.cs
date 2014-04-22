using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Core
{
	public interface IServiceLocatorDependent
	{
		void Inject(IServiceLocator serviceLocator);
	}
}