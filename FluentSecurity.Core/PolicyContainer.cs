using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using FluentSecurity.Caching;
using FluentSecurity.Configuration;
using FluentSecurity.Diagnostics;
using FluentSecurity.Internals;
using FluentSecurity.Policy;
using FluentSecurity.ServiceLocation;

namespace FluentSecurity
{
	public class PolicyContainer : IPolicyContainer, IPolicyContainerConfiguration
	{
		internal IPolicyAppender PolicyAppender;
		internal readonly List<PolicyResultCacheStrategy> CacheStrategies;

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
		}

		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }

		public IEnumerable<PolicyResult> EnforcePolicies(ISecurityContext context)
		{
			if (_policies.Count.Equals(0))
				throw new ConfigurationErrorsException("You must add at least 1 policy for controller {0} action {1}.".FormatWith(ControllerName, ActionName));

			var defaultResultsCacheLifecycle = context.Runtime.DefaultResultsCacheLifecycle;
			var cache = context.Runtime.Cache;
			
			var results = new List<PolicyResult>();
			foreach (var securityPolicy in _policies.Select(NonLazyIfPolicyHasCacheKeyProvider()))
			{
				var policy = securityPolicy;
				var strategy = GetExecutionCacheStrategyForPolicy(policy, defaultResultsCacheLifecycle);
				var cacheKey = PolicyResultCacheKeyBuilder.CreateFromStrategy(strategy, policy, context);
				
				var policyResult = Publish.RuntimePolicyEvent(() =>
				{
					var result = cache.Get<PolicyResult>(cacheKey, strategy.CacheLifecycle.ToLifecycle());
					if (result == null)
					{
						result = policy.Enforce(context);
						cache.Store(result, cacheKey, strategy.CacheLifecycle.ToLifecycle());
					}
					else result.Cached = true;
					return result;
				}, r => CreateMessageForResult(r, strategy, cacheKey), context);

				results.Add(policyResult);

				if (policyResult.ViolationOccured) break;
			}

			return results.AsReadOnly();
		}

		private static string CreateMessageForResult(PolicyResult result, PolicyResultCacheStrategy strategy, string cacheKey)
		{
			return "Enforced policy {0} - {1}! \r\n{2}: {3} at {4} with key '{5}'".FormatWith(
				result.PolicyType.FullName,
				result.ViolationOccured ? "Violation occured" : "Success",
				result.Cached ? "Cached. Strategy" : "Strategy",
				strategy.CacheLifecycle,
				strategy.CacheLevel,
				cacheKey
				);
		}

		private static Func<ISecurityPolicy, ISecurityPolicy> NonLazyIfPolicyHasCacheKeyProvider()
		{
			return policy => policy.IsCacheKeyProvider() ? policy.EnsureNonLazyPolicy() : policy;
		}

		public IPolicyContainerConfiguration AddPolicy(ISecurityPolicy securityPolicy)
		{
			Publish.ConfigurationEvent(() => "Updating policies for {0} action {1} using {2}.".FormatWith(ControllerName, ActionName, PolicyAppender.GetType().FullName));

			var policiesBeforeUpdate = new ISecurityPolicy[_policies.Count];
			_policies.CopyTo(policiesBeforeUpdate, 0);

			PolicyAppender.UpdatePolicies(securityPolicy, _policies);

			var policiesRemoved = policiesBeforeUpdate.Except(_policies).ToList();
			policiesRemoved.ForEach(p => Publish.ConfigurationEvent(() => "- Removed policy {0} [{1}].".FormatWith(p.GetPolicyType().FullName, p is ILazySecurityPolicy ? "Lazy" : "Instance")));

			var policiesAdded = _policies.Except(policiesBeforeUpdate).ToList();
			policiesAdded.ForEach(p => Publish.ConfigurationEvent(() => "- Added policy {0} [{1}].".FormatWith(p.GetPolicyType().FullName, p is ILazySecurityPolicy ? "Lazy" : "Instance")));

			return this;
		}

		public IPolicyContainerConfiguration<TSecurityPolicy> AddPolicy<TSecurityPolicy>() where TSecurityPolicy : ISecurityPolicy
		{
			return new PolicyContainerConfigurationWrapper<TSecurityPolicy>(AddPolicy(new LazySecurityPolicy<TSecurityPolicy>()));
		}

		public IPolicyContainerConfiguration RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : class, ISecurityPolicy
		{
			Publish.ConfigurationEvent(() => "Removing policies from {0} action {1}.".FormatWith(ControllerName, ActionName));

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
			{
				var policy = matchingPolicy;
				_policies.Remove(matchingPolicy);
				Publish.ConfigurationEvent(() => "- Removed policy {0} [{1}].".FormatWith(policy.GetPolicyType().FullName, policy is ILazySecurityPolicy ? "Lazy" : "Instance"));
			}

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

		public override string ToString()
		{
			return String.Format("{0} - {1} - {2}", base.ToString(), ControllerName, ActionName);
		}
	}
}