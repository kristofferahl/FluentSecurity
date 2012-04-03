using System;
using System.Collections.Generic;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class ConventionPolicyContainer : IConventionPolicyContainer
	{
		private readonly By _defaultCacheLevel;
		private readonly IList<IPolicyContainer> _policyContainers;

		public ConventionPolicyContainer(IList<IPolicyContainer> policyContainers, By defaultCacheLevel = By.Policy)
		{
			if (policyContainers == null)
				throw new ArgumentNullException("policyContainers", "A list of policycontainers was not provided");
			
			_policyContainers = policyContainers;
			_defaultCacheLevel = defaultCacheLevel;
		}

		public IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.AddPolicy(securityPolicy);
			
			return this;
		}

		public IConventionPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.RemovePolicy(predicate);

			return this;
		}

		public IConventionPolicyContainer Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy
		{
			return Cache<TSecurityPolicy>(lifecycle, _defaultCacheLevel);
		}

		public IConventionPolicyContainer Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.Cache<TSecurityPolicy>(lifecycle, level);

			return this;
		}

		public IConventionPolicyContainer ClearCacheStrategies()
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.ClearCacheStrategies();

			return this;
		}

		public IConventionPolicyContainer ClearCacheStrategiesFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.ClearCacheStrategyFor<TSecurityPolicy>();

			return this;
		}
	}
}