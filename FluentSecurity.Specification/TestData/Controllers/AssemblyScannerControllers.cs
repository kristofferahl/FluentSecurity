using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData.Controllers
{
	namespace AssemblyScannerControllers
	{
		namespace Include
		{
			internal class ClassInIncludeNamespace {}

			public class IncludedController : Controller
			{
				public ActionResult Index()
				{
					return new EmptyResult();
				}
			}
		}

		namespace Exclude
		{
			internal class ClassInExcludeNamespace {}

			public class ExcludedController : Controller
			{
				public ActionResult Index()
				{
					return new EmptyResult();
				}
			}
		}

		internal class ClassInRootNamespace { }

		public class RootController : Controller
		{
			public ActionResult Index()
			{
				return new EmptyResult();
			}
		}
	}
}