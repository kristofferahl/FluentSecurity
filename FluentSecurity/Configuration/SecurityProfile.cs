namespace FluentSecurity.Configuration
{
	public abstract class SecurityProfile : ConfigurationExpression
	{
		public abstract void Configure();
	}
}