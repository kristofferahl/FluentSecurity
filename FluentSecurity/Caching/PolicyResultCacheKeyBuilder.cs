using System;
using FluentSecurity.Policy;

namespace FluentSecurity.Caching
{
	public static class PolicyResultCacheKeyBuilder
	{
		const string Separator = "_";

		public static string CreateFromManifest(PolicyResultCacheManifest manifest, ISecurityPolicy securityPolicy, ISecurityContext context)
		{
			var policyCacheKey = BuildPolicyCacheKey(manifest, securityPolicy, context);
			var cacheKey = BuildCacheKey(manifest, policyCacheKey);
			return cacheKey;
		}

		private static string BuildPolicyCacheKey(PolicyResultCacheManifest manifest, ISecurityPolicy securityPolicy, ISecurityContext context)
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

			return String.Concat(manifest.PolicyType.FullName, customPolicyCacheKey);
		}

		private static string BuildCacheKey(PolicyResultCacheManifest manifest, string policyCacheKey)
		{
			string cacheKey;
			switch (manifest.CacheLevel)
			{
				case By.Controller:
					cacheKey = String.Concat(manifest.ControllerName, Separator, "*", Separator, policyCacheKey);
					break;
				case By.ControllerAction:
					cacheKey = String.Concat(manifest.ControllerName, Separator, manifest.ActionName, Separator, policyCacheKey);
					break;
				default: // Policy
					cacheKey = String.Concat("*", Separator, "*", Separator, policyCacheKey);
					break;
			}
			return String.Concat(typeof(PolicyResult).Name, Separator, cacheKey);
		}
	}
}