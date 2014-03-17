using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public class ProfileScanner : AssemblyScanner
	{
		public void LookForProfiles()
		{
			With<ProfileTypeScanner>();
		}
	}
}