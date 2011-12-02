using System;
using System.Collections.Generic;
using FluentSecurity.Policy;

namespace FluentSecurity
{
	public class ConventionPolicyContainer : IConventionPolicyContainer
	{
		private readonly IList<IPolicyContainer> _policyContainers;

		public ConventionPolicyContainer(IList<IPolicyContainer> policyContainers)
		{
			if (policyContainers == null)
				throw new ArgumentNullException("policyContainers", "A list of policycontainers was not provided");
			
			_policyContainers = policyContainers;
		}

		public IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.AddPolicy(securityPolicy);
			
			return this;
		}

		public IConventionPolicyContainer RemovePolicy<TSecurityPolicy>(Func<TSecurityPolicy, bool> predicate = null) where TSecurityPolicy : ISecurityPolicy
		{
			foreach (var policyContainer in _policyContainers)
				policyContainer.RemovePolicy(predicate);

			return this;
		}
	}
}