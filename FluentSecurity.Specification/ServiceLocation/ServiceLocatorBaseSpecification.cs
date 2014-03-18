using System;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity.Specification.ServiceLocation
{
	public abstract class ServiceLocatorBaseSpecification
	{
		public IFluentConfiguration ConfigureSecurity(Action<MvcConfiguration> modifyer)
		{
			IFluentConfiguration fluentConfiguration = null;
			Action<MvcConfiguration> configurationExpression = configuration =>
				{
					fluentConfiguration = configuration;
					modifyer.Invoke(configuration);
				};
			SecurityConfigurator.Configure(configurationExpression);
			return fluentConfiguration;
		}

		public ServiceLocator CreateServiceLocator(Action<MvcConfiguration> modifyer)
		{
			var config = ConfigureSecurity(modifyer);
			return new ServiceLocator(config);
		}
	}
}