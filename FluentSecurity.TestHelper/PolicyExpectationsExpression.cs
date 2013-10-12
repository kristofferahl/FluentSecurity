using System;
using System.Linq.Expressions;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectationsExpression
	{
		public PolicyExpectationsExpression()
		{
			Expectations = new PolicyExpectations();
		}

		internal PolicyExpectations Expectations { get; private set; }

		public ExpectationExpression Expect<TController>(Expression<Func<TController, object>> actionExpression)
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect<TController>(Expression<Action<TController>> actionExpression)
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect<TController>()
		{
			return Expectations.For<TController>();
		}
	}

	public class PolicyExpectationsExpression<TController>
	{
		public PolicyExpectationsExpression()
		{
			Expectations = new PolicyExpectations();
		}

		internal PolicyExpectations Expectations { get; private set; }

		public ExpectationExpression Expect(Expression<Func<TController, object>> actionExpression)
		{
			return Expectations.For(actionExpression);
		}

		public ExpectationExpression Expect()
		{
			return Expectations.For<TController>();
		}
	}
}