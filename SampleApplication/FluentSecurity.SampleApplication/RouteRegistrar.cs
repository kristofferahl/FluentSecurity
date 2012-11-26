using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSecurity.SampleApplication
{
	public class RouteRegistrar : IRouteRegistrar
	{
		private readonly RouteCollection _routes;

		public RouteRegistrar(RouteCollection routes)
		{
			_routes = routes;
		}

		public void RegisterRoutes()
		{
			_routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			_routes.MapRoute(
				"Default",                                              // Route name
				"{controller}/{action}/{id}",                           // URL with parameters
				new { controller = "Home", action = "Index", id = "" },  // Parameter defaults
				new[] { "FluentSecurity.SampleApplication.Controllers" }
				);
		}
	}
}