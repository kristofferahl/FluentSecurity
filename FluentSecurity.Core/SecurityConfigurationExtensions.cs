using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Core
{
	public static class SecurityConfigurationExtensions
	{
		public static void AssertAllActionsAreConfigured(this ISecurityConfiguration configuration, Assembly[] assemblies)
		{
			var controllerNameResolver = configuration.ServiceLocator.Resolve<IControllerNameResolver>();
			var actionNameResolver = configuration.ServiceLocator.Resolve<IActionNameResolver>();
			var actionResolver = configuration.ServiceLocator.Resolve<IActionResolver>();
			var controlleTypeScanner = configuration.ServiceLocator.Resolve<IControllerTypeScanner>();

			var assemblyScanner = new CoreAssemblyScanner();
			assemblyScanner.Assemblies(assemblies);
			assemblyScanner.With(controlleTypeScanner);

			var controllerTypes = assemblyScanner.Scan();

			var unconfiguredActions = (
				from c in controllerTypes
				from a in actionResolver.ActionMethods(c)
				let actionName = actionNameResolver.Resolve(a)
				let controllerName = controllerNameResolver.Resolve(c)
				where configuration.Runtime.PolicyContainers.GetContainerFor(controllerName, actionName) == null
				select new { ControllerName = controllerName, ActionName = actionName }
				).ToList();

			if (unconfiguredActions.Any())
			{
				var errorMessageBuilder = new StringBuilder();
				unconfiguredActions.ForEach(a =>
					errorMessageBuilder.AppendLine("- Security has not been configured for {0} action {1}.".FormatWith(a.ControllerName, a.ActionName))
				);
				throw new ConfigurationErrorsException(errorMessageBuilder.ToString());
			}
		}

		public static void AssertAllActionsAreConfigured(this ISecurityConfiguration configuration)
		{
			//As per http://bloggingabout.net/blogs/vagif/archive/2010/07/02/net-4-0-and-notsupportedexception-complaining-about-dynamic-assemblies.aspx
			var assemblies = (
				from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !(assembly is System.Reflection.Emit.AssemblyBuilder) &&
				assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder" &&
				!assembly.GlobalAssemblyCache
				select assembly
				).ToArray();

			configuration.AssertAllActionsAreConfigured(assemblies);
		}
	}
}