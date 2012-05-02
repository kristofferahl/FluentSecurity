using System;
using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
{
	public class DefaultPolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			throw new NotImplementedException();
		}
	}
}