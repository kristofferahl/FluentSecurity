using System;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public static class SecurityConfigurator
	{
		private static readonly object LockObject = new object();

		public static ISecurityConfiguration Configure(Action<ConfigurationExpression> configurationExpression)
		{
			if (configurationExpression == null)
				throw new ArgumentNullException("configurationExpression");

			Reset();
			lock (LockObject)
			{
				var configuration = new SecurityConfiguration(configurationExpression);
				SecurityConfiguration.SetConfiguration(configuration);
				return SecurityConfiguration.Current;
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
				ServiceLocator.Reset();
				SecurityConfiguration.Reset();
			}
		}
	}
}