namespace FluentSecurity.Scanning
{
	public interface IProfileAssemblyScanner : IAssemblyScanner
	{
		void LookForProfiles();
	}
}