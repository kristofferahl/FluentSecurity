using System;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration
	{
		private readonly SecurityRuntime _runtime;

		internal AdvancedConfiguration(SecurityRuntime runtime)
		{
			if (runtime == null) throw new ArgumentNullException("runtime");
			_runtime = runtime;

			if (!_runtime.Conventions.Any())
			{
				Conventions(conventions =>
				{
					conventions.Add(new FindByPolicyNameConvention());
					conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
				});
			}
		}

		public void IgnoreMissingConfiguration()
		{
			_runtime.ShouldIgnoreMissingConfiguration = true;
		}

		public void ModifySecurityContext(Action<ISecurityContext> modifyer)
		{
			_runtime.SecurityContextModifyer = modifyer;
		}

		public void SetDefaultResultsCacheLifecycle(Cache lifecycle)
		{
			_runtime.DefaultResultsCacheLifecycle = lifecycle;
		}

		public void Violations(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			_runtime.ApplyConfiguration(violationConfiguration);
		}

		public void Conventions(Action<ConventionConfiguration> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			_runtime.ApplyConfiguration(conventions);
		}
	}
}