using System;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("SecurityExpectationsHelperSpecs")]
	public class When_calling_expect_for_SampleController_Index_with_helper : SecurityExpectationHelper
	{
		private ISecurityConfiguration _securityConfiguration;

		[SetUp]
		public void SetUp()
		{
			_securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();
		}

		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return _securityConfiguration;
		}

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			// Arrange
			_securityConfiguration = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => 
				VerifyExpectations(expectations => expectations.Expect<SampleController>(x => x.Index()))
			);
		}

		[Test]
		public void Should_verify_and_return_2_results()
		{
			// Act
			var result = VerifyExpectations(expectations =>
			{	
				expectations.Expect<SampleController>(x => x.Index()).Has<DenyInternetExplorerPolicy>();
				expectations.Expect<SampleController>(x => x.New()).DoesNotHave<IgnorePolicy>();
			});

			// Assert
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_verify_and_return_4_results()
		{
			// Act
			var result = VerifyExpectations(expectations =>
				expectations.Expect<SampleController>().Has<DenyInternetExplorerPolicy>()
			);

			// Assert
			Assert.That(result.Count(), Is.EqualTo(4));
		}
	}

	[TestFixture]
	[Category("SecurityExpectationsHelperSpecs")]
	public class When_calling_expect_for_SampleController_with_helper : SecurityExpectationHelper<SampleController>
	{
		private ISecurityConfiguration _securityConfiguration;

		[SetUp]
		public void SetUp()
		{
			_securityConfiguration = FluentSecurityFactory.CreateSecurityConfiguration();
		}

		protected override ISecurityConfiguration ConfigurationToTest()
		{
			return _securityConfiguration;
		}

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			// Arrange
			_securityConfiguration = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				VerifyExpectations(expectations => expectations.Expect(x => x.Index()))
			);
		}

		[Test]
		public void Should_verify_and_return_2_results()
		{
			// Act
			var result = VerifyExpectations(expectations =>
			{
				expectations.Expect(x => x.Index()).Has<DenyInternetExplorerPolicy>();
				expectations.Expect(x => x.New()).DoesNotHave<IgnorePolicy>();
			});

			// Assert
			Assert.That(result.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_verify_and_return_4_results()
		{
			// Act
			var result = VerifyExpectations(expectation =>
				expectation.Expect().Has<DenyInternetExplorerPolicy>()
			);

			// Assert
			Assert.That(result.Count(), Is.EqualTo(4));
		}
	}
}