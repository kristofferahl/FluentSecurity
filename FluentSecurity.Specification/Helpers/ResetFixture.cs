using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification // Do not change the namespace
{
	[SetUpFixture]
	public class ResetFixture
	{
		[SetUp]
		public void Reset()
		{
			PolicyExecutionMode.StopOnFirstViolation(false);
			ServiceLocator.Reset();
			ExceptionFactory.Reset();
		}
	}
}