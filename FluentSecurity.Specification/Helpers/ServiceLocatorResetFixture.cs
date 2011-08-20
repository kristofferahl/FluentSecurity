using FluentSecurity.ServiceLocation;
using NUnit.Framework;

namespace FluentSecurity.Specification // Do not change the namespace
{
	[SetUpFixture]
	public class ServiceLocatorResetFixture
	{
		[SetUp]
		public void ResetServiceLocator()
		{
			ServiceLocator.Reset();
			ExceptionFactory.Reset();
		}
	}
}