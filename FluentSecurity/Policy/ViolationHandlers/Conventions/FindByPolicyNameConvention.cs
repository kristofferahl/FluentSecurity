using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindByPolicyNameConvention : IPolicyViolationHandlerConvention
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public FindByPolicyNameConvention(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			if (policyViolationHandlers == null) throw new ArgumentNullException("policyViolationHandlers");
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var matchingHandler = _policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForPolicyName(handler, exception));
			return matchingHandler;
		}

		private static bool HandlerIsMatchForPolicyName(IPolicyViolationHandler handler, PolicyViolationException exception)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(exception.PolicyType.Name);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}