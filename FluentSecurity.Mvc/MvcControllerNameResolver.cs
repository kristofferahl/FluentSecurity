using System;
using System.Web.Mvc;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class MvcControllerNameResolver : IControllerNameResolver<AuthorizationContext>
	{
		public string Resolve(AuthorizationContext context)
		{
			return Resolve(context.ActionDescriptor.ControllerDescriptor.ControllerType);
		}

		public string Resolve(Type controllerType)
		{
			return controllerType.FullName;
		}
	}
}