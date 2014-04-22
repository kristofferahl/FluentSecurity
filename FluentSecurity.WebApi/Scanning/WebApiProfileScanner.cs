using FluentSecurity.Scanning.TypeScanners;
using FluentSecurity.WebApi.Configuration;
using FluentSecurity.WebApi.Scanning;

namespace FluentSecurity.WebApi
{
	public class WebApiProfileScanner : WebApiAssemblyScanner
	{
		public void LookForProfiles()
		{
			With<ProfileTypeScanner<WebApiSecurityProfile>>();
		}
	}
}