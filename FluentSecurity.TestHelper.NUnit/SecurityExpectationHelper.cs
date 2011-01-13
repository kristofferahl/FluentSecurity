using System.Web.Mvc;

namespace FluentSecurity.TestHelper.NUnit
{
	public abstract class SecurityExpectationHelper<TController> : SecurityExpectationHelperBase<TController> where TController : IController
	{
		protected SecurityExpectationHelper()
		{
			ExpectationViolationHandler = new NUnitExpectationViolationHandler();
		}
	}

	public abstract class SecurityExpectationHelper : SecurityExpectationHelperBase
	{
		protected SecurityExpectationHelper()
		{
			ExpectationViolationHandler = new NUnitExpectationViolationHandler();
		}
	}
}