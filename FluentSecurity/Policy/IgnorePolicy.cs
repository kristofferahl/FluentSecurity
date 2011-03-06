namespace FluentSecurity.Policy
{
	public class IgnorePolicy : ISecurityPolicy
	{
		public void Enforce(ISecurityContext context) {}
	}
}