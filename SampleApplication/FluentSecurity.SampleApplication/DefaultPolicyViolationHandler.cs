using System.Web.Mvc;

namespace FluentSecurity.SampleApplication
{
	public class DefaultPolicyViolationHandler : IPolicyViolationHandler
	{
		public string ViewName = "AccessDenied";

		public ActionResult Handle(PolicyViolationException exception)
		{
			return new ViewResult { ViewName = ViewName };
		}
	}
}