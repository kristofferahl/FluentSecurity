using FluentSecurity.Policy;

namespace FluentSecurity.TestHelper.Expectations
{
	public class HasInstanceExpectation : IExpectation
	{
		public ISecurityPolicy Instance { get; private set; }

		public HasInstanceExpectation(ISecurityPolicy instance)
		{
			Instance = instance;
		}
	}
}