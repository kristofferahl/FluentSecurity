using System;
using System.Linq;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration : AdvancedConfigurationBase<SecurityRuntime>
	{
		internal AdvancedConfiguration(SecurityRuntime runtime) : base(runtime)
		{
			if (!Runtime.Conventions.Any())
			{
				Conventions(conventions =>
				{
					conventions.Add(new FindByPolicyNameConvention());
					conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
				});
			}
		}

		public void Violations(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			Runtime.ApplyConfiguration(violationConfiguration);
		}
	}
}