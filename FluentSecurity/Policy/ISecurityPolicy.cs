namespace FluentSecurity.Policy
{
	public interface ISecurityPolicy
	{
		void Enforce(bool isAuthenticated, object[] roles);
		object[] RolesRequired { get; }
	}
}