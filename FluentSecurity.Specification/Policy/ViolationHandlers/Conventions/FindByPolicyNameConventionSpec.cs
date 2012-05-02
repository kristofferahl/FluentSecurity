using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("FindByPolicyNameConventionSpec")]
	public class When_getting_a_PolicyViolationHandler_using_FindByPolicyNameConvention
	{
		private FindByPolicyNameConvention _convention;

		[SetUp]
		public void SetUp()
		{
			_convention = new FindByPolicyNameConvention
			{
				PolicyViolationHandlerProvider = () => TestDataFactory.CreatePolicyViolationHandlers()
			};
		}

		[Test]
		public void Should_return_null_when_no_handler_is_a_match()
		{
			// Arrange
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = _convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_return_DenyAnonymousAccessPolicyViolationHandler_for_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var exception = TestDataFactory.CreateExceptionFor(new DenyAnonymousAccessPolicy());

			// Act
			var handler = _convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<DenyAnonymousAccessPolicyViolationHandler>());
		}

		[Test]
		public void Should_return_DenyAuthenticatedAccessPolicyViolationHandler_for_DenyAuthenticatedAccessPolicy()
		{
			// Arrange
			var exception = TestDataFactory.CreateExceptionFor(new DenyAuthenticatedAccessPolicy());

			// Act
			var handler = _convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<DenyAuthenticatedAccessPolicyViolationHandler>());
		}
	}
}