using System.Linq.Expressions;
using System.Reflection;

namespace FluentSecurity.Core.Internals
{
	public static class LambdaExpressionExtensions
	{
		public static MethodInfo GetActionMethodInfo(this LambdaExpression actionExpression)
		{
			var expression = actionExpression.Body is UnaryExpression
				? ((UnaryExpression)actionExpression.Body).Operand
				: actionExpression.Body;

			return ((MethodCallExpression)expression).Method;
		}
	}
}