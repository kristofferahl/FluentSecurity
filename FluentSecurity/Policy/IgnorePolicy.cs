namespace FluentSecurity.Policy
{
	public class IgnorePolicy : ISecurityPolicy
	{
		public void Enforce(bool isAuthenticated, object[] roles) {}

		public object[] RolesRequired
		{
			get { return null; }
		}
	}
}