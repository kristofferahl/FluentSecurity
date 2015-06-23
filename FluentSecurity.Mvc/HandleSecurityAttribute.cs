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
		internal IControllerNameResolver<AuthorizationContext> ControllerNameResolver { get; private set; }
		internal IActionNameResolver<AuthorizationContext> ActionNameResolver { get; private set; }

		public HandleSecurityAttribute() : this(
			SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<ISecurityHandler<ActionResult>>(),
			SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<IControllerNameResolver<AuthorizationContext>>(),
			SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<IActionNameResolver<AuthorizationContext>>()
			) {}

		public HandleSecurityAttribute(ISecurityHandler<ActionResult> securityHandler, IControllerNameResolver<AuthorizationContext> controllerNameResolver, IActionNameResolver<AuthorizationContext> actionNameResolver)
		{
			Handler = securityHandler;
			ControllerNameResolver = controllerNameResolver;
			ActionNameResolver = actionNameResolver;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
			var controllerName = ControllerNameResolver.Resolve(filterContext);
			var actionName = ActionNameResolver.Resolve(filterContext);

			var securityContext = SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.RouteValues = filterContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) filterContext.Result = overrideResult;
		}
	}
}