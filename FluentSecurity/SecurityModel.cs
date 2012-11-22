using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;

namespace FluentSecurity
{
	internal class SecurityModel : IAdvanced
	{
		private readonly List<Type> _profiles = new List<Type>();
		private readonly List<IPolicyContainer> _policyContainers = new List<IPolicyContainer>();
		private readonly List<IConvention> _conventions = new List<IConvention>();

		public Func<bool> IsAuthenticated { get; internal set; }
		public Func<IEnumerable<object>> Roles { get; internal set; }
		public ISecurityServiceLocator ExternalServiceLocator { get; internal set; }

		public IEnumerable<Type> Profiles { get { return _profiles.AsReadOnly(); } }
		public IEnumerable<IPolicyContainer> PolicyContainers { get { return _policyContainers.AsReadOnly(); } }
		public IEnumerable<IConvention> Conventions { get { return _conventions.AsReadOnly(); } }

		public Cache DefaultResultsCacheLifecycle { get; internal set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; internal set; }
		public bool ShouldIgnoreMissingConfiguration { get; internal set; }

		public SecurityModel()
		{
			ShouldIgnoreMissingConfiguration = false;
			DefaultResultsCacheLifecycle = Cache.DoNotCache;
		}

		public void ApplyConfiguration(Action<ConventionConfiguration> conventionConfiguration)
		{
			if (conventionConfiguration == null) throw new ArgumentNullException("conventionConfiguration");
			var configuration = new ConventionConfiguration(_conventions);
			conventionConfiguration.Invoke(configuration);
		}

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
			profileConfiguration.Initialize(this);
			profileConfiguration.Configure();
			_profiles.Add(profileConfiguration.GetType());
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