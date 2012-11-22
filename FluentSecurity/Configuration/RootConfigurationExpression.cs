namespace FluentSecurity.Configuration
{
	public class RootConfigurationExpression : ConfigurationExpression
	{
		public RootConfigurationExpression()
		{
			Initialize(new SecurityModel());
		}
	}
}