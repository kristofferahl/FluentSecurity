using System;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationConfiguration<TSecurityPolicyViolationHandler> where TSecurityPolicyViolationHandler : ISecurityPolicyViolationHandler
	{
		private readonly ConventionConfiguration _conventionConfiguration;

		internal ViolationConfiguration(ConventionConfiguration conventionConfiguration)
		{
			if (conventionConfiguration == null) throw new ArgumentNullException("conventionConfiguration");
			_conventionConfiguration = conventionConfiguration;
		}

		public void AddConvention(IPolicyViolationHandlerConvention convention)
		{
			if (convention == null) throw new ArgumentNullException("convention");
			_conventionConfiguration.Insert(0, convention);
		}

		public void RemoveConventions<TPolicyViolationHandlerConvention>() where TPolicyViolationHandlerConvention : class, IPolicyViolationHandlerConvention
		{
			RemoveConventions(c => c is TPolicyViolationHandlerConvention);
		}

		public void RemoveConventions(Func<IPolicyViolationHandlerConvention, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			
			_conventionConfiguration.RemoveAll(convention =>
				convention is IPolicyViolationHandlerConvention &&
				predicate.Invoke(convention as IPolicyViolationHandlerConvention)
				);
		}

		public ViolationHandlerConfiguration<TSecurityPolicy, TSecurityPolicyViolationHandler> Of<TSecurityPolicy>() where TSecurityPolicy : class, ISecurityPolicy
		{
			return new ViolationHandlerConfiguration<TSecurityPolicy, TSecurityPolicyViolationHandler>(this);
		}

		public ViolationHandlerConfiguration<TSecurityPolicyViolationHandler> Of(Func<PolicyResult, bool> predicate)
		{
			return new ViolationHandlerConfiguration<TSecurityPolicyViolationHandler>(this, predicate);
		}
	}
}