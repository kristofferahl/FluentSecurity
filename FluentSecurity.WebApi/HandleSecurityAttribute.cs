using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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
			var actionName = actionContext.ActionDescriptor.ActionName;
			var controllerName = actionContext.ActionDescriptor.ControllerDescriptor.ControllerType.FullName;

			var securityContext = SecurityConfiguration.Get<WebApiConfiguration>().ServiceLocator.Resolve<ISecurityContext>();
			securityContext.Data.RouteValues = actionContext.RequestContext.RouteData.Values;

			var overrideResult = Handler.HandleSecurityFor(controllerName, actionName, securityContext);
			if (overrideResult != null) actionContext.Response.Content = new ObjectContent(overrideResult.GetType(), overrideResult, new JsonMediaTypeFormatter());
		}
	}
}