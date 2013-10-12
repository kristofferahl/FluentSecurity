using System.Web.Mvc;
using System.Web.Routing;

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
			Bootstrapper.SetupFluentSecurity();
			AreaRegistration.RegisterAllAreas();
			RegisterRoutes(RouteTable.Routes);
			RegisterGlobalFilters(GlobalFilters.Filters);
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

		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleSecurityAttribute(), -1);
		}
	}
}