using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindByPolicyNameConvention : PolicyViolationHandlerFilterConvention
	{
		public override IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			var matchingHandler = policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForPolicyName(handler, exception));
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