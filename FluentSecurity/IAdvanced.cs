using System;
using System.Collections.Generic;
using FluentSecurity.Caching;

namespace FluentSecurity
{
	public interface IAdvanced
	{
		IEnumerable<IConvention> Conventions { get; }
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
		bool ShouldIgnoreMissingConfiguration { get; }
	}
}