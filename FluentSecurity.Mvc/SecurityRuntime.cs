using System;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class SecurityRuntime : SecurityRuntimeBase
	{
		public SecurityRuntime(ISecurityCache cache, ITypeFactory typeFactory) : base(cache, typeFactory) {}

		public void ApplyConfiguration<TSecurityPolicyViolationHandler>(Action<ViolationConfiguration<TSecurityPolicyViolationHandler>> violationConfiguration) where TSecurityPolicyViolationHandler : ISecurityPolicyViolationHandler
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			var conventionsConfiguration = new ConventionConfiguration(_conventions);
			var configuration = new ViolationConfiguration<TSecurityPolicyViolationHandler>(conventionsConfiguration);
			violationConfiguration.Invoke(configuration);
		}
	}
}