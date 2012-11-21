using System;
using System.Linq.Expressions;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class MvcExtensions
	{
		public static string GetControllerName(this Type controllerType)
		{
			return controllerType.Name.Replace("Controller", string.Empty);
		}

		public static string GetFullControllerName(this Type controllerType)
		{
			return controllerType.FullName;
		}

		public static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.GetActionName();
		}
	}
}