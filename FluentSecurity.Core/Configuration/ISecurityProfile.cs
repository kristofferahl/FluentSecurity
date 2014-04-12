namespace FluentSecurity.Configuration
{
	public interface ISecurityProfile
	{
		void Initialize(ISecurityRuntime runtime);
		void Configure();
	}
}