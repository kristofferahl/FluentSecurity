using System;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity.Configuration
{
	public interface IPolicyContainerConfiguration
	{
		IPolicyContainerConfiguration AddPolicy(ISecurityPolicy securityPolicy);
		IPolicyContainerConfiguration<TSecurityPolicy> AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy;
		IPolicyContainerConfiguration RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : class, ISecurityPolicy;
		IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy;
		IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy;
		IPolicyContainerConfiguration ClearCacheStrategies();
		IPolicyContainerConfiguration ClearCacheStrategyFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy;
	}

	public interface IPolicyContainerConfiguration<TSecurityPolicy> : IPolicyContainerConfiguration where TSecurityPolicy : ISecurityPolicy
	{
		IPolicyContainerConfiguration DoNotCache();
		IPolicyContainerConfiguration DoNotCache(By level);
		IPolicyContainerConfiguration CachePerHttpRequest();
		IPolicyContainerConfiguration CachePerHttpRequest(By level);
		IPolicyContainerConfiguration CachePerHttpSession();
		IPolicyContainerConfiguration CachePerHttpSession(By level);
	}
}