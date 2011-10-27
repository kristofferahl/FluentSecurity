using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Policy.Results;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class DelegatePolicyViolationHandler : IPolicyViolationHandler
	{
		private readonly IEnumerable<IPolicyViolationHandler> _policyViolationHandlers;

		public DelegatePolicyViolationHandler(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			_policyViolationHandlers = policyViolationHandlers;
		}

		public ActionResult Handle(PolicyViolationException exception)
		{
			var delegatePolicyResult = (DelegatePolicyResult)exception.PolicyResult;
			
			if (delegatePolicyResult.ViolationHandler != null)
				return delegatePolicyResult.ViolationHandler.Invoke(exception);

			var matchingViolationHandler = _policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForException(handler, delegatePolicyResult.PolicyName));
			if (matchingViolationHandler != null)
				return matchingViolationHandler.Handle(exception);

			throw exception;
		}

		private static bool HandlerIsMatchForException(IPolicyViolationHandler handler, string policyName)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(policyName);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}