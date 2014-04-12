using FluentSecurity.Caching;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Configuration
{
	public class MvcConfiguration : ConfigurationExpression, IFluentConfiguration
	{
		private readonly MvcLifecycleResolver _lifecycleResolver;
		private readonly MvcRegistry _registry;
		private readonly SecurityRuntime _runtime;

		public MvcConfiguration()
		{
			_lifecycleResolver = new MvcLifecycleResolver();
			_registry = new MvcRegistry();
			_runtime = new SecurityRuntime(new SecurityCache(_lifecycleResolver), new MvcTypeFactory());

			Initialize(_runtime);
		}

		ISecurityRuntime IFluentConfiguration.GetRuntime()
		{
			return _runtime;
		}

		IRegistry IFluentConfiguration.GetRegistry()
		{
			return _registry;
		}

		ILifecycleResolver IFluentConfiguration.GetLifecycleResolver()
		{
			return _lifecycleResolver;
		}
	}
}