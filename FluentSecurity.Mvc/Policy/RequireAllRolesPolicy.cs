using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentSecurity.Policy
{
	public class RequireAllRolesPolicy : ISecurityPolicy
	{
		private readonly IEnumerable<object> _requiredRoles;

		public RequireAllRolesPolicy(params object[] requiredRoles)
		{
			if (requiredRoles == null)
				throw new ArgumentNullException("requiredRoles", "Required roles must not be null");

			if (requiredRoles.Length == 0)
				throw new ArgumentException("Required roles must be specified");

			_requiredRoles = requiredRoles;
		}

		public IEnumerable<object> RolesRequired
		{
			get { return _requiredRoles; }
		}

		public PolicyResult Enforce(ISecurityContext context)
		{
			if (context.CurrentUserIsAuthenticated() == false)
				return PolicyResult.CreateFailureResult(this, "Anonymous access denied");

			var currentUserRoles = context.CurrentUserRoles().EnsureIsList();
			if (currentUserRoles.Any() == false)
				return PolicyResult.CreateFailureResult(this, "Access denied");

			if (_requiredRoles.Any(requiredRole => currentUserRoles.Contains(requiredRole) == false))
			{
				const string message = "Access requires all of the following roles: {0}.";
				var formattedMessage = string.Format(message, GetRoles());
				return PolicyResult.CreateFailureResult(this, formattedMessage);
			}

			return PolicyResult.CreateSuccessResult(this);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as RequireAllRolesPolicy);
		}

		public bool Equals(RequireAllRolesPolicy other)
		{
			if (ReferenceEquals(null, other)) return false;
			if (RolesRequired.Count() != other.RolesRequired.Count()) return false;
			return RolesRequired.All(role => other.RolesRequired.Contains(role));
		}

		public static bool operator ==(RequireAllRolesPolicy left, RequireAllRolesPolicy right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(RequireAllRolesPolicy left, RequireAllRolesPolicy right)
		{
			return !Equals(left, right);
		}

		public override int GetHashCode()
		{
			unchecked
			{
				var hash = typeof(RequireAllRolesPolicy).GetHashCode();
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
				builder.AppendFormat("{0} and ", requiredRole);

			return builder.ToString(0, builder.Length - 5);
		}
	}
}