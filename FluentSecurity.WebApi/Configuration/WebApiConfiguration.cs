using FluentSecurity.Caching;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;
using FluentSecurity.WebApi.ServiceLocation;

namespace FluentSecurity.WebApi.Configuration
{
	public class WebApiConfiguration : WebApiConfigurationExpression, IFluentConfiguration
	{
		private readonly WebApiLifecycleResolver _lifecycleResolver;
		private readonly WebApiRegistry _registry;
		private readonly WebApiSecurityRuntime _rutime;

		public WebApiConfiguration()
		{
			_lifecycleResolver = new WebApiLifecycleResolver();
			_registry = new WebApiRegistry();
			_rutime = new WebApiSecurityRuntime(new SecurityCache(_lifecycleResolver), new WebApiTypeFactory());

			Initialize(_rutime);
		}

		ISecurityRuntime IFluentConfiguration.GetRuntime()
		{
			return _rutime;
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