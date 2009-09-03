using System;
using System.Web.Mvc;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : ActionFilterAttribute
	{
		private readonly ISecurityHandler _securityHandler;

		public HandleSecurityAttribute() : this(new SecurityHandler())
		{
		}

		public HandleSecurityAttribute(ISecurityHandler securityHandler)
		{
			_securityHandler = securityHandler;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;

			_securityHandler.HandleSecurityFor(controllerName, actionName);
		}
	}
}