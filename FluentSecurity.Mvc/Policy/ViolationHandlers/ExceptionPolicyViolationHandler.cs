using System.Web.Mvc;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class ExceptionPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			throw exception;
		}
	}
}