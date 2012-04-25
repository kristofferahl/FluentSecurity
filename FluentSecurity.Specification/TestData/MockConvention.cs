using FluentSecurity.Policy.ViolationHandlers.Conventions;

namespace FluentSecurity.Specification.TestData
{
	public class MockConvention : IPolicyViolationHandlerConvention
	{
		private readonly IPolicyViolationHandler _handlerToReturn;

		public bool WasCalled { get; private set; }

		public MockConvention(IPolicyViolationHandler handlerToReturn = null)
		{
			_handlerToReturn = handlerToReturn;
		}

		public IPolicyViolationHandler GetHandlerFor(PolicyViolationException exception)
		{
			WasCalled = true;
			return _handlerToReturn;
		}
	}
}