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

		[SetUp]
		public void SetUp()
		{
			_policyExpectations = new PolicyExpectations();
		}

		[Test]
		public void Should_have_0_expectation_groups()
		{
			Assert.That(_policyExpectations.ExpectationGroups.Count(), Is.EqualTo(0));
		}

		[Test]
		public void Should_have_expectation_builder()
		{
			Assert.That(_policyExpectations.ExpectationGroupBuilder.GetType(), Is.EqualTo(typeof(ExpectationGroupBuilder)));
		}

		[Test]
		public void Should_have_expectation_violation()
		{
			Assert.That(_policyExpectations.ExpectationViolationHandler.GetType(), Is.EqualTo(typeof(DefaultExpectationViolationHandler)));
		}

		[Test]
		public void Should_have_expectation_verifyer_provider()
		{
			var configuration = FluentSecurityFactory.CreateEmptySecurityConfiguration();
			var handler = _policyExpectations.ExpectationViolationHandler;
			Assert.That(_policyExpectations.ExpectationVerifyerProvider(configuration, handler).GetType(), Is.EqualTo(typeof(ExpectationVerifyer)));
		}
	}

	[TestFixture]
	[Category("PolicyExpectationsSpec")]
	public class When_verifying_all_policy_expectations
	{
		[Test]
		public void Should_throw_when_configuration_is_null()
		{
			// Arrange
			ISecurityConfiguration configuration = null;
			var policyExpectations = new PolicyExpectations();

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => policyExpectations.VerifyAll(configuration));
		}

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

			var policyExpectations = new PolicyExpectations();
			policyExpectations.SetExpectationGroupBuilder(builder.Object);
			policyExpectations.ConstructExpectationVerifyerUsing((c, h) => verifyer.Object);
			policyExpectations.For<SampleController>();
			policyExpectations.For<AdminController>(x => x.Login());

			// Act
			policyExpectations.VerifyAll(configuration);

			// Assert
			builder.Verify(x => x.Build(It.IsAny<IEnumerable<ExpectationExpression>>()), Times.AtMostOnce());
			verifyer.Verify(x => x.VerifyExpectationsOf(expectationGroups), Times.AtMostOnce());
			builder.VerifyAll();
			verifyer.VerifyAll();
		}
	}
}