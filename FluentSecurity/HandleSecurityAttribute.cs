using System;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : ActionFilterAttribute
	{
		public ISecurityHandler SecurityHandler { get; private set; }

		public HandleSecurityAttribute() : this(ServiceLocator.Current.Resolve<ISecurityHandler>()) {}

		public HandleSecurityAttribute(ISecurityHandler securityHandler)
		{
			SecurityHandler = securityHandler;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
		    var areaName = filterContext.RouteData.GetAreaName();

			var overrideResult = SecurityHandler.HandleSecurityFor(areaName, controllerName, actionName);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}