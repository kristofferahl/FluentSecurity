using NUnit.Framework;

namespace FluentSecurity.TestHelper
{
	// TODO: Move to it's own assembly
	public class NUnitExpectationViolationHandler : IExpectationViolationHandler
	{
		public ExpectationResult Handle(string message)
		{
			throw new AssertionException(message);
		}
	}
}