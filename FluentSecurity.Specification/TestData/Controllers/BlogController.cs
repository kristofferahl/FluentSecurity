using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
{
	public class BlogController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public Task<ActionResult> ListPosts()
		{
			return Task.Factory.StartNew<ActionResult>(View);
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

		public Task<JsonResult> AjaxList()
		{
			return Task.Factory.StartNew(() => Json(new {}));
		}

		private void PrivateMethodThatDoesNothing()
		{
		}
	}
}