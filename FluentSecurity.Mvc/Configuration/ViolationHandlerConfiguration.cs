using System;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class ViolationHandlerConfiguration<TSecurityPolicy> : ViolationHandlerConfigurationBase where TSecurityPolicy : class, ISecurityPolicy
	{
		internal ViolationHandlerConfiguration(ViolationConfiguration violationConfiguration) : base(violationConfiguration) {}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PolicyTypeToPolicyViolationHandlerTypeConvention<TSecurityPolicy, TPolicyViolationHandler>());
		}

		public void IsHandledBy<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandlerFactory) where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PolicyTypeToPolicyViolationHandlerInstanceConvention<TSecurityPolicy, TPolicyViolationHandler>(policyViolationHandlerFactory));
		}
	}

	public class ViolationHandlerConfiguration : ViolationHandlerConfigurationBase
	{
		public Func<PolicyResult, bool> Predicate { get; private set; }

		internal ViolationHandlerConfiguration(ViolationConfiguration violationConfiguration, Func<PolicyResult, bool> predicate) : base(violationConfiguration)
		{
			if (predicate == null) throw new ArgumentNullException("predicate");
			Predicate = predicate;
		}

		public void IsHandledBy<TPolicyViolationHandler>() where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PredicateToPolicyViolationHandlerTypeConvention<TPolicyViolationHandler>(Predicate));
		}

		public void IsHandledBy<TPolicyViolationHandler>(Func<TPolicyViolationHandler> policyViolationHandlerFactory) where TPolicyViolationHandler : class, IPolicyViolationHandler
		{
			ViolationConfiguration.AddConvention(new PredicateToPolicyViolationHandlerInstanceConvention<TPolicyViolationHandler>(policyViolationHandlerFactory, Predicate));
		}
	}

	public abstract class ViolationHandlerConfigurationBase
	{
		protected ViolationConfiguration ViolationConfiguration { get; private set; }

		internal ViolationHandlerConfigurationBase(ViolationConfiguration violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			ViolationConfiguration = violationConfiguration;
		}
	}
}