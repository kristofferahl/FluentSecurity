using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http;
using FluentSecurity.Internals;

namespace FluentSecurity.WebApi
{
	public static class Extensions
	{
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
			//if (Attribute.IsDefined(actionMethod, ActionNameAttributeType))
			//{
			//	var actionNameAttribute = (ActionNameAttribute)Attribute.GetCustomAttribute(actionMethod, ActionNameAttributeType);
			//	return actionNameAttribute.Name;
			//}
			return actionMethod.Name;
		}

		//private static readonly Type ActionNameAttributeType = typeof(ActionNameAttribute);


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
				.Where(action => actionFilter.Invoke(new ControllerActionInfo(controllerType, action.GetActionName(), action.ReturnType)))
				.ToList();
		}

		internal static bool IsValidActionMethod(this MethodInfo methodInfo)
		{
			return
				methodInfo.ReturnType.IsControllerActionReturnType() &&
				!methodInfo.IsSpecialName &&
				!methodInfo.IsDeclaredBy<ApiController>();
		}

		/// <summary>
		/// Returns true if the passed method is declared by the type T.
		/// </summary>
		/// <param name="methodInfo"></param>
		/// <returns>A boolean</returns>
		internal static bool IsDeclaredBy<T>(this MethodInfo methodInfo)
		{
			var passedType = typeof(T);
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
			return true;
			//(
			//	typeof(ActionResult).IsAssignableFrom(returnType) ||
			//	typeof(Task<ActionResult>).IsAssignableFromGenericType(returnType) ||
			//	typeof(void).IsAssignableFrom(returnType)
			//);
		}
	}
}