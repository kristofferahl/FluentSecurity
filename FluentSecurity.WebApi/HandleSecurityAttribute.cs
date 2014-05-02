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

		public HandleSecurityAttribute() : this(SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<ISecurityHandler<object>>()) { }

		public HandleSecurityAttribute(ISecurityHandler<object> securityHandler)
		{
			Handler = securityHandler;
		}

		public override void OnAuthorization(HttpActionContext actionContext)
		{
			var controllerNameResolver = SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<IControllerNameResolver<HttpActionContext>>();
			var actionNameResolver = SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<IActionNameResolver<HttpActionContext>>();

			var controllerName = controllerNameResolver.Resolve(actionContext);
			var actionName = actionNameResolver.Resolve(actionContext);

			var securityContext = SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.RouteValues = actionContext.RequestContext.RouteData.Values;
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