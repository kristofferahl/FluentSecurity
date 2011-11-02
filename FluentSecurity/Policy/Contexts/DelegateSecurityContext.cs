namespace FluentSecurity.Policy.Contexts
{
	public class DelegateSecurityContext : SecurityContextWrapper
	{
		internal DelegateSecurityContext(ISecurityPolicy policy, ISecurityContext securityContext) : base(securityContext)
		{
			Policy = policy;
		}

		public ISecurityPolicy Policy { get; private set; }
	}
}