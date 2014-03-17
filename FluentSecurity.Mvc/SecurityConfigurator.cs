using System;
using FluentSecurity.Diagnostics;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public static class SecurityConfigurator
	{
		private static readonly object LockObject = new object();

		internal static Guid CorrelationId { get; private set; }

		static SecurityConfigurator()
		{
			CorrelationId = Guid.NewGuid();
		}

		public static ISecurityConfiguration Configure(Action<ConfigurationExpression> configurationExpression)
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
					var configuration = new SecurityConfiguration(configurationExpression);
					SecurityConfiguration.SetConfiguration(configuration);
					return SecurityConfiguration.Current;
				}, config => "Finished configuring FluentSecurity.");
			}
		}

		public static void SetConfiguration(ISecurityConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			Reset();
			lock (LockObject)
			{
				SecurityConfiguration.SetConfiguration(configuration);
			}
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				CorrelationId = Guid.NewGuid();
				ServiceLocator.Reset();
				SecurityConfiguration.Reset();
			}
		}
	}
}