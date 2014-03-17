namespace FluentSecurity.Policy
{
	public interface ISecurityPolicy
	{
		PolicyResult Enforce(ISecurityContext context);
	}
}