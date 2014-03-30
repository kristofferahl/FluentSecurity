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
			_runtime = new SecurityRuntime(new SecurityCache(_lifecycleResolver));
			Initialize(_runtime);
		}

		public ISecurityRuntime GetRuntime()
		{
			return _runtime;
		}

		public IRegistry GetRegistry()
		{
			return _registry;
		}

		public ILifecycleResolver GetLifecycleResolver()
		{
			return _lifecycleResolver;
		}
	}
}