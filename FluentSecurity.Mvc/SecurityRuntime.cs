using System;
using System.Linq;
using FluentSecurity.Configuration;
using FluentSecurity.Core;

namespace FluentSecurity
{
	public class SecurityRuntime : SecurityRuntimeBase
	{
		public void ApplyConfiguration(Action<ViolationConfiguration> violationConfiguration)
		{
			if (violationConfiguration == null) throw new ArgumentNullException("violationConfiguration");
			var conventionsConfiguration = new ConventionConfiguration(_conventions);
			var configuration = new ViolationConfiguration(conventionsConfiguration);
			violationConfiguration.Invoke(configuration);
		}

		public void ApplyConfiguration(SecurityProfile profileConfiguration)
		{
			if (profileConfiguration == null) throw new ArgumentNullException("profileConfiguration");
			
			var profileType = profileConfiguration.GetType();
			if (_profiles.Any(pi => pi.Type == profileType)) return;

			var profileImport = new ProfileImport(profileType);
			_profiles.Add(profileImport);

			profileConfiguration.Initialize(this);
			profileConfiguration.Configure();
			
			profileImport.MarkCompleted();
		}

		public PolicyContainer AddPolicyContainer(PolicyContainer policyContainer)
		{
			if (policyContainer == null) throw new ArgumentNullException("policyContainer");

			var existingContainer = PolicyContainers.GetContainerFor(policyContainer.ControllerName, policyContainer.ActionName);
			if (existingContainer != null) return (PolicyContainer) existingContainer;

			_policyContainers.Add(policyContainer);

			return policyContainer;
		}
	}
}