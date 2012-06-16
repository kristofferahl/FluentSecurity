using System;
using FluentSecurity.Caching;
using FluentSecurity.Internals;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration : IAdvancedConfiguration
	{
		internal AdvancedConfiguration()
		{
			Conventions = new Conventions
			{
				new FindByPolicyNameConvention(),
				new FindDefaultPolicyViolationHandlerByNameConvention()
			};

			SetDefaultResultsCacheLifecycle(Cache.DoNotCache);
			ShouldIgnoreMissingConfiguration = false;
		}

		public Conventions Conventions { get; private set; }
		public Cache DefaultResultsCacheLifecycle { get; private set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; private set; }
		public bool ShouldIgnoreMissingConfiguration { get; private set; }

		public void IgnoreMissingConfiguration()
		{
			ShouldIgnoreMissingConfiguration = true;
		}

		public void ModifySecurityContext(Action<ISecurityContext> modifyer)
		{
			SecurityContextModifyer = modifyer;
		}

		public void SetDefaultResultsCacheLifecycle(Cache lifecycle)
		{
			DefaultResultsCacheLifecycle = lifecycle;
		}

		public void Violations(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			violationConfiguration.Invoke(new ViolationConfiguration(Conventions));
		}
	}
}