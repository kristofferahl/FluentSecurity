using System;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity.Configuration
{
	public class PolicyContainerConfigurationWrapper<TCurrentSecurityPolicy> : IPolicyContainerConfiguration<TCurrentSecurityPolicy> where TCurrentSecurityPolicy : ISecurityPolicy
	{
		private readonly IPolicyContainerConfiguration _inner;

		public PolicyContainerConfigurationWrapper(IPolicyContainerConfiguration policyContainerConfiguration)
		{
			_inner = policyContainerConfiguration;
		}

		public IPolicyContainerConfiguration AddPolicy(ISecurityPolicy securityPolicy)
		{
			return _inner.AddPolicy(securityPolicy);
		}

		public IPolicyContainerConfiguration<TSecurityPolicy> AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return _inner.AddPolicy<TSecurityPolicy>();
		}

		public IPolicyContainerConfiguration RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : class, ISecurityPolicy
		{
			return _inner.RemovePolicy(predicate);
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy
		{
			return _inner.Cache<TSecurityPolicy>(lifecycle);
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy
		{
			return _inner.Cache<TSecurityPolicy>(lifecycle, level);
		}

		public IPolicyContainerConfiguration ClearCacheStrategies()
		{
			return _inner.ClearCacheStrategies();
		}

		public IPolicyContainerConfiguration ClearCacheStrategyFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return _inner.ClearCacheStrategyFor<TSecurityPolicy>();
		}
	}
}