using System;

namespace FluentSecurity
{
	public class SecurityContext : ISecurityContext
	{
		private readonly Func<bool> _isAuthenticated;
		private readonly Func<object[]> _roles;

		public SecurityContext(Func<bool> isAuthenticated, Func<object[]> roles)
		{
			if (isAuthenticated == null) throw new ArgumentNullException("isAuthenticated");

			_isAuthenticated = isAuthenticated;
			_roles = roles;
		}

		public bool CurrenUserAuthenticated()
		{
			return _isAuthenticated();
		}

		public object[] CurrenUserRoles()
		{
			return _roles != null ? _roles() : null;
		}
	}
}