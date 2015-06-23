using FluentSecurity.Scanning;
using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.WebApi.Configuration;
using FluentSecurity.WebApi.Scanning;

namespace FluentSecurity.WebApi
{
	public class WebApiProfileScanner : WebApiAssemblyScanner, IProfileAssemblyScanner
	{
		public void LookForProfiles()
		{
			With<ProfileTypeScanner<WebApiSecurityProfile>>();
		}
	}
}