using System;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public static class SecurityPolicyExtensions
	{
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
	}
}