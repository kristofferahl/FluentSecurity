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

			var cacheKey = String.Concat(
				prefix,
				Separator,
				manifest.ControllerName,
				Separator,
				manifest.ActionName,
				Separator,
				policyTypeFullName
				);
			
			return cacheKey;
		}
	}
}