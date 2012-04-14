namespace FluentSecurity.Policy
{
	internal class LazyPolicy<TSecurityPolicy> : ISecurityPolicy where TSecurityPolicy : ISecurityPolicy
	{
		public PolicyResult Enforce(ISecurityContext context)
		{
			throw new System.NotImplementedException();
		}
	}
}