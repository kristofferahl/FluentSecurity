using System.Collections.ObjectModel;
using System.Linq;
using FluentSecurity.Specification.Fakes;

namespace FluentSecurity.Specification.Helpers
{
	public static class StaticHelper
	{
		public static bool IsAuthenticatedReturnsFalse()
		{
			return false;
		}

		public static bool IsAuthenticatedReturnsTrue()
		{
			return true;
		}

		public static object[] GetRolesExcludingOwner()
		{
			var roles = new Collection<object>
				{
					UserRole.Writer,
					UserRole.Publisher
				};
			return roles.ToArray();
		}

		public static object[] GetRolesIncludingOwner()
		{
			var roles = new Collection<object>
				{
					UserRole.Writer,
					UserRole.Publisher,
					UserRole.Owner
				};
			return roles.ToArray();
		}
	}
}