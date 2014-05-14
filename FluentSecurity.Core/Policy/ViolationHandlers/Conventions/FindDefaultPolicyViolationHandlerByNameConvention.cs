using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindDefaultPolicyViolationHandlerByNameConvention : PolicyViolationHandlerFilterConvention
	{
		public override ISecurityPolicyViolationHandler GetHandlerFor<TViolationHandlerType>(PolicyViolationException exception, IEnumerable<TViolationHandlerType> policyViolationHandlers)
		{
			var matchingHandler = policyViolationHandlers.SingleOrDefault(HandlerIsDefaultPolicyViolationHandler);
			return matchingHandler;
		}

		private static bool HandlerIsDefaultPolicyViolationHandler<TViolationHandlerType>(TViolationHandlerType handler)
		{
			var actualHandlerName = handler.GetType().Name;
			return actualHandlerName == "DefaultPolicyViolationHandler";
		}
	}
}