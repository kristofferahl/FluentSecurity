using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationsExpression
	{
		private readonly Conventions _conventions;

		internal ViolationsExpression(Conventions conventions)
		{
			_conventions = conventions;
		}

		public void AddConvention(IPolicyViolationHandlerConvention convention)
		{
			_conventions.Insert(0, convention);
		}

		public ViolationHandlerExpression<TSecurityPolicy> Of<TSecurityPolicy>() where TSecurityPolicy : class, ISecurityPolicy
		{
			return new ViolationHandlerExpression<TSecurityPolicy>(this);
		}
	}
}