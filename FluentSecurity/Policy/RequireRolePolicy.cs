using System;
using System.Linq;

namespace FluentSecurity.Policy
{
	public class RequireRolePolicy : ISecurityPolicy
	{
		private readonly object[] _requiredRoles;

		public RequireRolePolicy(params object[] requiredRoles)
		{
			if (requiredRoles == null)
				throw new ArgumentException("Required roles must not be null");

			if (requiredRoles.Length == 0)
				throw new ArgumentException("Required roles must be specified");

			_requiredRoles = requiredRoles;
		}

		public void Enforce(bool isAuthenticated, object[] roles)
		{
			if (isAuthenticated == false)
				throw new PolicyViolationException<RequireRolePolicy>("Anonymous access denied");

			if (roles == null || roles.Length == 0)
				throw new PolicyViolationException<RequireRolePolicy>("Access denied");

			foreach (var requiredRole in _requiredRoles)
			{
				foreach (var role in roles)
				{
					if (requiredRole.ToString() == role.ToString())
					{
						return;
					}
				}
			}

			const string message = "Access requires one of the following roles: {0}";
			var formattedMessage = string.Format(message, roles);
			throw new PolicyViolationException<RequireRolePolicy>(formattedMessage);
		}

		public object[] RolesRequired
		{
			get { return _requiredRoles; }
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RequireRolePolicy);
		}

		public bool Equals(RequireRolePolicy other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (RolesRequired.Count() != other.RolesRequired.Count()) return false;
			return RolesRequired.All(role => other.RolesRequired.Contains(role));
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				if (RolesRequired != null)
				{
					hash = RolesRequired.Aggregate(
						hash,
						(current, role) => current * 23 + ((role != null) ? role.GetHashCode() : 0)
						);
				}
				return hash;
			}
		}
	}
}