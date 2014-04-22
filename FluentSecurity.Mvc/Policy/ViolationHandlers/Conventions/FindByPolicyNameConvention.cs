using System.Collections.Generic;
using System.Linq;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class FindByPolicyNameConvention : PolicyViolationHandlerFilterConvention
	{
		public override ISecurityPolicyViolationHandler GetHandlerFor(PolicyViolationException exception, IEnumerable<ISecurityPolicyViolationHandler> policyViolationHandlers)
		{
			var matchingHandler = policyViolationHandlers.SingleOrDefault(handler => HandlerIsMatchForPolicyName(handler, exception));
			return matchingHandler;
		}

		private static bool HandlerIsMatchForPolicyName(ISecurityPolicyViolationHandler handler, PolicyViolationException exception)
		{
			var expectedHandlerName = "{0}ViolationHandler".FormatWith(exception.PolicyType.Name);
			var actualHandlerName = handler.GetType().Name;
			return expectedHandlerName == actualHandlerName;
		}
	}
}