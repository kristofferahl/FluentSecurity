using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class Extensions
	{
	    /// <summary>
	    /// Gets a policycontainer matching the specified area,controller and actionname
	    /// </summary>
	    /// <param name="policyContainers">Policycontainers</param>
	    /// <param name="areaName">The areaname</param>
	    /// <param name="controllerName">The controllername</param>
	    /// <param name="actionName">The actionname</param>
	    /// <returns>A policycontainer</returns>
	    public static IPolicyContainer GetContainerFor(this IEnumerable<IPolicyContainer> policyContainers, string areaName, string controllerName, string actionName)
		{
			return policyContainers
				.Where(x => x.AreaName.ToLower() == areaName.ToLower() && 
                       x.ControllerName.ToLower() == controllerName.ToLower() && 
                       x.ActionName.ToLower() == actionName.ToLower())
				.SingleOrDefault();
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="routeData"></param>
        /// <returns></returns>
        public static string GetAreaName(this RouteData routeData)
        {
            object obj2;
            if (routeData.DataTokens.TryGetValue("area", out obj2))
            {
                return (obj2 as string);
            }
            return GetAreaName(routeData.Route);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="route"></param>
        /// <returns></returns>
        public static string GetAreaName(RouteBase route)
        {
            var area = route as IRouteWithArea;
            if (area != null)
            {
                return area.Area;
            }
            var route2 = route as Route;
            if ((route2 != null) && (route2.DataTokens != null))
            {
                return (route2.DataTokens["area"] as string) ?? string.Empty;
            }
            return string.Empty;
        }

		///<summary>
		/// Gets the area name for the specified controller type
		///</summary>
		public static string GetAreaName(this Type controllerType)
		{
		    var moduleName = controllerType.Module.Name;
            var nameSpaceArr = controllerType.Namespace.Replace(moduleName.Substring(0, moduleName.Length - 3), "").Split('.');
            // Areas.Area.Controllers.Controller
            return nameSpaceArr.Length == 4 ? nameSpaceArr[1] : string.Empty;
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
		public static IEnumerable<MethodInfo> GetActionMethods(this Type controllerType)
		{
			return controllerType
				.GetMethods(
					BindingFlags.Public |
					BindingFlags.Instance |
					BindingFlags.DeclaredOnly
				)
				.Where(x => typeof(ActionResult).IsAssignableFrom(x.ReturnType))
				.AsEnumerable();
		}

		///<summary>
		/// Gets the action name for the specified action expression
		///</summary>
		public static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.Name;
		}
		
		/// <summary>
		/// Performs an action on each item
		/// </summary>
		public static void Each<T>(this IEnumerable<T> items, Action<T> action)
		{
			foreach (var item in items)
				action(item);
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