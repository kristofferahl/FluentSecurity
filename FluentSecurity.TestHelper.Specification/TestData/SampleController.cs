using System.Web.Mvc;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class SampleController : Controller
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

		public ActionResult DoNotIgnoreThisAction()
		{
			return null;
		}
		
		[ActionName("AliasedAction")]
		public ActionResult ActualAction()
		{
			return null;
		}

		public void VoidAction() { }
	}
}