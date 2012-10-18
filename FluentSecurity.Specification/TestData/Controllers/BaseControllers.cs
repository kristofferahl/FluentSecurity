using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData.Controllers
{
	namespace BaseControllers
	{
		public class IneritingBaseController : BaseController {}

		public class IneritingAbstractBaseController : AbstractBaseController {}

		public class BaseController : Controller {}

		public abstract class AbstractBaseController : Controller {}
	}
}