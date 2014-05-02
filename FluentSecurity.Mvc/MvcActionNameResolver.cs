using System;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using FluentSecurity.Core;
using FluentSecurity.Core.Internals;

namespace FluentSecurity
{
	public class MvcActionNameResolver : IActionNameResolver<AuthorizationContext>
	{
		private static readonly Type ActionNameAttributeType = typeof(ActionNameAttribute);

		public virtual string Resolve(AuthorizationContext context)
		{
			return context.ActionDescriptor.ActionName;
		}

		public virtual string Resolve(LambdaExpression actionExpression)
		{
			var actionMethod = actionExpression.GetActionMethodInfo();
			return Resolve(actionMethod);
		}

		public virtual string Resolve(MethodInfo actionMethod)
		{
			if (Attribute.IsDefined(actionMethod, ActionNameAttributeType))
			{
				var actionNameAttribute = (ActionNameAttribute) Attribute.GetCustomAttribute(actionMethod, ActionNameAttributeType);
				return actionNameAttribute.Name;
			}
			return actionMethod.Name;
		}
	}
}