using System;
using FluentSecurity.Caching;
using FluentSecurity.Internals;

namespace FluentSecurity.Configuration
{
	public interface IAdvancedConfiguration
	{
		Conventions Conventions { get; }
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
		bool ShouldIgnoreMissingConfiguration { get; }
	}
}