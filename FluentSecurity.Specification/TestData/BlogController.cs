using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
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

		public ActionResult AddPost(PostForm postForm)
		{
			return View();
		}

		public ActionResult EditPost(int postId)
		{
			return View();
		}

		public ActionResult EditPost(PostForm postForm)
		{
			return View();
		}

		public ActionResult DeletePost(int posdId)
		{
			return View();
		}

		private void PrivateMethodThatDoesNothing()
		{
		}
	}
}