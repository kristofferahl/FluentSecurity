using FluentSecurity.TestHelper.NUnit;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification.NUnit
{
	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_creating_a_new_SecurityExpectationHelper : SecurityExpectationHelper
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return null;
		}

		[Test]
		public void Should_have_expectation_violation_handler_set()
		{
			Assert.That(ExpectationViolationHandler, Is.TypeOf(typeof(NUnitExpectationViolationHandler)));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_creating_a_new_SecurityExpectationHelper_for_AdminController : SecurityExpectationHelper<AdminController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return null;
		}

		[Test]
		public void Should_have_expectation_violation_handler_set()
		{
			Assert.That(ExpectationViolationHandler, Is.TypeOf(typeof(NUnitExpectationViolationHandler)));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_verifying_expectations_with_SecurityExpectationHelper : SecurityExpectationHelper
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return FluentSecurityFactory.CreateSecurityConfiguration();
		}

		[Test]
		public void Should_throw()
		{
			// Act & assert
			Assert.Throws<AssertionException>(() =>
				VerifyExpectations(expectations => expectations.Expect<AdminController>().Has<DenyInternetExplorerPolicy>())	
			);
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_verifying_expectations_with_SecurityExpectationHelper_for_AdminController : SecurityExpectationHelper<AdminController>
	{
		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return FluentSecurityFactory.CreateSecurityConfiguration();
		}

		[Test]
		public void Should_throw()
		{
			// Act & assert
			Assert.Throws<AssertionException>(() =>
				VerifyExpectations(expectations => expectations.Expect().Has<DenyInternetExplorerPolicy>())
			);
		}
	}
}