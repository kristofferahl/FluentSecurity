using FluentSecurity.Policy;

namespace FluentSecurity.Specification.Helpers
{
	public class FakePolicy : IgnorePolicy, ISecurityPolicy
	{
		public new virtual void Enforce(bool isAuthenticated, object[] roles)
		{
		}

		public new object[] RolesRequired
		{
			get { return null; }
		}
	}
}