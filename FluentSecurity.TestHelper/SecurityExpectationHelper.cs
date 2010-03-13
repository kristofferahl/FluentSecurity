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
		protected IEnumerable<IPolicyContainer> PolicyContainers { get; set; }

		protected PolicyExpectations Expect<TController>(Expression<Func<TController, ActionResult>> actionExpression) where TController : IController
		{
			if (PolicyContainers == null) throw new AssertionException("The property PolicyContainers must not be null! Did you forget to set the property?");
			return PolicyContainers.Expect(actionExpression);
		}

		protected PolicyExpectations Expect<TController>() where TController : IController
		{
			if (PolicyContainers == null) throw new AssertionException("The property PolicyContainers must not be null! Did you forget to set the property?");
			return PolicyContainers.Expect<TController>();
		}
	}
}