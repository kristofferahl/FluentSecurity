using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Policy.ViolationHandlers.Conventions;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy.ViolationHandlers.Conventions
{
	[TestFixture]
	[Category("FindDefaultPolicyViolationHandlerByNameConventionSpec")]
	public class When_getting_a_PolicyViolationHandler_using_FindDefaultPolicyViolationHandlerByNameConvention
	{
		[Test]
		public void Should_return_null_when_no_handler_is_a_match()
		{
			// Arrange
			var convention = new FindDefaultPolicyViolationHandlerByNameConvention
			{
				PolicyViolationHandlerProvider = () => new List<IPolicyViolationHandler>()
			};
			var exception = TestDataFactory.CreateExceptionFor(new IgnorePolicy());

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.Null);
		}

		[Test]
		public void Should_return_DefaultPolicyViolationHandler_for_RequireAnyRolePolicy()
		{
			// Arrange
			var convention = new FindDefaultPolicyViolationHandlerByNameConvention
			{
				PolicyViolationHandlerProvider = () => TestDataFactory.CreatePolicyViolationHandlers()
			};
			var exception = TestDataFactory.CreateExceptionFor(new RequireAnyRolePolicy("Role"));

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<DefaultPolicyViolationHandler>());
		}

		[Test]
		public void Should_return_DefaultPolicyViolationHandler_for_RequireAllRolesPolicy()
		{
			// Arrange
			var convention = new FindDefaultPolicyViolationHandlerByNameConvention
			{
				PolicyViolationHandlerProvider = () => TestDataFactory.CreatePolicyViolationHandlers()
			};
			var exception = TestDataFactory.CreateExceptionFor(new RequireAllRolesPolicy("Role"));

			// Act
			var handler = convention.GetHandlerFor(exception);

			// Assert
			Assert.That(handler, Is.InstanceOf<DefaultPolicyViolationHandler>());
		}
	}
}