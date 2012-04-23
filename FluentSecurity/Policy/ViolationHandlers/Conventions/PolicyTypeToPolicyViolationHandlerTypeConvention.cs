namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class PolicyTypeToPolicyViolationHandlerTypeConvention<TSecurityPolicy, TPolicyViolationHandler> : LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, IPolicyViolationHandler
	{
		public PolicyTypeToPolicyViolationHandlerTypeConvention() : base(policyResult => policyResult.PolicyType == typeof(TSecurityPolicy)) {}
	}
}