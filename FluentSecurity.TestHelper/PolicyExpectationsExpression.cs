using System;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectationsExpression
	{
		public PolicyExpectationsExpression()
		{
			Expectations = new PolicyExpectations();
		}

		public PolicyExpectations Expectations { get; private set; }

		public IExpectationExpression Expect<TController>(Expression<Func<TController, ActionResult>> actionExpression) where TController : IController
		{
			return Expectations.For(actionExpression);
		}

		public IExpectationExpression Expect<TController>() where TController : IController
		{
			return Expectations.For<TController>();
		}
	}

	public class PolicyExpectationsExpression<TController> where TController : IController
	{
		public PolicyExpectationsExpression()
		{
			Expectations = new PolicyExpectations();
		}

		public PolicyExpectations Expectations { get; private set; }

		public IExpectationExpression Expect(Expression<Func<TController, ActionResult>> actionExpression)
		{
			return Expectations.For(actionExpression);
		}

		public IExpectationExpression Expect()
		{
			return Expectations.For<TController>();
		}
	}
}