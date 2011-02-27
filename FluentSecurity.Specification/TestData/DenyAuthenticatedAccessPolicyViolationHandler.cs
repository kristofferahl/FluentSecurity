using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
{
	public class DenyAuthenticatedAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		private readonly ActionResult _actionResult;

		public DenyAuthenticatedAccessPolicyViolationHandler(ActionResult actionResult)
		{
			_actionResult = actionResult;
		}

		public virtual ActionResult Handle(PolicyViolationException exception)
		{
			return _actionResult;
		}
	}
}