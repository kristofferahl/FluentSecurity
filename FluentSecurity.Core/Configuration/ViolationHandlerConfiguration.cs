using System;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationHandlerConfiguration<TSecurityPolicy, TSecurityPolicyViolationHandler> : ViolationHandlerConfigurationBase<TSecurityPolicyViolationHandler> where TSecurityPolicy : class, ISecurityPolicy where TSecurityPolicyViolationHandler : ISecurityPolicyViolationHandler
	{
		internal ViolationHandlerConfiguration(ViolationConfiguration<TSecurityPolicyViolationHandler> violationConfiguration) : base(violationConfiguration) {}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : class, TSecurityPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PolicyTypeToPolicyViolationHandlerTypeConvention<TSecurityPolicy, TPolicyViolationHandler>());
		}

		public void IsHandledBy<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandlerFactory) where TPolicyViolationHandler : class, TSecurityPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PolicyTypeToPolicyViolationHandlerInstanceConvention<TSecurityPolicy, TPolicyViolationHandler>(policyViolationHandlerFactory));
		}
	}

	public class ViolationHandlerConfiguration<TSecurityPolicyViolationHandler> : ViolationHandlerConfigurationBase<TSecurityPolicyViolationHandler> where TSecurityPolicyViolationHandler : ISecurityPolicyViolationHandler
	{
		public Func<PolicyResult, bool> Predicate { get; private set; }

		internal ViolationHandlerConfiguration(ViolationConfiguration<TSecurityPolicyViolationHandler> violationConfiguration, Func<PolicyResult, bool> predicate) : base(violationConfiguration)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			Predicate = predicate;
		}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PredicateToPolicyViolationHandlerTypeConvention<TPolicyViolationHandler>(Predicate));
		}

		public void IsHandledBy<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandlerFactory) where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PredicateToPolicyViolationHandlerInstanceConvention<TPolicyViolationHandler>(policyViolationHandlerFactory, Predicate));
		}
	}

	public abstract class ViolationHandlerConfigurationBase<TSecurityPolicyViolationHandler> where TSecurityPolicyViolationHandler : ISecurityPolicyViolationHandler
	{
		protected ViolationConfiguration<TSecurityPolicyViolationHandler> ViolationConfiguration { get; private set; }

		internal ViolationHandlerConfigurationBase(ViolationConfiguration<TSecurityPolicyViolationHandler> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			ViolationConfiguration = violationConfiguration;
		}
	}
}