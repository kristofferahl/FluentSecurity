using System;
using System.Collections.Generic;
using System.Web.Mvc;
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
	}
}