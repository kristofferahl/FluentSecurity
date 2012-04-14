using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer
	{
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
		public IPolicyAppender PolicyAppender { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context)
		{
			if (_policies.Count.Equals(0))
				throw ExceptionFactory.CreateConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var defaultResultsCacheLifecycle = SecurityConfigurationProvider.Invoke().Advanced.DefaultResultsCacheLifecycle;
			var cache = SecurityCache.CacheProvider.Invoke();
			
			var results = new List<PolicyResult>();
			foreach (var policy in _policies)
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

		public IPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			PolicyAppender.UpdatePolicies(securityPolicy, _policies);

			return this;
		}

		public IPolicyContainer AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return AddPolicy(new LazyPolicy<TSecurityPolicy>());
		}

		public IPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy
		{
			if (predicate == null)
				predicate = x => true;

			var matchingPolicies = _policies.Where(p =>
				p is TSecurityPolicy &&
				predicate.Invoke((TSecurityPolicy)p)
				).ToList();
			
			foreach (var matchingPolicy in matchingPolicies)
				_policies.Remove(matchingPolicy);

			return this;
		}

		public IPolicyContainer Cache<TSecurityPolicy>(Cache lifecycle) where TSecurityPolicy : ISecurityPolicy
		{
			return Cache<TSecurityPolicy>(lifecycle, By.ControllerAction);
		}

		public IPolicyContainer Cache<TSecurityPolicy>(Cache lifecycle, By level) where TSecurityPolicy : ISecurityPolicy
		{
			var policyType = typeof (TSecurityPolicy);

			var existingCacheStrategy = GetExistingCacheStrategyForPolicy(policyType);
			if (existingCacheStrategy != null) CacheStrategies.Remove(existingCacheStrategy);

			CacheStrategies.Add(new PolicyResultCacheStrategy(ControllerName, ActionName, policyType, lifecycle, level));

			return this;
		}

		public IPolicyContainer ClearCacheStrategies()
		{
			CacheStrategies.Clear();
			return this;
		}

		public IPolicyContainer ClearCacheStrategyFor<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
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