using System.Web.Mvc;

namespace FluentSecurity.Specification.TestData
{
	public class DenyAnonymousAccessPolicyViolationHandler : IPolicyViolationHandler
	{
		private readonly ActionResult _actionResult;

		public DenyAnonymousAccessPolicyViolationHandler(ActionResult actionResult)
		{
			_actionResult = actionResult;
		}

		public virtual ActionResult Handle(PolicyViolationException exception)
		{
			return _actionResult;
		}
	}
}