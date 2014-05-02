using System.Linq.Expressions;
using System.Reflection;

namespace FluentSecurity.Core
{
	public interface IActionNameResolver<in TContext> : IActionNameResolver
	{
		string Resolve(TContext context);
	}

	public interface IActionNameResolver
	{
		string Resolve(LambdaExpression actionExpression);
		string Resolve(MethodInfo actionMethod);
	}
}