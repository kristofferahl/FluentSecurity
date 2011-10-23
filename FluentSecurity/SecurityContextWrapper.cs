using System.Collections.Generic;

namespace FluentSecurity
{
	public class SecurityContextWrapper : ISecurityContext
	{
		private readonly ISecurityContext _securityContext;

		public SecurityContextWrapper(ISecurityContext securityContext)
		{
			_securityContext = securityContext;
		}

		public bool CurrenUserAuthenticated()
		{
			return _securityContext.CurrenUserAuthenticated();
		}

		public IEnumerable<object> CurrenUserRoles()
		{
			return _securityContext.CurrenUserRoles();
		}
	}
}