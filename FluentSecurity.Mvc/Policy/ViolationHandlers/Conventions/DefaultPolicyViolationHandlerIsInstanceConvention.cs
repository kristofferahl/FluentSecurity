using System;

namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class DefaultPolicyViolationHandlerIsInstanceConvention<TPolicyViolationHandler> : LazyInstancePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler
	{
		public DefaultPolicyViolationHandlerIsInstanceConvention(Func<TPolicyViolationHandler> policyViolationHandler) : base(policyViolationHandler) {}
	}
}