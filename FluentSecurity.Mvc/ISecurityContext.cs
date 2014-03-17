using System;
using System.Collections.Generic;

namespace FluentSecurity
{
	public interface ISecurityContext
	{
		Guid Id { get; }
		dynamic Data { get; }
		bool CurrentUserIsAuthenticated();
		IEnumerable<object> CurrentUserRoles();
		ISecurityRuntime Runtime { get; }
	}
}