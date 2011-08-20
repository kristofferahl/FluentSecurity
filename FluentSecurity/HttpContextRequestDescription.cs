using System;
using System.Web;
using System.Web.Routing;

namespace FluentSecurity
{
	public class HttpContextRequestDescription : IRequestDescription
	{
		internal static readonly Func<HttpContextBase> DefaultHttpContextProvider = () => new HttpContextWrapper(HttpContext.Current);
		internal static Func<HttpContextBase> HttpContextProvider = DefaultHttpContextProvider;

		public HttpContextRequestDescription()
		{
			var route = GetRoute();
			AreName = route.GetAreaName();
			ControllerName = (string)route.Values["controller"];
			ActionName = (string)route.Values["action"];
		}

		private static RouteData GetRoute()
		{
			var httpContext = HttpContextProvider();
			var routeData = RouteTable.Routes.GetRouteData(httpContext);
			return routeData ?? new RouteData();
		}

		public string AreName { get; private set; }
		public string ControllerName { get; private set; }
		public string ActionName { get; private set; }
	}
}