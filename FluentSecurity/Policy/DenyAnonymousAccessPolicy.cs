namespace FluentSecurity.Policy
{
	public class DenyAnonymousAccessPolicy : ISecurityPolicy
	{
		public void Enforce(ISecurityContext context)
		{
			if (context.CurrenUserAuthenticated() == false)
			{
				throw new PolicyViolationException<DenyAnonymousAccessPolicy>("Anonymous access denied");
			}
		}
	}
}