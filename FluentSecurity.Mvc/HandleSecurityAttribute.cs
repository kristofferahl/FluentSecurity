using System;
using System.Web.Mvc;
using FluentSecurity.Configuration;
using FluentSecurity.Core;

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
			var controllerNameResolver = SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<IControllerNameResolver<AuthorizationContext>>();
			var actionNameResolver = SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<IActionNameResolver<AuthorizationContext>>();

			var controllerName = controllerNameResolver.Resolve(filterContext);
			var actionName = actionNameResolver.Resolve(filterContext);

			var securityContext = SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.RouteValues = filterContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}