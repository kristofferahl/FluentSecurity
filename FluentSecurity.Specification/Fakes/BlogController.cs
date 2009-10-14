using System.Web.Mvc;

namespace FluentSecurity.Specification.Fakes
{
	public class BlogController : SecureController
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult ListPosts()
		{
			return View();
		}

		public ActionResult AddPost()
		{
			return View();
		}

		public ActionResult EditPost()
		{
			return View();
		}

		public ActionResult DeletePost()
		{
			return View();
		}

		private void PrivateMethodThatDoesNothing()
		{
		}
	}
}