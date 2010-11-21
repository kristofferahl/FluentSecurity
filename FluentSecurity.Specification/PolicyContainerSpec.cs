using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Fakes;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_an_invalid_policycontainer
	{
		private string _validControllerName;
		private string _validActionName;
		private Func<bool> _validIsAuthenticatedFunction;
		private Func<object[]> _validRolesFunction;
		private IPolicyAppender _validPolicyAppender;

		[SetUp]
		public void SetUp()
		{
			_validControllerName = TestDataFactory.ValidControllerName;
			_validActionName = TestDataFactory.ValidActionName;
			_validIsAuthenticatedFunction = TestDataFactory.ValidIsAuthenticatedFunction;
			_validRolesFunction = TestDataFactory.ValidRolesFunction;
			_validPolicyAppender = TestDataFactory.CreateValidPolicyAppender();
		}

		[Test]
		public void Should_throw_ArgumentException_when_the_controllername_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(null, _validActionName, _validIsAuthenticatedFunction, _validRolesFunction, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_controllername_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(string.Empty, _validActionName, _validIsAuthenticatedFunction, _validRolesFunction, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, null, _validIsAuthenticatedFunction, _validRolesFunction, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, string.Empty, _validIsAuthenticatedFunction, _validRolesFunction, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_isauthenticatedfunction_is_null()
		{
			// Arrange
			const Func<bool> isAuthenticatedFunction = null;
			
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				new PolicyContainer(_validControllerName, _validActionName, isAuthenticatedFunction, _validRolesFunction, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_policy_manager_is_null()
		{
			// Arrange
			const IPolicyAppender policyAppender = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				new PolicyContainer(_validControllerName, _validActionName, _validIsAuthenticatedFunction, _validRolesFunction, policyAppender)
			);
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_a_valid_PolicyContainer
	{
		private IPolicyAppender _expectedPolicyAppender = new DefaultPolicyAppender();
		private string _expectedControllerName = "SomeController";
		private string _expectedActionName = "SomeAction";

		[SetUp]
		public void SetUp()
		{
			_expectedControllerName = TestDataFactory.ValidControllerName;
			_expectedActionName = TestDataFactory.ValidActionName;
			_expectedPolicyAppender = TestDataFactory.CreateValidPolicyAppender();
		}

		private IPolicyContainer Because()
		{
			return new PolicyContainer(_expectedControllerName, _expectedActionName, TestDataFactory.ValidIsAuthenticatedFunction, TestDataFactory.ValidRolesFunction, _expectedPolicyAppender);
		}

		[Test]
		public void Should_have_controller_name_set_to_SomeController()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.ControllerName, Is.EqualTo(_expectedControllerName));
		}

		[Test]
		public void Should_have_action_name_set_to_SomeAction()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.ActionName, Is.EqualTo(_expectedActionName));
		}

		[Test]
		public void Should_have_Manager_set_to_DefaultPolicyAppender()
		{
			// Act
			var policyContainer = Because();

			// Assert
			Assert.That(policyContainer.PolicyAppender, Is.EqualTo(_expectedPolicyAppender));
		}

	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_two_policies_of_the_same_type_to_a_policycontainer
	{
		private PolicyContainer _policyContainer;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyContainer = TestDataFactory.CreateValidPolicyContainer();
		}

		private void Because()
		{
			_policyContainer
				.AddPolicy(new DenyAnonymousAccessPolicy())
				.AddPolicy(new DenyAnonymousAccessPolicy());
		}

		[Test]
		public void Should_have_1_policy()
		{
			// Act
			Because();

			// Assert
			Assert.That(_policyContainer.GetPolicies().Count(), Is.EqualTo(1));
		}
	}

	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_encforcing_policies
	{
		[Test]
		public void Should_invoke_the_isautheticated_function()
		{
			// Arrange
			var helper = MockRepository.GenerateMock<MockedHelper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(true).Repeat.Once();
			helper.Replay();

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, helper.IsAuthenticatedReturnsTrue, TestDataFactory.ValidRolesFunction, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(new DenyAnonymousAccessPolicy());

			// Act
			policyContainer.EnforcePolicies();
			
			// Assert
			helper.VerifyAllExpectations();			
		}

		[Test]
		public void Should_invoke_the_roles_function()
		{
			// Arrange
			var helper = MockRepository.GenerateMock<MockedHelper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(true).Repeat.Once();
			helper.Expect(x => x.GetRoles()).Return(new List<object> { UserRole.Owner }.ToArray()).Repeat.Once();
			helper.Replay();

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, helper.IsAuthenticatedReturnsTrue, helper.GetRoles, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(new DenyAnonymousAccessPolicy());

			// Act
			policyContainer.EnforcePolicies();

			// Assert
			helper.VerifyAllExpectations();
		}

		[Test]
		public void Should_enforce_policies_with_status_and_roles()
		{
			// Arrange
			var roles = new List<object> { UserRole.Owner }.ToArray();
			const bool isAuthenticated = true;

			var helper = MockRepository.GenerateMock<MockedHelper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(isAuthenticated).Repeat.Once();
			helper.Expect(x => x.GetRoles()).Return(roles).Repeat.Once();
			helper.Replay();

			var mockRepository = new MockRepository();
			var policy =  mockRepository.StrictMock<FakePolicy>();
			policy.Expect(x => x.Enforce(isAuthenticated, roles)).Repeat.Once();
			policy.Replay();

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, helper.IsAuthenticatedReturnsTrue, helper.GetRoles, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(policy);

			// Act
			policyContainer.EnforcePolicies();

			// Assert
			policy.VerifyAllExpectations();
		}

		[Test]
		public void Should_throw_ConfigurationErrorsException_when_a_container_has_no_policies()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(policyContainer.EnforcePolicies);
		}
	}
}