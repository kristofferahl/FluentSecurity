using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.TestHelper.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("PolicyExpectationsSpec")]
	public class When_creating_policy_expectations
	{
		private PolicyExpectations _policyExpectations;
		private ISecurityConfiguration _configuration;

		[SetUp]
		public void SetUp()
		{
			_configuration = FluentSecurityFactory.CreateSecurityConfiguration();
			_policyExpectations = new PolicyExpectations(_configuration);
		}

		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			// Arrange
			ISecurityConfiguration configuration = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new PolicyExpectations(configuration));
		}

		[Test]
		public void Should_have_0_expectation_groups()
		{
			Assert.That(_policyExpectations.BuildExpectationGroups().Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_expectation_builder()
		{
			Assert.That(_policyExpectations.ExpectationGroupBuilderProvider(_configuration).GetType(), Is.EqualTo(typeof(ExpectationGroupBuilder)));
		}

		[Test]
		public void Should_have_expectation_violation()
		{
			Assert.That(_policyExpectations.ExpectationViolationHandlerProvider(_configuration).GetType(), Is.EqualTo(typeof(DefaultExpectationViolationHandler)));
		}

		[Test]
		public void Should_have_expectation_verifyer_provider()
		{
			var configuration = FluentSecurityFactory.CreateEmptySecurityConfiguration();
			var handler = _policyExpectations.ExpectationViolationHandlerProvider(configuration);
			Assert.That(_policyExpectations.ExpectationVerifyerProvider(configuration, handler).GetType(), Is.EqualTo(typeof(ExpectationVerifyer)));
		}
	}

	[TestFixture]
	[Category("PolicyExpectationsSpec")]
	public class When_verifying_all_policy_expectations
	{
		[Test]
		public void Should_build_expectation_groups_and_verify_expectations()
		{
			// Arrange
			var configuration = FluentSecurityFactory.CreateSecurityConfiguration();
			var expectationGroups = new List<ExpectationGroup>();

			var builder = new Mock<IExpectationGroupBuilder>();
			var verifyer = new Mock<IExpectationVerifyer>();

			builder.Setup(x => x.Build(It.IsAny<IEnumerable<ExpectationExpression>>())).Returns(expectationGroups);
			verifyer.Setup(x => x.VerifyExpectationsOf(expectationGroups)).Returns(new List<ExpectationResult>());

			var policyExpectations = new PolicyExpectations(configuration);
			policyExpectations.Using(c => builder.Object);
			policyExpectations.Using((c, h) => verifyer.Object);
			policyExpectations.For<SampleController>();
			policyExpectations.For<AdminController>(x => x.Login());

			// Act
			policyExpectations.VerifyAll();

			// Assert
			builder.Verify(x => x.Build(It.IsAny<IEnumerable<ExpectationExpression>>()), Times.AtMostOnce());
			verifyer.Verify(x => x.VerifyExpectationsOf(expectationGroups), Times.AtMostOnce());
			builder.VerifyAll();
			verifyer.VerifyAll();
		}
	}
}