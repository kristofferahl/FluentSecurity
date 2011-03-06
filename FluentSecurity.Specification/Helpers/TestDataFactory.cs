using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FluentSecurity.Specification.TestData;

namespace FluentSecurity.Specification.Helpers
{
	public static class TestDataFactory
	{
		public const string ValidControllerName = "SomeController";
		public const string ValidActionName = "SomeAction";

		public static readonly Func<bool> ValidIsAuthenticatedFunction = () => true;
		public static readonly Func<object[]> ValidRolesFunction = () => new object[0];

		public static ISecurityContext CreateSecurityContext(bool authenticated, object[] roles = null)
		{
			return new SecurityContext(() => authenticated, () => roles);
		}

		public static PolicyContainer CreateValidPolicyContainer()
		{
			return new PolicyContainer(
				ValidControllerName,
				ValidActionName,
				ValidIsAuthenticatedFunction,
				ValidRolesFunction,
				CreateValidPolicyAppender());
		}

		public static PolicyContainer CreateValidPolicyContainer(string controllerName, string actionName)
		{
			return new PolicyContainer(
				controllerName ?? ValidControllerName,
				actionName ?? ValidActionName,
				ValidIsAuthenticatedFunction,
				ValidRolesFunction,
				CreateValidPolicyAppender());
		}

		public static SecurityConfiguration CreateValidSecurityConfiguration()
		{
			var builder = new SecurityConfiguration();
			return builder;
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