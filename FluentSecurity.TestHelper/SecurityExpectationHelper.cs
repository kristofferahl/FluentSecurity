using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.TestHelper
{
	public abstract class SecurityExpectationHelper<TController> : SecurityExpectationHelper where TController : IController
	{
		protected PolicyExpectations Expect(Expression<Func<TController, ActionResult>> actionExpression)
		{
			return Expect<TController>(actionExpression);
		}

		protected PolicyExpectations Expect()
		{
			return Expect<TController>();
		}
	}

	public abstract class SecurityExpectationHelper
	{
		protected abstract ISecurityConfiguration ConfigurationToTest();

		protected PolicyExpectations Expect<TController>(Expression<Func<TController, ActionResult>> actionExpression) where TController : IController
		{
			var configurationToTest = ConfigurationToTest();
			if (configurationToTest == null) throw new AssertionException("The configuration to test must not be null!");
			var policyContainers = configurationToTest.PolicyContainers;
			if (policyContainers == null) throw new AssertionException("The property PolicyContainers must not be null!");
			return policyContainers.Expect(actionExpression);
		}

		protected PolicyExpectations Expect<TController>() where TController : IController
		{
			var configurationToTest = ConfigurationToTest();
			if (configurationToTest == null) throw new AssertionException("The configuration to test must not be null!");
			var policyContainers = configurationToTest.PolicyContainers;
			if (policyContainers == null) throw new AssertionException("The property PolicyContainers must not be null!");
			return policyContainers.Expect<TController>();
		}
	}
}