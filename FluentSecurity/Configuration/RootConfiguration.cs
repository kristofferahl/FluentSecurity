namespace FluentSecurity.Configuration
{
	public class RootConfiguration : ConfigurationExpression
	{
		public RootConfiguration()
		{
			Initialize(new SecurityRuntime());
		}
	}
}