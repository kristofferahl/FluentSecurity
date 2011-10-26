using System.Web.Mvc;
using FluentSecurity.Policy.Results;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class DelegatePolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			var delegatePolicyResult = (DelegatePolicyResult)exception.PolicyResult;
			
			if (delegatePolicyResult.ViolationHandler != null)
				return delegatePolicyResult.ViolationHandler.Invoke(exception);

			throw exception;
		}
	}
}