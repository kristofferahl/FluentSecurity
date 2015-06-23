using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using FluentSecurity.Core;
using FluentSecurity.WebApi.Configuration;

namespace FluentSecurity.WebApi
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public class HandleSecurityAttribute : AuthorizationFilterAttribute
	{
		internal ISecurityHandler<object> Handler { get; private set; }
		internal IControllerNameResolver<HttpActionContext> ControllerNameResolver { get; private set; }
		internal IActionNameResolver<HttpActionContext> ActionNameResolver { get; private set; }

		public HandleSecurityAttribute() : this(
			SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<ISecurityHandler<object>>(),
			SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<IControllerNameResolver<HttpActionContext>>(),
			SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<IActionNameResolver<HttpActionContext>>()
			) {}

		public HandleSecurityAttribute(ISecurityHandler<object> securityHandler, IControllerNameResolver<HttpActionContext> controllerNameResolver, IActionNameResolver<HttpActionContext> actionNameResolver)
		{
			Handler = securityHandler;
			ControllerNameResolver = controllerNameResolver;
			ActionNameResolver = actionNameResolver;
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			var controllerName = ControllerNameResolver.Resolve(actionContext);
			var actionName = ActionNameResolver.Resolve(actionContext);

			var securityContext = SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.ActionContext = actionContext;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null)
			{
				var httpResponseMessage = overrideResult as HttpResponseMessage;
				if (httpResponseMessage != null)
				{
					actionContext.Response = httpResponseMessage;
				}
				else
				{
					actionContext.Response.Content = new ObjectContent(overrideResult.GetType(), overrideResult, new JsonMediaTypeFormatter());
				}
			}
		}
	}
}