using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindConfiguredDefaultPolicyViolationHandlerConvention : IPolicyViolationHandlerConvention
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public FindConfiguredDefaultPolicyViolationHandlerConvention(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			if (policyViolationHandlers == null) throw new ArgumentNullException("policyViolationHandlers");
			_policyViolationHandlers = policyViolationHandlers;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			var handlerType = SecurityConfiguration.Current.DefaultPolicyViolationHandler;
			if (handlerType == null) return null;
			
			var matchingHandler = _policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForName(handler, handlerType));
			return matchingHandler;
		}

		private static bool HandlerIsMatchForName(IPolicyViolationHandler handler, Type expectedHandlerType)
		{
			return handler.GetType() == expectedHandlerType;
		}
	}
}