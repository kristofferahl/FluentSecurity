using System;
using FluentSecurity.Specification.TestData;

namespace FluentSecurity.Specification.Helpers
{
	public static class TestDataFactory
	{
		public const string ValidControllerName = "SomeController";
		public const string ValidActionName = "SomeAction";

		public static readonly Func<bool> ValidIsAuthenticatedFunction = () => true;
		public static readonly Func<object[]> ValidRolesFunction = () => new object[0];

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
	}
}