using FluentSecurity.Configuration;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public class CrudControllerProfile : SecurityProfile
	{
		public override void Configure()
		{
			ForAllControllersInheriting<CrudController>().DenyAnonymousAccess();
			ForAllControllersInheriting<CrudController>(x => x.Delete()).RequireAnyRole(UserRole.Administrator);
			
			For<BlogPostController>(x => x.Index()).Ignore();
			For<BlogPostController>(x => x.Details()).Ignore();
		}
	}
}