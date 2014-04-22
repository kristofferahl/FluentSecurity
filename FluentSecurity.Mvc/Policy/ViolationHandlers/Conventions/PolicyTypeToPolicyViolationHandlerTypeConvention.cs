namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class PolicyTypeToPolicyViolationHandlerTypeConvention<TSecurityPolicy, TPolicyViolationHandler> : LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, ISecurityPolicyViolationHandler where TSecurityPolicy : class, ISecurityPolicy
	{
		public PolicyTypeToPolicyViolationHandlerTypeConvention() : base(policyResult => policyResult.PolicyType == typeof(TSecurityPolicy)) {}
	}
}