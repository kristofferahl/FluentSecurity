using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class ConventionPolicyContainer : IPolicyContainerConfiguration
	{
		private readonly By _defaultCacheLevel;
		private readonly IList<IPolicyContainerConfiguration> _policyContainers;

		public ConventionPolicyContainer(IList<IPolicyContainerConfiguration> policyContainers, By defaultCacheLevel = By.Policy)
		{
			if (policyContainers == null)
				throw new ArgumentNullException("policyContainers", "A list of policycontainers was not provided");
			
			_policyContainers = policyContainers;
			_defaultCacheLevel = defaultCacheLevel;
		}

		public IPolicyContainerConfiguration AddPolicy(ISecurityPolicy securityPolicy)
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.AddPolicy(securityPolicy);
			
			return this;
		}

		public IPolicyContainerConfiguration<TSecurityPolicy> AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new PolicyContainerConfigurationWrapper<TSecurityPolicy>(AddPolicy(new LazySecurityPolicy<TSecurityPolicy>()));
		}

		public IPolicyContainerConfiguration RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : class, ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.RemovePolicy(predicate);

			return this;
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy
		{
			return Cache<TSecurityPolicy>(lifecycle, _defaultCacheLevel);
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.Cache<TSecurityPolicy>(lifecycle, level);

			return this;
		}

		public IPolicyContainerConfiguration ClearCacheStrategies()
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.ClearCacheStrategies();

			return this;
		}

		public IPolicyContainerConfiguration ClearCacheStrategyFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.ClearCacheStrategyFor<TSecurityPolicy>();

			return this;
		}
	}
}