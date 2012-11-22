using System.Web;
using FluentSecurity.SampleApplication.Areas.ExampleArea;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public static class Bootstrapper
	{
		public static ISecurityConfiguration SetupFluentSecurity()
		{
			SecurityConfigurator.Configure(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(Helpers.SecurityHelper.UserIsAuthenticated);
				configuration.GetRolesFrom(Helpers.SecurityHelper.UserRoles);

				configuration.DefaultPolicyViolationHandlerIs(() => new DefaultPolicyViolationHandler());
				configuration.Advanced.ModifySecurityContext(context => context.Data.QueryString = HttpContext.Current.Request.QueryString);

				configuration.For<HomeController>().Ignore();

				configuration.For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

				configuration.For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
				configuration.For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

				configuration.For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
				configuration.For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

				configuration.For<AdminController>().AddPolicy(new AdministratorPolicy());
				configuration.For<AdminController>(x => x.Delete()).DelegatePolicy("LocalOnlyPolicy",
					context => HttpContext.Current.Request.IsLocal
					);

				configuration.Scan(scan =>
				{
					scan.AssembliesFromApplicationBaseDirectory();
					scan.IncludeNamespaceContainingType<ExampleAreaAreaRegistration>();
					scan.LookForProfiles();
				});

				configuration.ApplyProfile<CrudControllerProfile>();
			});
			return SecurityConfiguration.Current;
		}
	}
}