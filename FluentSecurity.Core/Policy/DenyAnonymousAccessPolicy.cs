namespace FluentSecurity.Policy
{
	public class DenyAnonymousAccessPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			if (context.CurrentUserIsAuthenticated() == false)
			{
				return PolicyResult.CreateFailureResult(this, "Anonymous access denied");
			}
			return PolicyResult.CreateSuccessResult(this);
		}
	}
}