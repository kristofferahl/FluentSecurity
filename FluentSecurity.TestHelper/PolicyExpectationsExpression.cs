using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectationsExpression
	{
		public PolicyExpectationsExpression()
		{
			Expectations = new PolicyExpectations();
		}

		internal PolicyExpectations Expectations { get; private set; }

		public ExpectationExpression Expect<TController>(Expression<Func<TController, ActionResult>> actionExpression) where TController : IController
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect<TController>(Expression<Func<TController, Task<ActionResult>>> actionExpression) where TController : IController
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect<TController>() where TController : IController
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

		internal PolicyExpectations Expectations { get; private set; }

		public ExpectationExpression Expect(Expression<Func<TController, ActionResult>> actionExpression)
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect<TResult>(Expression<Func<TController, Task<TResult>>> actionExpression) where TResult : ActionResult
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect()
		{
			return Expectations.For<TController>();
		}
	}
}