namespace FluentSecurity.Scanning
{
	public interface IProfileAssemblyScannerConfiguration : IAssemblyScannerConfiguration
	{
		void LookForProfiles();
	}
}