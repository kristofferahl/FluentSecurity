using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;
using FluentSecurity.WebApi.ServiceLocation;

namespace FluentSecurity.WebApi.Configuration
{
	public class WebApiConfiguration : WebApiConfigurationExpression, IFluentConfiguration
	{
		private readonly WebApiSecurityRuntime _runtime;
		private readonly IContainer _container;

		public WebApiConfiguration()
		{
			_container = new Container(new WebApiLifecycleResolver());
			new WebApiRegistry().Configure(_container);

			_runtime = new WebApiSecurityRuntime(_container);

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