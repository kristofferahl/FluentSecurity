using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_calling_expect_for_SampleController_Index_with_helper : SecurityExpectationHelper
	{
		[Test]
		public void Should_throw_when_policy_containers_is_null()
		{
			// Arrange
			PolicyContainers = null;

			// Act & Assert
			Assert.Throws<AssertionException>(() => Expect<SampleController>(x => x.Index()));
		}

		[Test]
		public void Should_return_1_expectation_when_policy_containers_has_been_set()
		{
			// Arrange
			PolicyContainers = FluentSecurityFactory.CreatePolicyContainers();

			// Act
			var expectations = Expect<SampleController>(x => x.Index());

			// Assert
			Assert.That(expectations, Is.TypeOf(typeof(PolicyExpectations)));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_calling_expect_for_SampleController_with_helper : SecurityExpectationHelper
	{
		[Test]
		public void Should_throw_when_policy_containers_is_null()
		{
			// Arrange
			PolicyContainers = null;

			// Act & Assert
			Assert.Throws<AssertionException>(() => Expect<SampleController>());
		}

		[Test]
		public void Should_return_4_expectations_when_policy_containers_has_been_set()
		{
			// Arrange
			PolicyContainers = FluentSecurityFactory.CreatePolicyContainers();

			// Act
			var expectations = Expect<SampleController>();

			// Assert
			Assert.That(expectations, Is.TypeOf(typeof(PolicyExpectations)));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_calling_expect_for_Index_with_helper_of_SampleController : SecurityExpectationHelper<SampleController>
	{
		[Test]
		public void Should_throw_when_policy_containers_is_null()
		{
			// Arrange
			PolicyContainers = null;

			// Act & Assert
			Assert.Throws<AssertionException>(() => Expect(x => x.Index()));
		}

		[Test]
		public void Should_return_expectations_when_policy_containers_has_been_set()
		{
			// Arrange
			PolicyContainers = FluentSecurityFactory.CreatePolicyContainers();

			// Act
			var expectations = Expect(x => x.Index());

			// Assert
			Assert.That(expectations, Is.TypeOf(typeof(PolicyExpectations)));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationHelperSpec")]
	public class When_calling_expect_with_helper_of_SampleController : SecurityExpectationHelper<SampleController>
	{
		[Test]
		public void Should_throw_when_policy_containers_is_null()
		{
			// Arrange
			PolicyContainers = null;

			// Act & Assert
			Assert.Throws<AssertionException>(() => Expect());
		}

		[Test]
		public void Should_return_expectations_when_policy_containers_has_been_set()
		{
			// Arrange
			PolicyContainers = FluentSecurityFactory.CreatePolicyContainers();

			// Act
			var expectations = Expect();

			// Assert
			Assert.That(expectations, Is.TypeOf(typeof(PolicyExpectations)));
		}
	}
}