namespace FluentSecurity.TestHelper
{
	public class DefaultExpectationViolationHandler : IExpectationViolationHandler
	{
		public ExpectationResult Handle(string message)
		{
			return ExpectationResult.CreateFailureResult(message);
		}
	}
}