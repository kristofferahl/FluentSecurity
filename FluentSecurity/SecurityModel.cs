using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Internals;

namespace FluentSecurity
{
	internal class SecurityModel : IAdvanced
	{
		public IList<Type> Profiles { get; private set; }
		public IList<IPolicyContainer> PolicyContainers { get; private set; }
		public Conventions Conventions { get; private set; }
		
		public Cache DefaultResultsCacheLifecycle { get; internal set; }
		public Action<ISecurityContext> SecurityContextModifyer { get; internal set; }
		public bool ShouldIgnoreMissingConfiguration { get; internal set; }

		public SecurityModel()
		{
			Profiles = new List<Type>();
			PolicyContainers = new List<IPolicyContainer>();
			Conventions = new Conventions();
			ShouldIgnoreMissingConfiguration = false;
			DefaultResultsCacheLifecycle = Cache.DoNotCache;
		}
	}
}