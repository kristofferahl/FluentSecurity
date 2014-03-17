namespace FluentSecurity.Policy.ViolationHandlers.Conventions
{
	public class DefaultPolicyViolationHandlerIsOfTypeConvention<TPolicyViolationHandler> : LazyTypePolicyViolationHandlerConvention<TPolicyViolationHandler> where TPolicyViolationHandler : class, IPolicyViolationHandler {}
}