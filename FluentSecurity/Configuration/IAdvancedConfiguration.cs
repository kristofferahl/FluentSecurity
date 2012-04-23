using System;
using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public interface IAdvancedConfiguration
	{
		Conventions Conventions { get; }
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
	}
}