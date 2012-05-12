using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		dynamic Data { get; }
		bool IsCurrentUserAuthenticated();
		IEnumerable<object> CurrentUserRoles();
	}
}