using System.Web.Mvc;

namespace FluentSecurity.SampleApplication.Controllers
{
	public abstract class CrudController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Create()
		{
			return View();
		}

		public ActionResult Edit()
		{
			return View();
		}

		public ActionResult Delete()
		{
			return View();
		}

		public ActionResult Details()
		{
			return View();
		}
	}
}