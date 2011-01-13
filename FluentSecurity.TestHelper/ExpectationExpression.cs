using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public class ExpectationExpression<TController> : IExpectationExpression
	{
		private readonly IList<IExpectation> _expectations;

		public Type Controller { get; private set; }
		public string Action { get; private set; }
		
		public IEnumerable<IExpectation> Expectations
		{
			get { return _expectations; }
		}

		public ExpectationExpression() : this(null) {}

		public ExpectationExpression(Expression<Func<TController, ActionResult>> actionExpression)
		{
			Controller = typeof (TController);
			
			if (actionExpression != null)
				Action = actionExpression.GetActionName();
			
			_expectations = new List<IExpectation>();
		}

		public IExpectationExpression Add(IExpectation expectation)
		{
			if (expectation == null) throw new ArgumentNullException("expectation");
			_expectations.Add(expectation);
			return this;
		}
	}
}