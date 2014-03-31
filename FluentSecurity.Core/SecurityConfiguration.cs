using System;
using System.Collections.Concurrent;
using FluentSecurity.Core;
using FluentSecurity.Diagnostics;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public static class SecurityConfiguration
	{
		private static readonly ConcurrentDictionary<string, ISecurityConfiguration> Configurations = new ConcurrentDictionary<string, ISecurityConfiguration>();

		public static ISecurityConfiguration Get<TConfiguration>() where TConfiguration : IFluentConfiguration
		{
			var key = typeof(TConfiguration).FullName;
			if (!Configurations.ContainsKey(key)) throw new InvalidOperationException("Security has not been configured!");
			return Configurations[key];
		}

		internal static void SetConfiguration<TConfiguration>(ISecurityConfiguration configuration) where TConfiguration : IFluentConfiguration
		{
			if (configuration == null)
				throw new ArgumentNullException("configuration");

			var key = typeof(TConfiguration).FullName;
			Configurations[key] = configuration;
		}

		internal static void Reset()
		{
			Configurations.Clear();
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

			ServiceLocator = new ServiceLocator(configuration);
			Runtime = configuration.GetRuntime();
		}

		public ISecurityRuntime Runtime { get; private set; }
		public IServiceLocator ServiceLocator { get; private set; }

		public ISecurityContext CreateContext()
		{
			return SecurityContext.CreateFrom(this);
		}

		// TODO: Make WhatDoIHave an extension method of ISecurityConfiguration as it is not a core feature anymore
		public string WhatDoIHave()
		{
			return ServiceLocator.Resolve<IWhatDoIHaveBuilder>().WhatDoIHave(this);
		}
	}
}