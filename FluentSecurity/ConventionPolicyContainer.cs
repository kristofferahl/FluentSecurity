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
			if (policyContainers == null || policyContainers.Count == 0) throw new ArgumentException("A list of policycontainers was not provided", "policyContainers");
			
			_policyContainers = policyContainers;
		}

		public IConventionPolicyContainer AddPolicy(ISecurityPolicy securityPolicy)
		{
			foreach (var policyContainer in _policyContainers)
			{
				policyContainer.AddPolicy(securityPolicy);
			}
			return this;
		}
	}
}