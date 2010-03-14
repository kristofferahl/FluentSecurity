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
				CreateValidPolicyManager());
		}

		public static PolicyContainer CreateValidPolicyContainer(string controllerName, string actionName)
		{
			return new PolicyContainer(
				controllerName ?? ValidControllerName,
				actionName ?? ValidActionName,
				ValidIsAuthenticatedFunction,
				ValidRolesFunction,
				CreateValidPolicyManager());
		}

		public static PolicyBuilder CreateValidPolicyBuilder()
		{
			var builder = new PolicyBuilder();
			builder.GetAuthenticationStatusFrom(ValidIsAuthenticatedFunction);
			return builder;
		}

		public static DefaultPolicyManager CreateValidPolicyManager()
		{
			return new DefaultPolicyManager();
		}

		public static IPolicyManager CreateFakePolicyManager()
		{
			return new FakePolicyManager();
		}
	}
}