namespace FluentSecurity.Policy
{
	public class DenyAuthenticatedAccessPolicy : ISecurityPolicy
	{
		public void Enforce(ISecurityContext context)
		{
			if (context.CurrenUserAuthenticated())
			{
				throw new PolicyViolationException<DenyAuthenticatedAccessPolicy>("Authenticated access denied");
			}
		}
	}
}