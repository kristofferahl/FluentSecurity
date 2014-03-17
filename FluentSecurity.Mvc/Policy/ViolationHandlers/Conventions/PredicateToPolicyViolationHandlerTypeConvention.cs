using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class PredicateToPolicyViolationHandlerTypeConvention<TPolicyViolationHandler> : LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		public PredicateToPolicyViolationHandlerTypeConvention(Func<PolicyResult, bool> predicate) : base(predicate) {}
	}
}