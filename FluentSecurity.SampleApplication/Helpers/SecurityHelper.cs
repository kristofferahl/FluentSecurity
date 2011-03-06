using System.Collections.Generic;
using System.Linq;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class SecurityHelper
	{
		public static bool ActionIsAllowedForUser(string controllerName, string actionName)
		{
			var policyContainer = SecurityConfigurator.CurrentConfiguration.PolicyContainers.GetContainerFor(controllerName, actionName);
			if (policyContainer != null)
			{
				var results = policyContainer.EnforcePolicies();
				return results.All(x => x.ViolationOccured == false);
			}
			return true;
		}

		public static bool UserIsAuthenticated()
		{
			var currentUser = SessionContext.Current.User;
			return currentUser != null;
		}

		public static object[] UserRoles()
		{
			var currentUser = SessionContext.Current.User;
			
			if (currentUser != null)
			{
				return currentUser.Roles.ToObjectArray();
			}
			
			return null;
		}

		private static object[] ToObjectArray(this IEnumerable<UserRole> collection)
		{
			var objectArray = new object[collection.Count()];
			var index = 0;
			foreach (var o in collection)
			{
				objectArray[index] = o;
				index++;
			}
			return objectArray;
		}
	}
}