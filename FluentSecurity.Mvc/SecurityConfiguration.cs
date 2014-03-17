using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSecurity.Configuration;
using FluentSecurity.Diagnostics;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity
{
	public class SecurityConfiguration : ISecurityConfiguration
	{
		public SecurityConfiguration(Action<RootConfiguration> configurationExpression)
		{
			if (configurationExpression == null)
				throw new ArgumentNullException("configurationExpression");

			var configuration = new RootConfiguration();
			configurationExpression.Invoke(configuration);
			
			Runtime = configuration.Runtime;
			PolicyContainers = Runtime.PolicyContainers;
		}

		public ISecurityRuntime Runtime { get; private set; }
		public IEnumerable<IPolicyContainer> PolicyContainers { get; private set; }

		public string WhatDoIHave()
		{
			return ServiceLocation.ServiceLocator.Current.Resolve<IWhatDoIHaveBuilder>().WhatDoIHave(this);
		}
		
		public void AssertAllActionsAreConfigured(Assembly[] assemblies)
		{
			var assemblyScanner = new AssemblyScanner();
			assemblyScanner.Assemblies(assemblies);
			assemblyScanner.With<ControllerTypeScanner>();
			
			var controllerTypes = assemblyScanner.Scan();
			
			var unconfiguredActions = (
				from c in controllerTypes
				from a in c.GetActionMethods()
				let actionName = a.GetActionName()
				let controllerName = c.GetControllerName()
				where PolicyContainers.GetContainerFor(controllerName, actionName) == null
				select new { ControllerName = controllerName, ActionName = actionName }
				).ToList();
			
			if (unconfiguredActions.Any())
			{
				var errorMessageBuilder = new StringBuilder();
				unconfiguredActions.Each(a =>
					errorMessageBuilder.AppendLine("- Security has not been configured for {0} action {1}.".FormatWith(a.ControllerName, a.ActionName))
				);
				throw new ConfigurationErrorsException(errorMessageBuilder.ToString());
			}
		}
		
		public void AssertAllActionsAreConfigured()
		{
			//As per http://bloggingabout.net/blogs/vagif/archive/2010/07/02/net-4-0-and-notsupportedexception-complaining-about-dynamic-assemblies.aspx
			var assemblies = (
				from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !(assembly is System.Reflection.Emit.AssemblyBuilder) &&
				assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder" &&
				!assembly.GlobalAssemblyCache 
				select assembly
				).ToArray();
			
			AssertAllActionsAreConfigured(assemblies);
		}

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
}