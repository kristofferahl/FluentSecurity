using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectations
	{
		private readonly IList<IExpectationExpression> _expectationsExpressions;

		public PolicyExpectations()
		{
			_expectationsExpressions = new List<IExpectationExpression>();

			ConstructExpectationVerifyerUsing((configuration, handler) => new ExpectationVerifyer(configuration, handler));
			SetExpectationGroupBuilder(new ExpectationGroupBuilder());
			SetExpectationViolationHandler(new DefaultExpectationViolationHandler());
		}

		public void ConstructExpectationVerifyerUsing(Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> expectationVerifyerProvider)
		{
			ExpectationVerifyerProvider = expectationVerifyerProvider;
		}

		public void SetExpectationViolationHandler(IExpectationViolationHandler expectationViolationHandler)
		{
			ExpectationViolationHandler = expectationViolationHandler;
		}

		public void SetExpectationGroupBuilder(IExpectationGroupBuilder expectationGroupBuilder)
		{
			ExpectationGroupBuilder = expectationGroupBuilder;
		}

		public Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> ExpectationVerifyerProvider { get; private set; }
		public IExpectationViolationHandler ExpectationViolationHandler { get; private set; }
		public IExpectationGroupBuilder ExpectationGroupBuilder { get; private set; }

		public IExpectationExpression For<TController>()
		{
			var expression = new ExpectationExpression<TController>();
			return Add(expression);
		}

		public IExpectationExpression For<TController>(Expression<Func<TController, ActionResult>> actionExpression)
		{
			var expression = new ExpectationExpression<TController>(actionExpression);
			return Add(expression);
		}

		private IExpectationExpression Add(IExpectationExpression expressionExpression)
		{
			_expectationsExpressions.Add(expressionExpression);
			return expressionExpression;
		}

		public IEnumerable<ExpectationResult> VerifyAll(ISecurityConfiguration configuration)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			return ExpectationVerifyerProvider(configuration, ExpectationViolationHandler).VerifyExpectationsOf(ExpectationGroups);
		}

		public IEnumerable<ExpectationGroup> ExpectationGroups
		{
			get { return ExpectationGroupBuilder.Build(_expectationsExpressions); }
		}
	}
}