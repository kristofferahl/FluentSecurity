namespace FluentSecurity.Policy
{
	public interface ISecurityPolicy
	{
		void Enforce(ISecurityContext context);
	}
}