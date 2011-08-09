using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Helpers
{
	public static class UrlExtensions
	{
		public static string Action<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			return urlHelper.Action(actionExpression, null);
		}

		public static string Action<TController>(this UrlHelper urlHelper, Expression<Func<TController, object>> actionExpression, object values) where TController : Controller
		{
		    var controllerType = typeof (TController);
            var areaName = controllerType.GetAreaName();
            var controllerName = controllerType.GetControllerName();
			var actionName = actionExpression.GetActionName();

			if (SecurityHelper.ActionIsAllowedForUser(areaName, controllerName, actionName) == false)
			{
				return string.Empty;
			}
			
			return urlHelper.Action(actionName, controllerName, values);
		}
	}
}