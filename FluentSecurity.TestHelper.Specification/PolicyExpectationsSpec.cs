using System;
using System.Collections.Generic;
using System.Linq;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("PolicyExpectationsSpec")]
	public class When_creating_policy_expectations
	{
		private IEnumerable<IPolicyContainer> _policyContainers;

		[SetUp]
		public void SetUp()
		{
			_policyContainers = FluentSecurityFactory.CreatePolicyContainers();
		}

		private PolicyExpectations Because()
		{
			return new PolicyExpectations(_policyContainers);
		}

		[Test]
		public void Should_have_policy_containers()
		{
			// Arrange
			_policyContainers = FluentSecurityFactory.CreatePolicyContainers();

			// Act
			var policyExtensions = Because();

			// Assert
			Assert.That(policyExtensions.PolicyContainers, Is.EqualTo(_policyContainers));
		}

		[Test]
		public void Should_have_0_expectations()
		{
			// Arrange
			const int expectedAmount = 0;

			// Act
			var policyExpectations = Because();

			// Assert
			Assert.That(policyExpectations.ExpectationsFor.Count(), Is.EqualTo(expectedAmount));
		}

		[Test]
		public void Should_throw_when_policy_containers_is_null()
		{
			// Arrange
			const string expectedName = "policyContainers";
			_policyContainers = null;

			// Act
			var results = Assert.Throws<ArgumentNullException>(() => Because());

			// Assert
			Assert.That(results.ParamName, Is.EqualTo(expectedName));
		}
	}

	[TestFixture]
	[Category("PolicyExpectationsSpec")]
	public class When_adding_expectation_for_SampleController_Index
	{
		private IEnumerable<IPolicyContainer> _policyContainers;
		private PolicyExpectations _expectations;
		private string _controller = "Sample";
		private string _action = "Index";

		[SetUp]
		public void SetUp()
		{
			_policyContainers = FluentSecurityFactory.CreatePolicyContainers();
			_expectations = new PolicyExpectations(_policyContainers);
			_controller = "Sample";
			_action = "Index";
		}

		private PolicyExpectations Because()
		{
			return _expectations.For(_controller, _action);
		}

		[Test]
		public void Should_have_one_expectation()
		{
			const int expectedExpectations = 1;

			// Act
			var expectations = Because();

			// Assert
			var totalExpectations = expectations.ExpectationsFor.Count();
			Assert.That(totalExpectations, Is.EqualTo(expectedExpectations));
		}

		[Test]
		public void Should_have_expectation_for_Sample_Index()
		{
			// Arrange
			const string controller = "Sample";
			const string action = "Index";
			const int expectedExpectations = 1;

			// Act
			var expectations = Because();

			// Assert
			var matchingExpectations = expectations.ExpectationsFor.Count(x => x.Key.Equals(controller) && x.Value.Equals(action));
			Assert.That(matchingExpectations, Is.EqualTo(expectedExpectations));
		}

		[Test]
		public void Should_throw_when_controller_name_is_null()
		{
			// Arrange
			const string expectedName = "controllerName";
			_controller = null;

			// Act
			var results = Assert.Throws<ArgumentException>(() => Because());

			// Assert
			Assert.That(results.ParamName, Is.EqualTo(expectedName));
		}

		[Test]
		public void Should_throw_when_controller_name_is_empty()
		{
			// Arrange
			const string expectedName = "controllerName";
			_controller = string.Empty;

			// Act
			var results = Assert.Throws<ArgumentException>(() => Because());

			// Assert
			Assert.That(results.ParamName, Is.EqualTo(expectedName));
		}

		[Test]
		public void Should_throw_when_action_name_is_null()
		{
			// Arrange
			const string expectedName = "actionName";
			_action = null;

			// Act
			var results = Assert.Throws<ArgumentException>(() => Because());

			// Assert
			Assert.That(results.ParamName, Is.EqualTo(expectedName));
		}

		[Test]
		public void Should_throw_when_action_name_is_empty()
		{
			// Arrange
			const string expectedName = "actionName";
			_action = string.Empty;

			// Act
			var results = Assert.Throws<ArgumentException>(() => Because());

			// Assert
			Assert.That(results.ParamName, Is.EqualTo(expectedName));
		}
	}
}