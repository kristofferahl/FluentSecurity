using NUnit.Framework;

namespace FluentSecurity.TestHelper.NUnit
{
	public class NUnitExpectationViolationHandler : IExpectationViolationHandler
	{
		public ExpectationResult Handle(string message)
		{
			throw new AssertionException(message);
		}
	}
}