using System;
using FluentSecurity.Internals;

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
			var externalServiceLocator = SecurityConfiguration.Current.Runtime.ExternalServiceLocator;
			if (externalServiceLocator != null)
			{
				var securityPolicy = externalServiceLocator.Resolve(PolicyType) as ISecurityPolicy;
				if (securityPolicy != null) return securityPolicy;
			}

			return PolicyType.HasEmptyConstructor()
				? (ISecurityPolicy)Activator.CreateInstance<TSecurityPolicy>()
				: null;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			var securityPolicy = Load();
			if (securityPolicy == null)
				throw new InvalidOperationException(
					String.Format("A policy of type {0} could not be loaded! Make sure the policy has an empty constructor or is registered in your IoC-container.", PolicyType.FullName)
					);
			
			return securityPolicy.Enforce(context);
		}
	}
}