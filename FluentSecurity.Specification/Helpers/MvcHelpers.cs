using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSecurity.Specification.Helpers
{
	public static class MvcHelpers
	{
		public static AuthorizationContext GetAuthorizationContextFor<TController>(Expression<Func<TController, object>> actionExpression) where TController : Controller
		{
			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

			var filterContext = MockRepository.GenerateMock<AuthorizationContext>();

			var controllerDescriptor = MockRepository.GenerateMock<ControllerDescriptor>();
			controllerDescriptor.Expect(x => x.ControllerName).Return(controllerName).Repeat.Any();
			controllerDescriptor.Expect(x => x.ControllerType).Return(typeof(TController)).Repeat.Any();
			controllerDescriptor.Replay();

			var actionDescriptor = MockRepository.GenerateMock<ActionDescriptor>();
			actionDescriptor.Expect(x => x.ActionName).Return(actionName).Repeat.Any();
			actionDescriptor.Expect(x => x.ControllerDescriptor).Return(controllerDescriptor).Repeat.Any();
			actionDescriptor.Replay();

			filterContext.Expect(x => x.ActionDescriptor).Return(actionDescriptor).Repeat.Any();
			filterContext.Replay();

			var routeData = new RouteData();
			filterContext.Expect(x => x.RouteData).Return(routeData).Repeat.Any();
			filterContext.Replay();

			return filterContext;
		}

		private static string GetControllerName(this Type controllerType)
		{
			return controllerType.Name.Replace("Controller", string.Empty);
		}

		private static string GetActionName(this LambdaExpression actionExpression)
		{
			return ((MethodCallExpression)actionExpression.Body).Method.GetActionName();
		}
	}
}