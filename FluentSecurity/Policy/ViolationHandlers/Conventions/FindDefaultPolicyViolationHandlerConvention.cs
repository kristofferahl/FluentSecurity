using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindDefaultPolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public FindDefaultPolicyViolationHandlerConvention(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			if (policyViolationHandlers == null) throw new ArgumentNullException("policyViolationHandlers");
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var matchingHandler = _policyViolationHandlers.SingleOrDefault(HandlerIsDefaultPolicyViolationHandler);
			return matchingHandler;
		}

		private static bool HandlerIsDefaultPolicyViolationHandler(IPolicyViolationHandler handler)
		{
			var actualHandlerName = handler.GetType().Name;
			return actualHandlerName == "DefaultPolicyViolationHandler";
		}
	}
}