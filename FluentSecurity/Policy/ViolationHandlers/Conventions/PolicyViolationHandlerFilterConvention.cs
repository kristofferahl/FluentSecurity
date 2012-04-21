using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public abstract class PolicyViolationHandlerFilterConvention : IPolicyViolationHandlerConvention
	{
		public abstract IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<IPolicyViolationHandler> policyViolationHandlers);
		
		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			return GetHandlerFor(exception, ServiceLocation.ServiceLocator.Current.ResolveAll<IPolicyViolationHandler>().ToList());
		}
	}
}