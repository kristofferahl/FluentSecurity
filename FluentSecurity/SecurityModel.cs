using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Internals;

namespace FluentSecurity
{
	internal class SecurityModel : IAdvanced
	{
		private readonly List<IConvention> _conventions = new List<IConvention>(); 

		public IList<Type> Profiles { get; private set; }
		public IList<IPolicyContainer> PolicyContainers { get; private set; }

		public IEnumerable<IConvention> Conventions
		{
			get { return _conventions; }
		}

		public Cache DefaultResultsCacheLifecycle { get; internal set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; internal set; }
		public bool ShouldIgnoreMissingConfiguration { get; internal set; }

		public SecurityModel()
		{
			Profiles = new List<Type>();
			PolicyContainers = new List<IPolicyContainer>();
			ShouldIgnoreMissingConfiguration = false;
			DefaultResultsCacheLifecycle = Cache.DoNotCache;
		}

		public void ApplyConventions(Action<ConventionConfiguration> conventionConfiguration)
		{
			var configuration = new ConventionConfiguration(_conventions);
			conventionConfiguration.Invoke(configuration);
		}

		public void ApplyViolationConfiguration(Action<ViolationConfiguration> violationConfiguration)
		{
			var conventionsConfiguration = new ConventionConfiguration(_conventions);
			var configuration = new ViolationConfiguration(conventionsConfiguration);
			violationConfiguration.Invoke(configuration);
		}
	}
}