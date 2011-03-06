namespace FluentSecurity.Policy
{
	public class DenyAnonymousAccessPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			if (context.CurrenUserAuthenticated() == false)
			{
				return PolicyResult.CreateFailureResult(this, "Anonymous access denied");
			}
			return PolicyResult.CreateSuccessResult(this);
		}
	}
}