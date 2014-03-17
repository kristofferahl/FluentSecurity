using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSecurity.Configuration;
using FluentSecurity.Core;
using FluentSecurity.Policy;
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
			dynamic data = new ExpandoObject();
			data.RouteValues = new RouteValueDictionary();
			var context = new Mock<ISecurityContext>();
			context.Setup(x => x.Runtime).Returns(CreateSecurityRuntime());
			context.Setup(x => x.Data).Returns(data as ExpandoObject);
			context.Setup(x => x.CurrentUserIsAuthenticated()).Returns(authenticated);
			context.Setup(x => x.CurrentUserRoles()).Returns(roles);
			return context.Object;
		}

		public static ISecurityRuntime CreateSecurityRuntime()
		{
			return new SecurityRuntime();
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

		public static SecurityConfiguration<MvcConfiguration> CreateValidSecurityConfiguration()
		{
			return CreateValidSecurityConfiguration(expression => {});
		}

		public static SecurityConfiguration<MvcConfiguration> CreateValidSecurityConfiguration(Action<ConfigurationExpression> modifyer)
		{
			Action<ConfigurationExpression> configurationExpression = configuration =>
			{
				if (modifyer != null) modifyer.Invoke(configuration);
			};
			return new SecurityConfiguration<MvcConfiguration>(configurationExpression);
		}

		public static ConfigurationExpression CreateValidConfigurationExpression()
		{
			var configurationExpression = new MvcConfiguration();
			configurationExpression.GetAuthenticationStatusFrom(ValidIsAuthenticatedFunction);
			return configurationExpression;
		}

		public static ViolationConfiguration CreatedValidViolationConfiguration(List<IConvention> conventions = null)
		{
			if (conventions == null) conventions = new List<IConvention>();
			return new ViolationConfiguration(new ConventionConfiguration(conventions));
		}

		public static DefaultPolicyAppender CreateValidPolicyAppender()
		{
			return new DefaultPolicyAppender();
		}

		public static PolicyResult CreatePolicyResultFailure()
		{
			return PolicyResult.CreateFailureResult(new DenyAnonymousAccessPolicy(), "Access denied");
		}

		public static PolicyViolationException CreateExceptionFor(ISecurityPolicy policy)
		{
			return CreatePolicyViolationException(PolicyResult.CreateFailureResult(policy, "Access denied"));
		}

		public static PolicyViolationException CreatePolicyViolationException(PolicyResult policyResult, ISecurityContext securityContext = null)
		{
			return new PolicyViolationException(policyResult, securityContext ?? CreateSecurityContext(false));
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
				new DefaultPolicyViolationHandler(),
				new DenyAnonymousAccessPolicyViolationHandler(viewResult),
				new DenyAuthenticatedAccessPolicyViolationHandler(redirectResult)
			};

			return violationHandlers;
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