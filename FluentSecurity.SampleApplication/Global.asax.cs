using System.Web.Routing;
using FluentSecurity.SampleApplication.Controllers;
using FluentSecurity.SampleApplication.Models;

namespace FluentSecurity.SampleApplication
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			SetupApplication();
		}

		protected void Application_Error()
		{
			// TODO: Add logging here
		}

		public static void SetupApplication()
		{
			SetupContainer();
			SetControllerFactory();
			RegisterRoutes(RouteTable.Routes);
			SetupFluentSecurity();
		}

		public static void SetupContainer()
		{
			// TODO: Setup the IOC-container of your choice
		}

		public static void SetControllerFactory()
		{
			// TODO: Set the controllerfactory of your choice
		}

		public static void RegisterRoutes(RouteCollection routes)
		{
			new RouteRegistrar(routes).RegisterRoutes();
		}

		public static void SetupFluentSecurity()
		{
			FluentSecurity.Configure(configuration =>
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

				configuration.For<AdminController>().RequireRole(UserRole.Administrator);
				configuration.For<AdminController>(x => x.Index()).Ignore();
			});
		}
	}
}