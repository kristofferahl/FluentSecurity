using System;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class SecurityHandler : ISecurityHandler
	{
		public ActionResult HandleSecurityFor(string areaName, string controllerName, string actionName)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty", "actionName");

			var configuration = ServiceLocator.Current.Resolve<ISecurityConfiguration>();
			
			var policyContainer = configuration.PolicyContainers.GetContainerFor(areaName, controllerName, actionName);
			if (policyContainer != null)
			{
				var context = ServiceLocator.Current.Resolve<ISecurityContext>();
				var results = policyContainer.EnforcePolicies(context);
				if (results.Any(x => x.ViolationOccured))
				{
					var result = results.First(x => x.ViolationOccured);
					var policyViolationException = new PolicyViolationException(result.Policy.GetType(), result.Message);
					var violationHandlerSelector = ServiceLocator.Current.Resolve<IPolicyViolationHandlerSelector>();
					var matchingHandler = violationHandlerSelector.FindHandlerFor(policyViolationException) ?? new ExceptionPolicyViolationHandler();
					return matchingHandler.Handle(policyViolationException);
				}
				return null;
			}

			if (configuration.IgnoreMissingConfiguration)
				return null;

			throw new ConfigurationErrorsException("Security has not been configured for area {0}, controller {1}, action {2}".FormatWith(areaName, controllerName, actionName));
		}
	}
}