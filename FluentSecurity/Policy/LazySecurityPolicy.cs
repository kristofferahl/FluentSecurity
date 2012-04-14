using System;

namespace FluentSecurity.Policy
{
	internal class LazySecurityPolicy<TSecurityPolicy> : ILazySecurityPolicy where TSecurityPolicy : ISecurityPolicy
	{
		public Type PolicyType
		{
			get { return typeof (TSecurityPolicy); }
		}

		public ISecurityPolicy Load()
		{
			return PolicyType.HasEmptyConstructor()
				? (ISecurityPolicy) Activator.CreateInstance<TSecurityPolicy>()
				: null;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			var securityPolicy = Load();
			if (securityPolicy == null)
				throw new InvalidOperationException(
					String.Format("A policy of type {0} could not be loaded! Make sure the policy has an empty constructor.", PolicyType.FullName)
					);
			
			return securityPolicy.Enforce(context);
		}
	}
}