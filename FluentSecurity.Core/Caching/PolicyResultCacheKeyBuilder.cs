using System;
using FluentSecurity.Policy;

namespace FluentSecurity.Caching
{
	public static class PolicyResultCacheKeyBuilder
	{
		const string Separator = "_";

		public static string CreateFromStrategy(PolicyResultCacheStrategy strategy, ISecurityPolicy securityPolicy, ISecurityContext context)
		{
			var policyCacheKey = BuildPolicyCacheKey(strategy, securityPolicy, context);
			var cacheKey = BuildCacheKey(strategy, policyCacheKey);
			return cacheKey;
		}

		private static string BuildPolicyCacheKey(PolicyResultCacheStrategy strategy, ISecurityPolicy securityPolicy, ISecurityContext context)
		{
			var customPolicyCacheKey = String.Empty;

			var cacheKeyProvider = securityPolicy as ICacheKeyProvider;
			if (cacheKeyProvider != null)
			{
				customPolicyCacheKey = cacheKeyProvider.Get(context);
				if (customPolicyCacheKey != null)
				{
					while (customPolicyCacheKey.StartsWith(" ") || customPolicyCacheKey.EndsWith(" "))
						customPolicyCacheKey = customPolicyCacheKey.Trim();

					if (!String.IsNullOrWhiteSpace(customPolicyCacheKey))
						customPolicyCacheKey = String.Concat(Separator, customPolicyCacheKey);
				}
			}

			return String.Concat(strategy.PolicyType.FullName, customPolicyCacheKey);
		}

		private static string BuildCacheKey(PolicyResultCacheStrategy strategy, string policyCacheKey)
		{
			string cacheKey;
			switch (strategy.CacheLevel)
			{
				case By.Controller:
					cacheKey = String.Concat(strategy.ControllerName, Separator, "*", Separator, policyCacheKey);
					break;
				case By.ControllerAction:
					cacheKey = String.Concat(strategy.ControllerName, Separator, strategy.ActionName, Separator, policyCacheKey);
					break;
				default: // Policy
					cacheKey = String.Concat("*", Separator, "*", Separator, policyCacheKey);
					break;
			}
			return String.Concat(typeof(PolicyResult).Name, Separator, cacheKey);
		}
	}
}