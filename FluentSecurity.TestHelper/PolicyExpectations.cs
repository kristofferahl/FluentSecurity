using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectations
	{
		private readonly IList<ExpectationExpression> _expectationsExpressions;
		
		public PolicyExpectations()
		{
			_expectationsExpressions = new List<ExpectationExpression>();

			ConstructExpectationVerifyerUsing(Settings.DefaultExpectationVerifyerConstructor);
			SetExpectationGroupBuilder(Settings.DefaultExpectationGroupBuilder);
			SetExpectationViolationHandler(Settings.DefaultExpectationViolationHandler);
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

		internal Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> ExpectationVerifyerProvider { get; private set; }
		internal IExpectationViolationHandler ExpectationViolationHandler { get; private set; }
		internal IExpectationGroupBuilder ExpectationGroupBuilder { get; private set; }

		public ExpectationExpression<TController> For<TController>()
		{
			var expression = new ExpectationExpression<TController>();
			_expectationsExpressions.Add(expression);
			return expression;
		}

		public ExpectationExpression<TController> For<TController>(Expression<Func<TController, object>> actionExpression)
		{
			var expression = new ExpectationExpression<TController>(actionExpression);
			_expectationsExpressions.Add(expression);
			return expression;
		}

		public ExpectationExpression<TController> For<TController>(Expression<Action<TController>> actionExpression)
		{
			var expression = new ExpectationExpression<TController>(actionExpression);
			_expectationsExpressions.Add(expression);
			return expression;
		}

		public IEnumerable<ExpectationResult> VerifyAll(ISecurityConfiguration configuration)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");
			return ExpectationVerifyerProvider(configuration, ExpectationViolationHandler).VerifyExpectationsOf(ExpectationGroups);
		}

		internal IEnumerable<ExpectationGroup> ExpectationGroups
		{
			get { return ExpectationGroupBuilder.Build(_expectationsExpressions); }
		}
	}
}