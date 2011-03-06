namespace FluentSecurity
{
	public interface ISecurityContext
	{
		bool CurrenUserAuthenticated();
		object[] CurrenUserRoles();
	}
}