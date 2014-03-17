namespace FluentSecurity.Caching
{
	public interface ICacheKeyProvider
	{
		string Get(ISecurityContext securityContext);
	}
}