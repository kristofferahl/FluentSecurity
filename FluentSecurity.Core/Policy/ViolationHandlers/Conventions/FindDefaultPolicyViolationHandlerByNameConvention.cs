using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindDefaultPolicyViolationHandlerByNameConvention : PolicyViolationHandlerFilterConvention
	{
		public override ISecurityPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<ISecurityPolicyViolationHandler> policyViolationHandlers)
		{
			var matchingHandler = policyViolationHandlers.SingleOrDefault(HandlerIsDefaultPolicyViolationHandler);
			return matchingHandler;
		}

		private static bool HandlerIsDefaultPolicyViolationHandler(ISecurityPolicyViolationHandler handler)
		{
			var actualHandlerName = handler.GetType().Name;
			return actualHandlerName == "DefaultPolicyViolationHandler";
		}
	}
}