using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Specification.TestData;
using Moq;

namespace FluentSecurity.Specification.Helpers
{
	public static class TestDataFactory
	{
		public const string ValidControllerName = "SomeController";
		public const string ValidActionName = "SomeAction";

		public static readonly Func<bool> ValidIsAuthenticatedFunction = () => true;
		public static readonly Func<IEnumerable<object>> ValidRolesFunction = () => new object[0];

		public static ISecurityContext CreateSecurityContext(bool authenticated, IEnumerable<object> roles = null)
		{
			var context = new Mock<ISecurityContext>();
			context.Setup(x => x.CurrenUserAuthenticated()).Returns(authenticated);
			context.Setup(x => x.CurrenUserRoles()).Returns(roles);
			return context.Object;
		}

		public static PolicyContainer CreateValidPolicyContainer()
		{
			return new PolicyContainer(
				ValidControllerName,
				ValidActionName,
				CreateValidPolicyAppender());
		}

		public static PolicyContainer CreateValidPolicyContainer(string controllerName, string actionName)
		{
			return new PolicyContainer(
				controllerName ?? ValidControllerName,
				actionName ?? ValidActionName,
				CreateValidPolicyAppender());
		}

		public static SecurityConfiguration CreateValidSecurityConfiguration()
		{
			return new SecurityConfiguration(configuration => {});
		}

		public static ConfigurationExpression CreateValidConfigurationExpression()
		{
			var configurationExpression = new ConfigurationExpression();
			configurationExpression.GetAuthenticationStatusFrom(ValidIsAuthenticatedFunction);
			return configurationExpression;
		}

		public static DefaultPolicyAppender CreateValidPolicyAppender()
		{
			return new DefaultPolicyAppender();
		}

		public static IPolicyAppender CreateFakePolicyAppender()
		{
			return new FakePolicyAppender();
		}

		public static IEnumerable<IPolicyViolationHandler> CreatePolicyViolationHandlers()
		{
			var viewResult = new ViewResult { ViewName = "SomeViewName" };
			var redirectResult = new RedirectResult("http://localhost/");
			
			var violationHandlers = new List<IPolicyViolationHandler>
			{
				new DenyAnonymousAccessPolicyViolationHandler(viewResult),
				new DenyAuthenticatedAccessPolicyViolationHandler(redirectResult)
			};

			return violationHandlers;
		}

		public static IRequestDescription CreateRequestDescription(string areName = "Area")
		{
			var mock = new Mock<IRequestDescription>();
			mock.Setup(x => x.AreName).Returns(areName);
			mock.Setup(x => x.ControllerName).Returns("Controller");
			mock.Setup(x => x.ActionName).Returns("Action");
			return mock.Object;
		}

		public static Route CreateRoute(string areaName, string controllerName, string actionName)
		{
			var routeValueDictionary = new RouteValueDictionary
			{
				{ "controller", controllerName },
				{ "action", actionName }
			};

			var route = new Route("some-url", routeValueDictionary, new MvcRouteHandler());
			if (!String.IsNullOrEmpty(areaName))
				route.DataTokens = new RouteValueDictionary { { "area", areaName } };

			return route;
		}
	}
}