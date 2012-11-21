using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Areas.ExampleArea.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult PublishersOnly()
		{
			return View();
		}

		public ActionResult AdministratorsOnly()
		{
			return View();
		}
	}
}