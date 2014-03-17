using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention
	{
		public Func<IEnumerable<IPolicyViolationHandler>> PolicyViolationHandlerProvider = () => ServiceLocation.ServiceLocator.Current.ResolveAll<IPolicyViolationHandler>();

		public abstract IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<IPolicyViolationHandler> policyViolationHandlers);
		
		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var policyViolationHandlers = PolicyViolationHandlerProvider.Invoke().ToList();
			return GetHandlerFor(exception, policyViolationHandlers);
		}
	}
}