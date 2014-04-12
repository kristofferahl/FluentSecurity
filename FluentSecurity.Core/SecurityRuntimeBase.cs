using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;

namespace FluentSecurity.Core
{
	public abstract class SecurityRuntimeBase : ISecurityRuntime
	{
		protected readonly List<ProfileImport> _profiles = new List<ProfileImport>();
		protected readonly List<IPolicyContainer> _policyContainers = new List<IPolicyContainer>();
		protected readonly List<IConvention> _conventions = new List<IConvention>();

		public Func<bool> IsAuthenticated { get; internal set; }
		public Func<IEnumerable<object>> Roles { get; internal set; }
		public ISecurityServiceLocator ExternalServiceLocator { get; internal set; }

		public IEnumerable<Type> Profiles { get { return _profiles.Where(pi => pi.Completed).Select(pi => pi.Type); } }
		public IEnumerable<IPolicyContainer> PolicyContainers { get { return _policyContainers.AsReadOnly(); } }
		public IEnumerable<IConvention> Conventions { get { return _conventions.AsReadOnly(); } }

		public Cache DefaultResultsCacheLifecycle { get; set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; set; }
		public bool ShouldIgnoreMissingConfiguration { get; set; }
		public ISecurityCache Cache { get; private set; }

		protected SecurityRuntimeBase(ISecurityCache cache)
		{
			Cache = cache;
			ShouldIgnoreMissingConfiguration = false;
			DefaultResultsCacheLifecycle = Caching.Cache.DoNotCache;;
		}

		public void ApplyConfiguration(Action<ConventionConfiguration> conventionConfiguration)
		{
			if (conventionConfiguration == null) throw new ArgumentNullException("conventionConfiguration");
			var configuration = new ConventionConfiguration(_conventions);
			conventionConfiguration.Invoke(configuration);
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