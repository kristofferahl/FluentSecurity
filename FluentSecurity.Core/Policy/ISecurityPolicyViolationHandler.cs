namespace FluentSecurity
{
	public interface ISecurityPolicyViolationHandler<out TResult> : ISecurityPolicyViolationHandler
	{
		TResult Handle(PolicyViolationException exception);
	}

	public interface ISecurityPolicyViolationHandler {}
}