namespace FluentSecurity.Policy
{
	public class IgnorePolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			return PolicyResult.CreateSuccessResult(this);
		}
	}
}