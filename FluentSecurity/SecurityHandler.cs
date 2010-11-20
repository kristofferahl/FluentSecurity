using System;
using System.Configuration;

namespace FluentSecurity
{
	public class SecurityHandler : ISecurityHandler
	{
		public void HandleSecurityFor(string controllerName, string actionName)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty", "actionName");

			var configuration = FluentSecurity.CurrentConfiguration;
			
			var policyContainer = configuration.PolicyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				policyContainer.EnforcePolicies();
				return;
			}

			if (configuration.IgnoreMissingConfiguration)
				return;

			throw new ConfigurationErrorsException("Security has not been configured for controller {0}, action {1}".FormatWith(controllerName, actionName));
		}
	}
}