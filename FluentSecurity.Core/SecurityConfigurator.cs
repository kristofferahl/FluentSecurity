using System;
using FluentSecurity.Core;
using FluentSecurity.Diagnostics;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public static class SecurityConfigurator
	{
		private static readonly object LockObject = new object();

		public static Guid CorrelationId { get; private set; }

		static SecurityConfigurator()
		{
			CorrelationId = Guid.NewGuid();
		}

		public static ISecurityConfiguration Configure<TConfiguration>(Action<TConfiguration> configurationExpression) where TConfiguration : class, IFluentConfiguration, new()
		{
			if (configurationExpression == null)
				throw new ArgumentNullException("configurationExpression");

			Reset();

			if (SecurityDoctor.Current.ScanForEventListenersOnConfigure)
				SecurityDoctor.Current.ScanForEventListeners();

			Publish.ConfigurationEvent(() => "Configuring FluentSecurity.");

			lock (LockObject)
			{
				return Publish.ConfigurationEvent(() =>
				{
					var securityConfiguration = new SecurityConfiguration<TConfiguration>(configurationExpression);
					SetConfiguration<TConfiguration>(securityConfiguration);
					return securityConfiguration;
				}, config => "Finished configuring FluentSecurity.");
			}
		}

		public static void SetConfiguration<TConfiguration>(ISecurityConfiguration configuration) where TConfiguration : class, IFluentConfiguration
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			Reset();

			SecurityConfiguration.SetConfiguration<TConfiguration>(configuration);
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				ServiceLocator.Reset();
				SecurityConfiguration.Reset();
				CorrelationId = Guid.NewGuid();
			}
		}
	}
}