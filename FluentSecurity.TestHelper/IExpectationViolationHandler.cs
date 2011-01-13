namespace FluentSecurity.TestHelper
{
	public interface IExpectationViolationHandler
	{
		ExpectationResult Handle(string message);
	}
}