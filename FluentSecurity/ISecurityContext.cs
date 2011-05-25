using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		bool CurrenUserAuthenticated();
		IEnumerable<object> CurrenUserRoles();
	}
}