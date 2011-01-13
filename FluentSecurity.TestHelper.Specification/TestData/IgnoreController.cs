using System.Web.Mvc;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class IgnoreController : Controller
	{
		public ActionResult Index()
		{
			return null;
		}

		public ActionResult New()
		{
			return null;
		}

		public ActionResult List()
		{
			return null;
		}
	}

	public class TestController : Controller
	{
		public ActionResult HasIgnoreAttribute()
		{
			return null;
		}
	}
}