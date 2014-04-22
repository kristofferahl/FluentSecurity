using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class PredicateToPolicyViolationHandlerInstanceConvention<TPolicyViolationHandler> : LazyInstancePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler
	{
		public PredicateToPolicyViolationHandlerInstanceConvention(Func<TPolicyViolationHandler> policyViolationHandlerFactory, Func<PolicyResult, bool> predicate) : base(policyViolationHandlerFactory, predicate) {}
	}
}