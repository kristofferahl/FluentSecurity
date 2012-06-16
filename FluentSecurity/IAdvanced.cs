using System;
using FluentSecurity.Caching;
using FluentSecurity.Internals;

namespace FluentSecurity
{
	public interface IAdvanced
	{
		Conventions Conventions { get; }
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
		bool ShouldIgnoreMissingConfiguration { get; }
	}
}