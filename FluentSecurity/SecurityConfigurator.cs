using System;

namespace FluentSecurity
{
	public static class SecurityConfigurator
	{
		private static readonly object LockObject;
		private static ISecurityConfiguration _configuration;

		static SecurityConfigurator()
		{
			LockObject = new object();
		}

		public static void SetConfiguration(ISecurityConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			lock (LockObject)
			{
				_configuration = configuration;
			}
		}

		public static ISecurityConfiguration CurrentConfiguration
		{
			get
			{
				if (_configuration == null)
				{
					lock (LockObject)
					{
						if (_configuration == null)
							_configuration = new SecurityConfiguration();
					}
				}
				return _configuration;
			}
		}

		public static void Configure(Action<ConfigurationExpression> configurationExpression)
		{
			CurrentConfiguration.Configure(configurationExpression);
		}

		public static string WhatDoIHave()
		{
			return CurrentConfiguration.WhatDoIHave();
		}

		public static void Reset()
		{
			lock (LockObject)
			{
				_configuration = null;
			}
		}
	}
}