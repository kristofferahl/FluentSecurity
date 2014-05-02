using System;
using System.Web.Http.Controllers;
using FluentSecurity.Core;

namespace FluentSecurity.WebApi
{
	public class WebApiControllerNameResolver : IControllerNameResolver<HttpActionContext>
	{
		public string Resolve(HttpActionContext actionContext)
		{
			return Resolve(actionContext.ActionDescriptor.ControllerDescriptor.ControllerType);
		}

		public string Resolve(Type controllerType)
		{
			return controllerType.FullName;
		}
	}
}