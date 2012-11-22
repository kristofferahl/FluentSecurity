using System;
using FluentSecurity.Internals;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationConfiguration
	{
		private readonly Conventions _conventions;

		internal ViolationConfiguration(Conventions conventions)
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
			
			_conventions.RemoveAll(convention =>
				convention is IPolicyViolationHandlerConvention &&
				predicate.Invoke(convention as IPolicyViolationHandlerConvention)
				);
		}

		public ViolationHandlerConfiguration<TSecurityPolicy> Of<TSecurityPolicy>() where TSecurityPolicy : class, ISecurityPolicy
		{
			return new ViolationHandlerConfiguration<TSecurityPolicy>(this);
		}

		public ViolationHandlerConfiguration Of(Func<PolicyResult, bool> predicate)
		{
			return new ViolationHandlerConfiguration(this, predicate);
		}
	}
}