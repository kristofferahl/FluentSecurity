using System.Web.Mvc;

namespace FluentSecurity.Policy.ViolationHandlers
{
	public class DelegatePolicyViolationHandler : IPolicyViolationHandler
	{
		public ActionResult Handle(PolicyViolationException exception)
		{
			var delegatePolicy = (DelegatePolicy)exception.Policy;
			
			if (delegatePolicy.ViolationHandler != null)
				return delegatePolicy.ViolationHandler.Invoke(exception);

			throw exception;
		}
	}
}