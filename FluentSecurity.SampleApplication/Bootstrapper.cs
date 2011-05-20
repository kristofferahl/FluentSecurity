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

				configuration.IgnoreMissingConfiguration();

				configuration.For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
				configuration.For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

				configuration.For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
				configuration.For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

				configuration.For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
				configuration.For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

				configuration.For<AdminController>().AddPolicy(new AdministratorPolicy());
				configuration.For<AdminController>(x => x.Index()).Ignore();
			});
			return SecurityConfiguration.Current;
		}
	}
}