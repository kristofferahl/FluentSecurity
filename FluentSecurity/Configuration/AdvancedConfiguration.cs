using System;
using FluentSecurity.Caching;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Configuration
{
	public class AdvancedConfiguration
	{
		private readonly SecurityModel _model;

		internal AdvancedConfiguration(SecurityModel model)
		{
			if (model == null) throw new ArgumentNullException("model");
			_model = model;

			Conventions(conventions =>
			{
				conventions.Add(new FindByPolicyNameConvention());
				conventions.Add(new FindDefaultPolicyViolationHandlerByNameConvention());
			});
		}

		public void IgnoreMissingConfiguration()
		{
			_model.ShouldIgnoreMissingConfiguration = true;
		}

		public void ModifySecurityContext(Action<ISecurityContext> modifyer)
		{
			_model.SecurityContextModifyer = modifyer;
		}

		public void SetDefaultResultsCacheLifecycle(Cache lifecycle)
		{
			_model.DefaultResultsCacheLifecycle = lifecycle;
		}

		public void Violations(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			_model.ApplyViolationConfiguration(violationConfiguration);
		}

		public void Conventions(Action<ConventionConfiguration> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			_model.ApplyConventions(conventions);
		}
	}
}