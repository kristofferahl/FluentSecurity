using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.TestHelper
{
	public static class PolicyContainerExtensions
	{
		public static PolicyExpectations Expect<TController>(this IEnumerable<IPolicyContainer> policyContainers) where TController : IController
		{
			if (policyContainers == null) throw new ArgumentNullException("policyContainers", "Expected IEnumerable of IPolicyContainer but was null!");

			var expectations = new PolicyExpectations(policyContainers);

			var controllerName = typeof(TController).GetControllerName();
			var actions = typeof(TController).GetActionMethods();
			foreach (var action in actions)
			{
				var actionName = action.Name;
				expectations.For(controllerName, actionName);
			}

			return expectations;
		}

		public static PolicyExpectations Expect<TController>(this IEnumerable<IPolicyContainer> policyContainers, Expression<Func<TController, ActionResult>> actionExpression) where TController : IController
		{
			if (policyContainers == null) throw new ArgumentNullException("policyContainers", "Expected IEnumerable of IPolicyContainer but was null!");

			var controllerName = typeof(TController).GetControllerName();
			var actionName = actionExpression.GetActionName();

			return new PolicyExpectations(policyContainers).For(controllerName, actionName);
		}
	}
}