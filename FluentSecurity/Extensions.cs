using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Caching;
using FluentSecurity.Internals;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class Extensions
	{
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

		///<summary>
		/// Gets the controller name for the specified controller type
		///</summary>
		public static string GetControllerName(this Type controllerType)
		{
			return controllerType.FullName;
		}

		///<summary>
		/// Gets the action name for the specified action expression
		///</summary>
		public static string GetActionName(this LambdaExpression actionExpression)
		{
			var expression = (MethodCallExpression)(actionExpression.Body is UnaryExpression ? ((UnaryExpression)actionExpression.Body).Operand : actionExpression.Body);
			return expression.Method.GetActionName();
		}

		/// <summary>
		/// Gets action name for the specified action method considering ActionName attribute
		/// </summary>
		public static String GetActionName(this MethodInfo actionMethod)
		{
			if (Attribute.IsDefined(actionMethod, ActionNameAttributeType))
			{
				var actionNameAttribute = (ActionNameAttribute) Attribute.GetCustomAttribute(actionMethod, ActionNameAttributeType);
				return actionNameAttribute.Name;
			}
			return actionMethod.Name;
		}

		private static readonly Type ActionNameAttributeType = typeof(ActionNameAttribute);

		/// <summary>
		/// Gets the actual type of the ISecurityPolicy. Takes care of checking for lazy policies.
		/// </summary>
		public static Type GetPolicyType(this ISecurityPolicy securityPolicy)
		{
			var lazySecurityPolicy = securityPolicy as ILazySecurityPolicy;
			return lazySecurityPolicy != null
				? lazySecurityPolicy.PolicyType
				: securityPolicy.GetType();
		}

		/// <summary>
		/// Gets actionmethods for the specified controller type
		/// </summary>
		internal static IEnumerable<MethodInfo> GetActionMethods(this Type controllerType, Func<ControllerActionInfo, bool> actionFilter = null)
		{
			if (actionFilter == null) actionFilter = info => true;
			
			return controllerType
				.GetMethods(
					BindingFlags.Public |
					BindingFlags.Instance 
				)
				.Where(IsValidActionMethod)
				.Where(action => actionFilter.Invoke(new ControllerActionInfo(controllerType, action)))
				.ToList();
		}

		internal static bool IsValidActionMethod(this MethodInfo methodInfo)
		{
			return
				methodInfo.ReturnType.IsControllerActionReturnType() &&
				!methodInfo.IsSpecialName &&
				!methodInfo.IsDeclaredBy<Controller>();
		}

		/// <summary>
		/// Returns true if the passed method is declared by the type T.
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <returns>A boolean</returns>
		internal static bool IsDeclaredBy<T>(this MethodInfo methodInfo)
		{
			var passedType = typeof (T);
			var declaringType = methodInfo.GetBaseDefinition().DeclaringType;
			return declaringType != null && declaringType.IsAssignableFrom(passedType);
		}

		/// <summary>
		/// Returns true if the type matches a controller action return type.
		/// </summary>
		/// <param name="returnType"></param>
		/// <returns>A boolean</returns>
		internal static bool IsControllerActionReturnType(this Type returnType)
		{
			return 
				(
					typeof (ActionResult).IsAssignableFrom(returnType) || 
					typeof (Task<ActionResult>).IsAssignableFromGenericType(returnType) ||
					typeof(void).IsAssignableFrom(returnType)
				);
		}

		/// <summary>
		/// Gets the area name of the route
		/// </summary>
		/// <param name="routeData">Route data</param>
		/// <returns>The name of the are</returns>
		internal static string GetAreaName(this RouteData routeData)
		{
			object value;
			if (routeData.DataTokens.TryGetValue("area", out value))
			{
				return (value as string);
			}
			return GetAreaName(routeData.Route);
		}

		/// <summary>
		/// Gets the area name of the route
		/// </summary>
		/// <param name="route">Route</param>
		/// <returns>The name of the are</returns>
		internal static string GetAreaName(this RouteBase route)
		{
			var areRoute = route as IRouteWithArea;
			if (areRoute != null)
			{
				return areRoute.Area;
			}
			var standardRoute = route as Route;
			if ((standardRoute != null) && (standardRoute.DataTokens != null))
			{
				return (standardRoute.DataTokens["area"] as string) ?? string.Empty;
			}
			return string.Empty;
		}

		/// <summary>
		/// Ensures we are working with the actual policy. Takes care of loading lazy policies.
		/// </summary>
		internal static ISecurityPolicy EnsureNonLazyPolicy(this ISecurityPolicy securityPolicy)
		{
			var lazySecurityPolicy = securityPolicy as ILazySecurityPolicy;
			return lazySecurityPolicy != null
				? lazySecurityPolicy.Load()
				: securityPolicy;
		}

		/// <summary>
		/// Ensures we are working with the expected policy type. Takes care of loading and casting lazy policies.
		/// </summary>
		internal static TSecurityPolicy EnsureNonLazyPolicyOf<TSecurityPolicy>(this ISecurityPolicy securityPolicy) where TSecurityPolicy : class, ISecurityPolicy
		{
			return securityPolicy.EnsureNonLazyPolicy() as TSecurityPolicy;
		}

		/// <summary>
		/// Returns true if the policy is of the expected type. Takes care of checking for lazy policies.
		/// </summary>
		/// <param name="securityPolicy">The policy</param>
		/// <returns>A boolean</returns>
		internal static bool IsPolicyOf<TSecurityPolicy>(this ISecurityPolicy securityPolicy) where TSecurityPolicy : class, ISecurityPolicy
		{
			var isMatch = securityPolicy is TSecurityPolicy;
			if (!isMatch) isMatch = securityPolicy.GetPolicyType() == typeof (TSecurityPolicy);
			return isMatch;
		}

		/// <summary>
		/// Returns true if the policy implements ICacheKeyProvider
		/// </summary>
		/// <param name="securityPolicy">The policy</param>
		/// <returns>A boolean</returns>
		internal static bool IsCacheKeyProvider(this ISecurityPolicy securityPolicy)
		{
			return typeof (ICacheKeyProvider).IsAssignableFrom(securityPolicy.GetPolicyType());
		}

		/// <summary>
		/// Performs an action on each item
		/// </summary>
		internal static void Each<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items)
				action(item);
		}

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
		/// Returns true if the value is null or empty
		/// </summary>
		/// <param name="value">The value</param>
		/// <returns>A boolean</returns>
		internal static bool IsNullOrEmpty(this string value)
		{
			return String.IsNullOrEmpty(value);
		}

		/// <summary>
		/// Returns a formatted string
		/// </summary>
		/// <param name="format">The format</param>
		/// <param name="values">The values</param>
		/// <returns>A formatted string</returns>
		internal static string FormatWith(this string format, params object[] values)
		{
			return string.Format(format, values);
		}

		/// <summary>
		/// Converts policies to a text
		/// </summary>
		/// <param name="policies">The policies</param>
		/// <returns>A string of policies</returns>
		internal static string ToText(this IEnumerable<ISecurityPolicy> policies)
		{
			var builder = new StringBuilder();
			foreach (var policy in policies)
			{
				builder.AppendFormat("\r\n\t{0}", policy);
			}
			return builder.ToString();
		}
	}
}