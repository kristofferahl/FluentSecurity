using System;
using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public interface IAdvancedConfiguration
	{
		Cache DefaultResultsCacheLifecycle { get; }
		Action<ISecurityContext> SecurityContextModifyer { get; }
	}
}