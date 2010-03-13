using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
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
			return policyContainers
				.Where(x => x.ControllerName.ToLower() == controllerName.ToLower() && x.ActionName.ToLower() == actionName.ToLower())
				.SingleOrDefault();
		}

		///<summary>
		/// Gets the controller name for the specified controller type
		///</summary>
		public static string GetControllerName(this Type controllerType)
		{
			return controllerType.Name.Replace("Controller", string.Empty);
		}

		/// <summary>
		/// Gets actionmethods for the specified controller type
		/// </summary>
		public static MethodInfo[] GetActionMethods(this Type controllerType)
		{
			return controllerType.GetMethods(
				BindingFlags.Public |
				BindingFlags.Instance |
				BindingFlags.DeclaredOnly);
		}

		///<summary>
		/// Gets the action name for the specified action expression
		///</summary>
		public static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.Name;
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
				var roles = policy.GetRoles();
				if (roles.IsNullOrEmpty())
				{
					builder.AppendFormat("\r\n\t{0}", policy.GetName());	
				}
				else
				{
					builder.AppendFormat("\r\n\t{0} ({1})", policy.GetName(), roles);
				}
			}
			return builder.ToString();
		}

		private static string GetName(this ISecurityPolicy policy)
		{
			return policy.GetType().Name.Replace("Policy", string.Empty);
		}

		private static string GetRoles(this ISecurityPolicy policy)
		{
			if (policy.RolesRequired == null || policy.RolesRequired.Length == 0)
				return string.Empty;

			var builder = new StringBuilder();
			foreach (var requiredRole in policy.RolesRequired)
			{
				builder.AppendFormat("{0} or ", requiredRole);
			}

			return builder.ToString(0, builder.Length - 4);
		}
	}
}