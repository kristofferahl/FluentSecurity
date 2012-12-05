using FluentSecurity.Configuration;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication.Areas.ExampleArea
{
	public class ExampleAreaSecurityProfile : SecurityProfile
	{
		public override void Configure()
		{
			For<Controllers.HomeController>().DenyAnonymousAccess();
			For<Controllers.HomeController>(x => x.PublishersOnly()).RequireAnyRole(UserRole.Publisher);
			For<Controllers.HomeController>(x => x.AdministratorsOnly()).RequireAnyRole(UserRole.Administrator);
		}
	}
}