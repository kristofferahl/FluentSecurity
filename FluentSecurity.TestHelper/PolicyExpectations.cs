using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FluentSecurity.TestHelper
{
	public class PolicyExpectations
	{
		private readonly ISecurityConfiguration _configuration;
		private readonly IList<ExpectationExpression> _expectationsExpressions;
		
		public PolicyExpectations(ISecurityConfiguration configuration)
		{
			if (configuration == null) throw new ArgumentNullException("configuration");

			_configuration = configuration;
			_expectationsExpressions = new List<ExpectationExpression>();

			Using(Settings.DefaultExpectationVerifyer);
			Using(Settings.DefaultExpectationGroupBuilder);
			Using(Settings.DefaultExpectationViolationHandler);
		}

		public void Using(Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> expectationVerifyerProvider)
		{
			ExpectationVerifyerProvider = expectationVerifyerProvider;
		}

		public void Using(Func<ISecurityConfiguration, IExpectationViolationHandler> expectationViolationHandler)
		{
			ExpectationViolationHandlerProvider = expectationViolationHandler;
		}

		public void Using(Func<ISecurityConfiguration, IExpectationGroupBuilder> expectationGroupBuilder)
		{
			ExpectationGroupBuilderProvider = expectationGroupBuilder;
		}

		internal Func<ISecurityConfiguration, IExpectationViolationHandler, IExpectationVerifyer> ExpectationVerifyerProvider { get; private set; }
		internal Func<ISecurityConfiguration, IExpectationViolationHandler> ExpectationViolationHandlerProvider { get; private set; }
		internal Func<ISecurityConfiguration, IExpectationGroupBuilder> ExpectationGroupBuilderProvider { get; private set; }

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

		public IEnumerable<ExpectationResult> VerifyAll()
		{
			var expectationViolationHandler = ExpectationViolationHandlerProvider(_configuration);
			var expectationVerifyer = ExpectationVerifyerProvider(_configuration, expectationViolationHandler);

			var expectationGroups = BuildExpectationGroups();

			return expectationVerifyer.VerifyExpectationsOf(expectationGroups);
		}

		public IEnumerable<ExpectationGroup> BuildExpectationGroups()
		{
			var expectationGroupBuilder = ExpectationGroupBuilderProvider(_configuration);
			return expectationGroupBuilder.Build(_expectationsExpressions);
		}
	}
}