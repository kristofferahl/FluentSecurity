using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Configuration;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention
	{
		public Func<IEnumerable<IPolicyViolationHandler>> PolicyViolationHandlerProvider = () => SecurityConfiguration.Get<MvcConfiguration>().ServiceLocator.ResolveAll<IPolicyViolationHandler>();

		public abstract IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<IPolicyViolationHandler> policyViolationHandlers);
		
		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var policyViolationHandlers = PolicyViolationHandlerProvider.Invoke().ToList();
			return GetHandlerFor(exception, policyViolationHandlers);
		}
	}
}