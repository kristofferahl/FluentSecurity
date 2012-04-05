using System;
using System.Web.Mvc;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : ActionFilterAttribute
	{
		internal ISecurityContext Context { get; private set; }
		internal ISecurityHandler Handler { get; private set; }

		public HandleSecurityAttribute() : this(ServiceLocator.Current.Resolve<ISecurityHandler>(), ServiceLocator.Current.Resolve<ISecurityContext>()) {}

		public HandleSecurityAttribute(ISecurityHandler securityHandler, ISecurityContext securityContext)
		{
			Handler = securityHandler;
			Context = securityContext;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

			Context.Data.RouteValues = filterContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, Context);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}