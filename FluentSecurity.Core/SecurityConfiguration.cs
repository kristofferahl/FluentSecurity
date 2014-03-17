using System;
using System.Collections.Generic;
using FluentSecurity.Core;
using FluentSecurity.Diagnostics;

namespace FluentSecurity
{
	public static class SecurityConfiguration
	{
		private static readonly object LockObject = new object();
		private static volatile ISecurityConfiguration _configuration;

		public static ISecurityConfiguration Current
		{
			get
			{
				EnsureConfigured();
				return _configuration;
			}
		}

		internal static void SetConfiguration(ISecurityConfiguration configuration)
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			lock (LockObject)
			{
				_configuration = configuration;
			}
		}

		internal static void Reset()
		{
			lock (LockObject)
			{
				_configuration = null;
			}
		}

		private static void EnsureConfigured()
		{
			if (_configuration == null) throw new InvalidOperationException("Security has not been configured!");
		}
	}

	public class SecurityConfiguration<TConfiguration> : ISecurityConfiguration where TConfiguration : IFluentConfiguration, new()
	{
		public SecurityConfiguration(Action<TConfiguration> configurationExpression)
		{
			if (configurationExpression == null)
				throw new ArgumentNullException("configurationExpression");

			var configuration = new TConfiguration();
			configurationExpression.Invoke(configuration);

			Runtime = configuration.GetRuntime();
		}

		public ISecurityRuntime Runtime { get; private set; }
		public IEnumerable<IPolicyContainer> PolicyContainers { get { return Runtime.PolicyContainers; } }

		public string WhatDoIHave()
		{
			return ServiceLocation.ServiceLocator.Current.Resolve<IWhatDoIHaveBuilder>().WhatDoIHave(this);
		}
	}
}