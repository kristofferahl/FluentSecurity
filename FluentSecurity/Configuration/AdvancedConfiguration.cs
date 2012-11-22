using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Internals;
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
			_model.Conventions.AddRange(new List<IConvention>
			{
				new FindByPolicyNameConvention(),
				new FindDefaultPolicyViolationHandlerByNameConvention()
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
			violationConfiguration.Invoke(new ViolationConfiguration(_model.Conventions));
		}

		public void Conventions(Action<Conventions> conventions)
		{
			if (conventions == null) throw new ArgumentNullException("conventions");
			conventions.Invoke(_model.Conventions);
		}
	}
}