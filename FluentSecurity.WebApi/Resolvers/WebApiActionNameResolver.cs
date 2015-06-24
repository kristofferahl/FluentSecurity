using System.Linq.Expressions;
using System.Reflection;
using System.Web.Http.Controllers;
using FluentSecurity.Core;
using FluentSecurity.Core.Internals;

namespace FluentSecurity.WebApi.Resolvers
{
	public class WebApiActionNameResolver : IActionNameResolver<HttpActionContext>
	{
		public virtual string Resolve(HttpActionContext actionContext)
		{
			return actionContext.ActionDescriptor.ActionName;
		}

		public virtual string Resolve(LambdaExpression actionExpression)
		{
			var actionMethod = actionExpression.GetActionMethodInfo();
			return Resolve(actionMethod);
		}

		public virtual string Resolve(MethodInfo actionMethod)
		{
			// TODO: Ensure we take any action name related attributes into account
			return actionMethod.Name;
		}
	}
}