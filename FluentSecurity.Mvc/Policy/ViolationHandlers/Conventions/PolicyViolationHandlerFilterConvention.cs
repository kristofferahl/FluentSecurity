using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Configuration;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention
	{
		public Func<IEnumerable<ISecurityPolicyViolationHandler>> PolicyViolationHandlerProvider = () => SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.ResolveAll<ISecurityPolicyViolationHandler>();

		public abstract ISecurityPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<ISecurityPolicyViolationHandler> policyViolationHandlers);
		
		public object GetHandlerFor(PolicyViolationException exception)
		{
			var policyViolationHandlers = PolicyViolationHandlerProvider.Invoke().ToList();
			return GetHandlerFor(exception, policyViolationHandlers);
		}
	}
}