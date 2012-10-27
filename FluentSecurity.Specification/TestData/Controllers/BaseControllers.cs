using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData.Controllers
{
	namespace BaseControllers
	{
		public class IneritingBaseController : BaseController
		{
			public ActionResult FirstClassAction()
			{
				return null;
			}
		}

		public class IneritingAbstractBaseController : AbstractBaseController
		{
			public ActionResult FirstClassAction()
			{
				return null;
			}
		}

		public class FirstInheritingGenericBaseController : GenericBaseController<FirstInheritingEntity, FirstInheritingBaseViewModel>
		{
			public ActionResult FirstClassAction()
			{
				return null;
			}
		}

		public class SecondInheritingGenericBaseController : GenericBaseController<SecondInheritingEntity, SecondInheritingBaseViewModel>
		{
			public ActionResult FirstClassAction()
			{
				return null;
			}
		}

		public class BaseController : Controller
		{
			public ActionResult InheritedAction()
			{
				return null;
			}
		}

		public abstract class AbstractBaseController : Controller
		{
			public ActionResult InheritedAction()
			{
				return null;
			}
		}

		public abstract class GenericBaseController<TEntity, TViewModel> : Controller
			where TEntity : BaseEntity
			where TViewModel : BaseViewModel
		{
			public ActionResult InheritedAction()
			{
				return null;
			}
		}

		public class SecondInheritingEntity : BaseEntity {}

		public class FirstInheritingEntity : BaseEntity {}

		public abstract class BaseEntity {}

		public class SecondInheritingBaseViewModel : BaseViewModel {}

		public class FirstInheritingBaseViewModel : BaseViewModel {}

		public abstract class BaseViewModel {}
	}
}