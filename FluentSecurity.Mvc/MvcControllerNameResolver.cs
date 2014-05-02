using System;
using System.Web.Mvc;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class MvcControllerNameResolver : IControllerNameResolver<AuthorizationContext>
	{
		public virtual string Resolve(AuthorizationContext context)
		{
			return Resolve(context.ActionDescriptor.ControllerDescriptor.ControllerType);
		}

		public virtual string Resolve(Type controllerType)
		{
			return controllerType.FullName;
		}
	}
}