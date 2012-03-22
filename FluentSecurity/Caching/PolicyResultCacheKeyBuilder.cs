using System;
using FluentSecurity.Policy;

namespace FluentSecurity.Caching
{
	public static class PolicyResultCacheKeyBuilder
	{
		const string Separator = "_";

		public static string CreateFromManifest(CacheManifest manifest, ISecurityPolicy securityPolicy, ISecurityContext context)
		{
			var prefix = typeof(PolicyResult).Name;
			var policyTypeFullName = manifest.PolicyType.FullName;
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

			var cacheKey = String.Concat(
				prefix,
				Separator,
				manifest.ControllerName,
				Separator,
				manifest.ActionName,
				Separator,
				policyTypeFullName,
				customPolicyCacheKey
				);
			
			return cacheKey;
		}
	}
}