using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSecurity.Policy
{
	public class RequireRolePolicy : ISecurityPolicy
	{
		private readonly IEnumerable<object> _requiredRoles;

		public RequireRolePolicy(params object[] requiredRoles)
		{
			if (requiredRoles == null)
				throw new ArgumentNullException("requiredRoles", "Required roles must not be null");

			if (requiredRoles.Length == 0)
				throw new ArgumentException("Required roles must be specified");

			_requiredRoles = requiredRoles;
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			if (context.CurrenUserAuthenticated() == false)
				return PolicyResult.CreateFailureResult(this, "Anonymous access denied");

			if (context.CurrenUserRoles() == null || context.CurrenUserRoles().Any() == false)
				return PolicyResult.CreateFailureResult(this, "Access denied");

			if (context.CurrenUserRoles().Any(role => _requiredRoles.Contains(role)) == false)
			{
				const string message = "Access requires one of the following roles: {0}.";
				var formattedMessage = string.Format(message, GetRoles());
				return PolicyResult.CreateFailureResult(this, formattedMessage);
			}

			return PolicyResult.CreateSuccessResult(this);
		}

		public IEnumerable<object> RolesRequired
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

		public static bool operator ==(RequireRolePolicy left, RequireRolePolicy right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(RequireRolePolicy left, RequireRolePolicy right)
		{
			return !Equals(left, right);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = typeof(RequireRolePolicy).GetHashCode();
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

		public override string ToString()
		{
			var name = base.ToString();
			var roles = GetRoles();
			return String.IsNullOrEmpty(roles) ? name : String.Concat(name, " (", roles, ")");
		}

		private string GetRoles()
		{
			var builder = new StringBuilder();
			
			foreach (var requiredRole in _requiredRoles)
				builder.AppendFormat("{0} or ", requiredRole);

			return builder.ToString(0, builder.Length - 4);
		}
	}
}