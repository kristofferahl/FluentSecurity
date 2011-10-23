using System;
using System.Linq.Expressions;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;

namespace FluentSecurity.TestHelper
{
	public static class ExpectationExpressionExtensions
	{
		public static ExpectationExpression Has<TSecurityPolicy>(this ExpectationExpression expectationExpression) where TSecurityPolicy : class, ISecurityPolicy
		{
			return expectationExpression.Add(new HasTypeExpectation<TSecurityPolicy>());
		}

		public static ExpectationExpression Has<TSecurityPolicy>(this ExpectationExpression expectationExpression, Expression<Func<TSecurityPolicy, bool>> predicate) where TSecurityPolicy : class, ISecurityPolicy
		{
			return expectationExpression.Add(new HasTypeExpectation<TSecurityPolicy>(predicate));
		}

		public static ExpectationExpression Has<TSecurityPolicy>(this ExpectationExpression expectationExpression, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new HasInstanceExpectation(instance));
		}

		public static ExpectationExpression DoesNotHave<TSecurityPolicy>(this ExpectationExpression expectationExpression) where TSecurityPolicy : class, ISecurityPolicy
		{
			return expectationExpression.Add(new DoesNotHaveTypeExpectation<TSecurityPolicy>());
		}

		public static ExpectationExpression DoesNotHave<TSecurityPolicy>(this ExpectationExpression expectationExpression, Expression<Func<TSecurityPolicy, bool>> predicate) where TSecurityPolicy : class, ISecurityPolicy
		{
			return expectationExpression.Add(new DoesNotHaveTypeExpectation<TSecurityPolicy>(predicate));
		}

		public static ExpectationExpression DoesNotHave<TSecurityPolicy>(this ExpectationExpression expectationExpression, TSecurityPolicy instance) where TSecurityPolicy : ISecurityPolicy
		{
			return expectationExpression.Add(new DoesNotHaveInstanceExpectation(instance));
		}
	}
}