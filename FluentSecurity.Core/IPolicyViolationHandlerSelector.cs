namespace FluentSecurity
{
	public interface IPolicyViolationHandlerSelector<out TResult>
	{
		ISecurityPolicyViolationHandler<TResult> FindHandlerFor(PolicyViolationException exception);
	}
}