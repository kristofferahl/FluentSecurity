using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Ensures we are working with a list of T
		/// </summary>
		internal static IList<T> EnsureIsList<T>(this IEnumerable<T> items)
		{
			return items == null
				? new List<T>()
				: (items as IList<T> ?? items.ToList());
		}

		/// <summary>
		/// Gets a policycontainer matching the specified controller and actioname
		/// </summary>
		/// <param name="policyContainers">Policycontainers</param>
		/// <param name="controllerName">The controllername</param>
		/// <param name="actionName">The actionname</param>
		/// <returns>A policycontainer</returns>
		public static IPolicyContainer GetContainerFor(this IEnumerable<IPolicyContainer> policyContainers, string controllerName, string actionName)
		{
			return policyContainers.SingleOrDefault(x => x.ControllerName.ToLowerInvariant() == controllerName.ToLowerInvariant() && x.ActionName.ToLowerInvariant() == actionName.ToLowerInvariant());
		}
	}
}