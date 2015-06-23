using FluentSecurity.Configuration;
using FluentSecurity.Scanning.TypeScanners;

namespace FluentSecurity.Scanning
{
	public class ProfileScanner : AssemblyScanner, IProfileAssemblyScanner
	{
		public void LookForProfiles()
		{
			With<ProfileTypeScanner<SecurityProfile>>();
		}
	}
}