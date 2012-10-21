using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData.Controllers
{
	public class AdminController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult LogIn()
		{
			return View();
		}

		public ActionResult LogOut()
		{
			return View();
		}
	}
}