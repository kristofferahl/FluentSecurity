using System;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationConfigurationExpression
	{
		private readonly Conventions _conventions;

		internal ViolationConfigurationExpression(Conventions conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			_conventions = conventions;
		}

		public void AddConvention(IPolicyViolationHandlerConvention convention)
		{
			if (convention == null) throw new ArgumentNullException("convention");
			_conventions.Insert(0, convention);
		}

		public void RemoveConventions<TPolicyViolationHandlerConvention>() where TPolicyViolationHandlerConvention : class, IPolicyViolationHandlerConvention
		{
			RemoveConventions(c => c is TPolicyViolationHandlerConvention);
		}

		public void RemoveConventions(Func<IPolicyViolationHandlerConvention, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");

			var conventionsToRemove = _conventions.OfType<IPolicyViolationHandlerConvention>().Where(predicate).ToList();
			foreach (var convention in conventionsToRemove)
				_conventions.Remove(convention);
		}

		public ViolationHandlerExpression<TSecurityPolicy> Of<TSecurityPolicy>() where TSecurityPolicy : class, ISecurityPolicy
		{
			return new ViolationHandlerExpression<TSecurityPolicy>(this);
		}

		public ViolationHandlerExpression Of(Func<PolicyResult, bool> predicate)
		{
			return new ViolationHandlerExpression(this, predicate);
		}
	}
}