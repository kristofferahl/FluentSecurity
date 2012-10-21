using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		dynamic Data { get; }
		bool CurrentUserIsAuthenticated();
		IEnumerable<object> CurrentUserRoles();
	}
}