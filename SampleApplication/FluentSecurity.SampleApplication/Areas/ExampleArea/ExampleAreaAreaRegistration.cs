using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Areas.ExampleArea
{
	public class ExampleAreaAreaRegistration : AreaRegistration
	{
		public override string AreaName
		{
			get { return "ExampleArea"; }
		}

		public override void RegisterArea(AreaRegistrationContext context)
		{
			context.MapRoute(
				"ExampleArea_default",
				"ExampleArea/{controller}/{action}/{id}",
				new { area = AreaName, action = "Index", id = UrlParameter.Optional }
				);
		}
	}
}