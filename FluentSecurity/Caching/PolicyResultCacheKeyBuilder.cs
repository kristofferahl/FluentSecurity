using FluentSecurity.Policy;

namespace FluentSecurity.Caching
{
	public static class PolicyResultCacheKeyBuilder
	{
		public static string CreateFromPolicy(ISecurityPolicy policy, ISecurityContext context)
		{
			var cacheKey = policy.GetType().FullName;
			return cacheKey;
		}
	}
}