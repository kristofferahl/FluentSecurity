using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Internals;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer, IPolicyContainerConfiguration
	{
		internal IPolicyAppender PolicyAppender;
		internal readonly List<PolicyResultCacheStrategy> CacheStrategies;
		internal Func<ISecurityConfiguration> SecurityConfigurationProvider;

		private readonly IList<ISecurityPolicy> _policies;

		public PolicyContainer(string controllerName, string actionName, IPolicyAppender policyAppender)
		{
			if (controllerName.IsNullOrEmpty())
				throw new ArgumentException("Controllername must not be null or empty!", "controllerName");

			if (actionName.IsNullOrEmpty())
				throw new ArgumentException("Actionname must not be null or empty!", "actionName");

			if (policyAppender == null)
				throw new ArgumentNullException("policyAppender");

			_policies = new List<ISecurityPolicy>();

			ControllerName = controllerName;
			ActionName = actionName;
			
			PolicyAppender = policyAppender;
			CacheStrategies = new List<PolicyResultCacheStrategy>();
			SecurityConfigurationProvider = () => SecurityConfiguration.Current;
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context)
		{
			if (_policies.Count.Equals(0))
				throw ExceptionFactory.CreateConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var defaultResultsCacheLifecycle = SecurityConfigurationProvider.Invoke().Advanced.DefaultResultsCacheLifecycle;
			var cache = SecurityCache.CacheProvider.Invoke();
			
			var results = new List<PolicyResult>();
			foreach (var policy in _policies.Select(NonLazyIfPolicyHasCacheKeyProvider()))
			{
				var strategy = GetExecutionCacheStrategyForPolicy(policy, defaultResultsCacheLifecycle);
				var cacheKey = PolicyResultCacheKeyBuilder.CreateFromStrategy(strategy, policy, context);
				
				var result = cache.Get<PolicyResult>(cacheKey, strategy.CacheLifecycle.ToLifecycle());
				if (result == null)
				{
					result = policy.Enforce(context);
					cache.Store(result, cacheKey, strategy.CacheLifecycle.ToLifecycle());
				}
				results.Add(result);
				
				if (result.ViolationOccured) break;
			}

			return results.AsReadOnly();
		}

		private static Func<ISecurityPolicy, ISecurityPolicy> NonLazyIfPolicyHasCacheKeyProvider()
		{
			return policy => policy.IsCacheKeyProvider() ? policy.EnsureNonLazyPolicy() : policy;
		}

		public IPolicyContainerConfiguration AddPolicy(ISecurityPolicy securityPolicy)
		{
			PolicyAppender.UpdatePolicies(securityPolicy, _policies);

			return this;
		}

		public IPolicyContainerConfiguration<TSecurityPolicy> AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new PolicyContainerConfigurationWrapper<TSecurityPolicy>(AddPolicy(new LazySecurityPolicy<TSecurityPolicy>()));
		}

		public IPolicyContainerConfiguration RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : class, ISecurityPolicy
		{
			IEnumerable<ISecurityPolicy> matchingPolicies;
			
			if (predicate == null)
				matchingPolicies = _policies.Where(p => p.IsPolicyOf<TSecurityPolicy>()).ToList();
			else
			{
				matchingPolicies = _policies.Where(p =>
					p.IsPolicyOf<TSecurityPolicy>() &&
					predicate.Invoke(p.EnsureNonLazyPolicyOf<TSecurityPolicy>())
					).ToList();
			}
			
			foreach (var matchingPolicy in matchingPolicies)
				_policies.Remove(matchingPolicy);

			return this;
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy
		{
			return Cache<TSecurityPolicy>(lifecycle, By.ControllerAction);
		}

		public IPolicyContainerConfiguration Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy
		{
			var policyType = typeof (TSecurityPolicy);

			var existingCacheStrategy = GetExistingCacheStrategyForPolicy(policyType);
			if (existingCacheStrategy != null) CacheStrategies.Remove(existingCacheStrategy);

			CacheStrategies.Add(new PolicyResultCacheStrategy(ControllerName, ActionName, policyType, lifecycle, level));

			return this;
		}

		public IPolicyContainerConfiguration ClearCacheStrategies()
		{
			CacheStrategies.Clear();
			return this;
		}

		public IPolicyContainerConfiguration ClearCacheStrategyFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			var existingStrategy = GetExistingCacheStrategyForPolicy(typeof (TSecurityPolicy));
			CacheStrategies.Remove(existingStrategy);
			return this;
		}

		public IEnumerable<ISecurityPolicy> GetPolicies()
		{
			return new ReadOnlyCollection<ISecurityPolicy>(_policies);
		}

		private PolicyResultCacheStrategy GetExecutionCacheStrategyForPolicy(ISecurityPolicy securityPolicy, Cache defaultResultsCacheLifecycle)
		{
			var existingStrategy = GetExistingCacheStrategyForPolicy(securityPolicy.GetType());
			return existingStrategy ?? new PolicyResultCacheStrategy(ControllerName, ActionName, securityPolicy.GetType(), defaultResultsCacheLifecycle);
		}

		private PolicyResultCacheStrategy GetExistingCacheStrategyForPolicy(Type policyType)
		{
			return CacheStrategies.SingleOrDefault(m => m.PolicyType == policyType);
		}
	}
}