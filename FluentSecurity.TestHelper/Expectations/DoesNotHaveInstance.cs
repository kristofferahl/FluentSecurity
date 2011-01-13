using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public class DoesNotHaveInstanceExpectation : IExpectation
	{
		public ISecurityPolicy Instance { get; private set; }

		public DoesNotHaveInstanceExpectation(ISecurityPolicy instance)
		{
			Instance = instance;
		}
	}
}