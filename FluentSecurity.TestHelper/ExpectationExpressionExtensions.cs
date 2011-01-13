using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public static class ExpectationExpressionExtensions
	{
		public static IExpectationExpression Has<TSecurityPolicy>(this IExpectationExpression expectationExpression) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new HasTypeExpectation<TSecurityPolicy>());
		}

		public static IExpectationExpression Has<TSecurityPolicy>(this IExpectationExpression expectationExpression, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new HasInstanceExpectation(instance));
		}

		public static IExpectationExpression DoesNotHave<TSecurityPolicy>(this IExpectationExpression expectationExpression) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new DoesNotHaveTypeExpectation<TSecurityPolicy>());
		}

		public static IExpectationExpression DoesNotHave<TSecurityPolicy>(this IExpectationExpression expectationExpression, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new DoesNotHaveInstanceExpectation(instance));
		}
	}
}