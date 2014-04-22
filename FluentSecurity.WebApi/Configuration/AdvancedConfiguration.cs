using System;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.WebApi.Policy.ViolationHandlers;

namespace FluentSecurity.WebApi.Configuration
{
	public class AdvancedConfiguration : AdvancedConfigurationBase<WebApiSecurityRuntime>
	{
		internal AdvancedConfiguration(WebApiSecurityRuntime runtime) : base(runtime)
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

		public void Violations(Action<ViolationConfiguration<IWebApiPolicyViolationHandler>> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			Runtime.ApplyConfiguration(violationConfiguration);
		}
	}
}