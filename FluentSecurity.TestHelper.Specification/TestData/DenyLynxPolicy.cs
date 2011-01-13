using System.Security;
using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class DenyLynxPolicy : ISecurityPolicy
	{
		public void Enforce(bool isAuthenticated, object[] roles)
		{
			const bool isLynx = true;
			if (isLynx)
			{
				throw new SecurityException("Access to this section is restricted for Lynx. Please switch to another browser!");
			}
		}

		public object[] RolesRequired
		{
			get { return null; }
		}
	}
}