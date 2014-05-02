using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using FluentSecurity.Core;
using FluentSecurity.Core.Internals;
using FluentSecurity.Internals;

namespace FluentSecurity.WebApi
{
	public class WebApiActionResolver : IActionResolver
	{
		private readonly IActionNameResolver _actionNameResolver;

		public WebApiActionResolver(IActionNameResolver actionNameResolver)
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
				!methodInfo.IsDeclaredBy<ApiController>();
		}

		private static bool IsControllerActionReturnType(Type returnType)
		{
			// TODO: Ensure the controller return type is valid
			return true;
		}
	}
}