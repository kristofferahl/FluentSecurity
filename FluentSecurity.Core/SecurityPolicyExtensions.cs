using System;
using FluentSecurity.Caching;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class SecurityPolicyExtensions
	{
		/// <summary>
		/// Returns true if the policy is of the expected type. Takes care of checking for lazy policies.
		/// </summary>
		/// <param name="securityPolicy">The policy</param>
		/// <returns>A boolean</returns>
		public static bool IsPolicyOf<TSecurityPolicy>(this ISecurityPolicy securityPolicy) where TSecurityPolicy : class, ISecurityPolicy
		{
			var isMatch = securityPolicy is TSecurityPolicy;
			if (!isMatch) isMatch = securityPolicy.GetPolicyType() == typeof(TSecurityPolicy);
			return isMatch;
		}

		/// <summary>
		/// Returns true if the policy implements ICacheKeyProvider
		/// </summary>
		/// <param name="securityPolicy">The policy</param>
		/// <returns>A boolean</returns>
		public static bool IsCacheKeyProvider(this ISecurityPolicy securityPolicy)
		{
			return typeof(ICacheKeyProvider).IsAssignableFrom(securityPolicy.GetPolicyType());
		}

		/// <summary>
		/// Gets the actual type of the ISecurityPolicy. Takes care of checking for lazy policies.
		/// </summary>
		public static Type GetPolicyType(this ISecurityPolicy securityPolicy)
		{
			var lazySecurityPolicy = securityPolicy as ILazySecurityPolicy;
			return lazySecurityPolicy != null
				? lazySecurityPolicy.PolicyType
				: securityPolicy.GetType();
		}

		/// <summary>
		/// Ensures we are working with the actual policy. Takes care of loading lazy policies.
		/// </summary>
		public static ISecurityPolicy EnsureNonLazyPolicy(this ISecurityPolicy securityPolicy)
		{
			var lazySecurityPolicy = securityPolicy as ILazySecurityPolicy;
			return lazySecurityPolicy != null
				? lazySecurityPolicy.Load()
				: securityPolicy;
		}

		/// <summary>
		/// Ensures we are working with the expected policy type. Takes care of loading and casting lazy policies.
		/// </summary>
		public static TSecurityPolicy EnsureNonLazyPolicyOf<TSecurityPolicy>(this ISecurityPolicy securityPolicy) where TSecurityPolicy : class, ISecurityPolicy
		{
			return securityPolicy.EnsureNonLazyPolicy() as TSecurityPolicy;
		}
	}
}