namespace FluentSecurity.Policy
{
	public class DelegatePolicy : ISecurityPolicy
	{
		public string Name { get; private set; }

		public PolicyResult Enforce(ISecurityContext context)
		{
			throw new System.NotImplementedException();
		}
	}
}