using System;
using System.Web.Mvc;
using FluentSecurity.Configuration;

namespace FluentSecurity
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : Attribute, IAuthorizationFilter
	{
		internal ISecurityHandler<ActionResult> Handler { get; private set; }

		public HandleSecurityAttribute() : this(SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<ISecurityHandler<ActionResult>>()) { }

		public HandleSecurityAttribute(ISecurityHandler<ActionResult> securityHandler)
		{
			Handler = securityHandler;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var actionName = filterContext.ActionDescriptor.ActionName;
			var controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

			var securityContext = SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.RouteValues = filterContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}