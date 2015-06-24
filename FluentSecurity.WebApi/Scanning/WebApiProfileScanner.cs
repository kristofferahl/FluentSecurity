using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.WebApi.Configuration;

namespace FluentSecurity.WebApi.Scanning
{
	public class WebApiProfileScanner : WebApiAssemblyScanner, IProfileAssemblyScanner
	{
		public void LookForProfiles()
		{
			With<ProfileTypeScanner<WebApiSecurityProfile>>();
		}
	}
}