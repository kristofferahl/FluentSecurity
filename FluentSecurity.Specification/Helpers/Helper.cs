namespace FluentSecurity.Specification.Helpers
{
	public class Helper
	{
		public virtual bool IsAuthenticatedReturnsFalse()
		{
			return false;
		}

		public virtual bool IsAuthenticatedReturnsTrue()
		{
			return true;
		}

		public virtual object[] GetRoles()
		{
			return StaticHelper.GetRolesIncludingOwner();
		}
	}
}