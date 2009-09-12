using System;
using System.Security;

namespace FluentSecurity.Policy
{
	public class DenyAuthenticatedAccessPolicy : ISecurityPolicy
	{
		public void Enforce(bool isAuthenticated, object[] roles)
		{
			if (isAuthenticated)
			{
				throw new SecurityException("Authenticated access denied");
			}
		}

		public object[] RolesRequired
		{
			get { return null; }
		}
	}
}