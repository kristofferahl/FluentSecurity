using System;
using System.Web.Mvc;

namespace FluentSecurity.Specification.Policy.ViolationHandlers
{
	public class HttpUnauthorizedPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			if (exception == null) throw new ArgumentNullException("exception");
			
			return new HttpUnauthorizedResult(exception.Message);
		}
	}
}