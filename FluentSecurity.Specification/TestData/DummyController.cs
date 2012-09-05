using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
{
	public class DummyController : Controller
	{
		[ActionName("View")]
		public ActionResult Show()
		{
			return View();
		}
	}
}
