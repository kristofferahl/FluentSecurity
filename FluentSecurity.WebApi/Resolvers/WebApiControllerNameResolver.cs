using System;
using System.Web.Http.Controllers;
using FluentSecurity.Core;

namespace FluentSecurity.WebApi.Resolvers
{
	public class WebApiControllerNameResolver : IControllerNameResolver<HttpActionContext>
	{
		public virtual string Resolve(HttpActionContext actionContext)
		{
			return Resolve(actionContext.ActionDescriptor.ControllerDescriptor.ControllerType);
		}

		public virtual string Resolve(Type controllerType)
		{
			return controllerType.FullName;
		}
	}
}