using System.Web.Mvc;

namespace FluentSecurity
{
	public class ExceptionPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			throw exception;
		}
	}
}