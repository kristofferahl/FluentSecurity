using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class PolicyViolationHandlerSelector : IPolicyViolationHandlerSelector
	{
		private readonly IList<IPolicyViolationHandlerConvention> _conventions;

		public PolicyViolationHandlerSelector(IEnumerable<IPolicyViolationHandler> policyViolationHandlers)
		{
			policyViolationHandlers = policyViolationHandlers.ToList();
			
			_conventions = new List<IPolicyViolationHandlerConvention>
			{
				new FindByPolicyNameConvention(policyViolationHandlers),
				new FindDefaultPolicyViolationHandlerConvention(policyViolationHandlers)
			};
		}

		public IPolicyViolationHandler FindHandlerFor(PolicyViolationException exception)
		{
			IPolicyViolationHandler matchingHandler = null;
			foreach (var convention in _conventions)
			{
				matchingHandler = convention.GetHandlerFor(exception);
				if (matchingHandler != null) break;
			}
			return matchingHandler;
		}
	}
}