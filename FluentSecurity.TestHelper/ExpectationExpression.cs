using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public class ExpectationExpression<TController> : ExpectationExpression
	{
		public ExpectationExpression()
		{
			Controller = typeof(TController);
		}

		public ExpectationExpression(Expression<Func<TController, object>> actionExpression) : this()
		{
			if (actionExpression != null)
				Action = actionExpression.GetActionName();
		}

		public ExpectationExpression(Expression<Action<TController>> actionExpression) : this()
		{
			if (actionExpression != null)
				Action = actionExpression.GetActionName();
		}
	}

	public abstract class ExpectationExpression
	{
		private readonly IList<IExpectation> _expectations;

		internal Type Controller { get; set; }
		internal string Action { get; set; }

		internal IEnumerable<IExpectation> Expectations
		{
			get { return _expectations; }
		}

		protected ExpectationExpression()
		{
			_expectations = new List<IExpectation>();
		}

		internal ExpectationExpression Add(IExpectation expectation)
		{
			if (expectation == null) throw new ArgumentNullException("expectation");
			_expectations.Add(expectation);
			return this;
		}
	}
}