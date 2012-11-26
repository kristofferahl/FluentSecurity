using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class SecurityHelper
	{
		public static bool ActionIsAllowedForUser(string controllerName, string actionName)
		{
			var configuration = SecurityConfiguration.Current; 
			var policyContainer = configuration.PolicyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				var context = SecurityContext.Current;
				var results = policyContainer.EnforcePolicies(context);
				return results.All(x => x.ViolationOccured == false);
			}
			return true;
		}

		public static bool UserIsAuthenticated()
		{
			var currentUser = SessionContext.Current.User;
			return currentUser != null;
		}

		public static IEnumerable<object> UserRoles()
		{
			var currentUser = SessionContext.Current.User;
			return currentUser != null ? currentUser.Roles.Cast<object>().ToArray() : null;
		}
	}
}