using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Configuration
{
	public class MvcConfiguration : ConfigurationExpression, IFluentConfiguration
	{
		private readonly SecurityRuntime _runtime;
		private readonly IContainer _container;

		public MvcConfiguration()
		{
			_container = new Container(new MvcLifecycleResolver());
			new MvcRegistry().Configure(_container);

			_runtime = new SecurityRuntime(_container);

			Initialize(_runtime);
		}

		ISecurityRuntime IFluentConfiguration.GetRuntime()
		{
			return _runtime;
		}

		IContainer IFluentConfiguration.GetContainer()
		{
			return _container;
		}
	}
}