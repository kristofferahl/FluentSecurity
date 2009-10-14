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
			Configuration.Configure(policy =>
			{
				policy.GetAuthenticationStatusFrom(Helpers.SecurityHelper.UserIsAuthenticated);
				policy.GetRolesFrom(Helpers.SecurityHelper.UserRoles);

				policy.IgnoreMissingConfiguration();

				policy.For<AccountController>(x => x.LogInAsAdministrator()).DenyAuthenticatedAccess();
				policy.For<AccountController>(x => x.LogInAsPublisher()).DenyAuthenticatedAccess();
				policy.For<AccountController>(x => x.LogOut()).DenyAnonymousAccess();

				policy.For<ExampleController>(x => x.DenyAnonymousAccess()).DenyAnonymousAccess();
				policy.For<ExampleController>(x => x.DenyAuthenticatedAccess()).DenyAuthenticatedAccess();

				policy.For<ExampleController>(x => x.RequireAdministratorRole()).RequireRole(UserRole.Administrator);
				policy.For<ExampleController>(x => x.RequirePublisherRole()).RequireRole(UserRole.Publisher);

				policy.For<AdminController>().RequireRole(UserRole.Administrator);
				policy.For<AdminController>(x => x.Index()).Ignore();
			});
		}
	}
}