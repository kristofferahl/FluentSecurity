using FluentSecurity.Caching;

namespace FluentSecurity.Configuration
{
	public interface IAdvancedConfiguration
	{
		Cache DefaultResultsCacheLifecycle { get; }
	}
}