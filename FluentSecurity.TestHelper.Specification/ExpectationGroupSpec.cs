using System;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.TestHelper.Expectations;
using FluentSecurity.TestHelper.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.TestHelper.Specification
{
	[TestFixture]
	[Category("ExpectationGroupSpec")]
	public class When_creating_a_new_ExpectationGroup
	{
		private ExpectationGroup _expectationGroup;

		[SetUp]
		public void SetUp()
		{
			_expectationGroup = new ExpectationGroup(typeof(AdminController), "Login");
		}

		[Test]
		public void Should_have_controller_set_to_AdminController()
		{
			Assert.That(_expectationGroup.Controller, Is.EqualTo(typeof(AdminController)));
		}

		[Test]
		public void Should_have_action_set_to_Login()
		{
			Assert.That(_expectationGroup.Action, Is.EqualTo("Login"));
		}

		[Test]
		public void Should_have_0_expectations()
		{
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(0));
		}
	}

	[TestFixture]
	[Category("ExpectationGroupSpec")]
	public class When_applying_an_expectation_to_an_ExpectationGroup
	{
		private ExpectationGroup _expectationGroup;

		[SetUp]
		public void SetUp()
		{
			_expectationGroup = new ExpectationGroup(typeof(AdminController), "Login");
		}

		[Test]
		public void Should_throw_when_expectation_type_is_not_implemented()
		{
			// Arrange
			var expectation = new NotImplementedExpectation();

			// Act & assert
			Assert.Throws<ArgumentOutOfRangeException>(() => _expectationGroup.ApplyExpectation(expectation));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_type_for_HasTypeExpectation()
		{
			// Arrange
			var expectation1 = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new HasTypeExpectation<DenyInternetExplorerPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_2_expecation_when_expecations_have_different_types_for_HasTypeExpectation()
		{
			// Arrange
			var expectation1 = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new HasTypeExpectation<DenyLynxPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_type_for_DoesNotHaveTypeExpectation()
		{
			// Arrange
			var expectation1 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_2_expecation_when_expecations_have_different_types_for_DoesNotHaveTypeExpectation()
		{
			// Arrange
			var expectation1 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new DoesNotHaveTypeExpectation<DenyLynxPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_type_for_has_and_does_not_have()
		{
			// Arrange
			var expectation1 = new HasTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_type_for_does_not_have_and_has()
		{
			// Arrange
			var expectation1 = new DoesNotHaveTypeExpectation<DenyInternetExplorerPolicy>();
			var expectation2 = new HasTypeExpectation<DenyInternetExplorerPolicy>();

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_instance_for_HasInstanceExpectation()
		{
			// Arrange
			var expectation1 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_2_expecation_when_expecations_have_different_instances_for_HasInstanceExpectation()
		{
			// Arrange
			var expectation1 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor", "Writer"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_instance_for_DoesNotHaveInstanceExpectation()
		{
			// Arrange
			var expectation1 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_2_expecation_when_expecations_have_different_instances_for_DoesNotHaveInstanceExpectation()
		{
			// Arrange
			var expectation1 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor", "Writer"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(2));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_instance_for_has_and_does_not_have()
		{
			// Arrange
			var expectation1 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		[Test]
		public void Should_have_1_expecation_when_expecations_have_same_instance_for_does_not_have_and_has()
		{
			// Arrange
			var expectation1 = new DoesNotHaveInstanceExpectation(new RequireAnyRolePolicy("Editor"));
			var expectation2 = new HasInstanceExpectation(new RequireAnyRolePolicy("Editor"));

			// Act
			_expectationGroup.ApplyExpectation(expectation1);
			_expectationGroup.ApplyExpectation(expectation2);

			// Assert
			Assert.That(_expectationGroup.Expectations.Count(), Is.EqualTo(1));
		}

		private class NotImplementedExpectation : IExpectation {}
	}

	
}