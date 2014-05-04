using System.Threading.Tasks;
using System.Web.Mvc;
using NUnit.Framework;

namespace FluentSecurity.Specification.Mvc
{
	[TestFixture]
	[Category("MvcActionResolverSpec")]
	public class When_checking_if_a_type_is_a_controller_action_return_type
	{
		[Test]
		public void Should_be_true_for_ActionResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(ActionResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_ContentResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(ContentResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_EmptyResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(EmptyResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_FileResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(FileResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_HttpStatusCodeResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(HttpStatusCodeResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_JavaScriptResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(JavaScriptResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_JsonResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(JsonResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_PartialViewResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(PartialViewResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_RedirectResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(RedirectResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_RedirectToRouteResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(RedirectToRouteResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_ViewResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(ViewResult)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ActionResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<ActionResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ContentResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<ContentResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_EmptyResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<EmptyResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_FileResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<FileResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_HttpStatusCodeResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<HttpStatusCodeResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_JavaScriptResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<JavaScriptResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_JsonResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<JsonResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_PartialViewResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<PartialViewResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_RedirectResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<RedirectResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_RedirectToRouteResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<RedirectToRouteResult>)), Is.True);
		}

		[Test]
		public void Should_be_true_for_Task_of_ViewResult()
		{
			Assert.That(MvcActionResolver.IsControllerActionReturnType(typeof(Task<ViewResult>)), Is.True);
		}
	}
}