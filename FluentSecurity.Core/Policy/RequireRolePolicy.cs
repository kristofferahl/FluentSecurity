using System;

namespace FluentSecurity.Policy
{
	[Obsolete("Use RequireAnyRolesPolicy instead.")]
	public class RequireRolePolicy : RequireAnyRolePolicy
	{
		public RequireRolePolicy(params object[] requiredRoles) : base(requiredRoles) {}
	}
}