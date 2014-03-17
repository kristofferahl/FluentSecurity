namespace FluentSecurity
{
	public interface ISecurityPolicyViolationHandler<out TResult>
	{
		TResult Handle(PolicyViolationException exception);
	}
}