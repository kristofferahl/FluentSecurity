using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Mvc;
using FluentSecurity.Core;
using FluentSecurity.Core.Internals;
using FluentSecurity.Internals;

namespace FluentSecurity
{
	public class MvcActionResolver : IActionResolver
	{
		private readonly IActionNameResolver _actionNameResolver;

		public MvcActionResolver(IActionNameResolver actionNameResolver)
		{
			_actionNameResolver = actionNameResolver;
		}

		public IEnumerable<MethodInfo> ActionMethods(Type controllerType, Func<ControllerActionInfo, bool> actionFilter)
		{
			if (actionFilter == null) actionFilter = info => true;

			return controllerType
				.GetMethods(
					BindingFlags.Public |
					BindingFlags.Instance
				)
				.Where(IsValidActionMethod)
				.Where(action => actionFilter.Invoke(new ControllerActionInfo(controllerType, _actionNameResolver.Resolve(action), action.ReturnType)))
				.ToList();
		}

		private static bool IsValidActionMethod(MethodInfo methodInfo)
		{
			return
				IsControllerActionReturnType(methodInfo.ReturnType) &&
				!methodInfo.IsSpecialName &&
				!methodInfo.IsDeclaredBy<Controller>() &&
				!methodInfo.HasAttribute<NonActionAttribute>();
		}

		private static bool IsControllerActionReturnType(Type returnType)
		{
			return
			(
				typeof(ActionResult).IsAssignableFrom(returnType) ||
				typeof(Task<ActionResult>).IsAssignableFromGenericType(returnType) ||
				typeof(void).IsAssignableFrom(returnType)
			);
		}
	}
}