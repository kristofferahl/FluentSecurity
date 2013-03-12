using System.Threading.Tasks;
using System.Web.Mvc;

namespace FluentSecurity.TestHelper.Specification.TestData
{
	public class TaskController : AsyncController
	{
		public Task<ActionResult> LongRunningAction()
		{
			return null;
		}

		public Task<JsonResult> LongRunningJsonAction()
		{
			return null;
		}
	}
}
