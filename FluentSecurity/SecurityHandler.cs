using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace FluentSecurity
{
	public class SecurityHandler : ISecurityHandler
	{
		public ActionResult HandleSecurityFor(string controllerName, string actionName)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty", "actionName");

			var configuration = SecurityConfigurator.CurrentConfiguration;
			
			var policyContainer = configuration.PolicyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				var results = policyContainer.EnforcePolicies();
				if (results.Any(x => x.ViolationOccured))
				{
					var result = results.First(x => x.ViolationOccured);
					if (configuration.ServiceLocator != null)
					{
						var violationHandlers = configuration.ServiceLocator(typeof(IPolicyViolationHandler)).Cast<IPolicyViolationHandler>();
						if (violationHandlers.Any())
						{
							// TODO: Refactor and resolve selector from container.
							var violationHandlerSelector = new PolicyViolationHandlerSelector(configuration, violationHandlers);
							var exception = new PolicyViolationException(result.Policy.GetType(), result.Message);
							var matchingHandler = violationHandlerSelector.FindHandlerFor(exception);
							if (matchingHandler != null) return matchingHandler.Handle(exception);
						}
					}
					throw new PolicyViolationException(result.Policy.GetType(), result.Message);
				}
				return null;
			}

			if (configuration.IgnoreMissingConfiguration)
				return null;

			throw new ConfigurationErrorsException("Security has not been configured for controller {0}, action {1}".FormatWith(controllerName, actionName));
		}
	}
}