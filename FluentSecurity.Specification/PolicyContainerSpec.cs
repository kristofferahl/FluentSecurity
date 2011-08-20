using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;
using Rhino.Mocks;
using MockRepository = Rhino.Mocks.MockRepository;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_an_invalid_policycontainer
	{
		private string _validControllerName;
		private string _validActionName;
		private IPolicyAppender _validPolicyAppender;

		[SetUp]
		public void SetUp()
		{
			_validControllerName = TestDataFactory.ValidControllerName;
			_validActionName = TestDataFactory.ValidActionName;
			_validPolicyAppender = TestDataFactory.CreateValidPolicyAppender();
		}

		[Test]
		public void Should_throw_ArgumentException_when_the_controllername_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(null, _validActionName, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_controllername_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(string.Empty, _validActionName, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, null, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(_validControllerName, string.Empty, _validPolicyAppender)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_policy_manager_is_null()
		{
			// Arrange
			const IPolicyAppender policyAppender = null;

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				new PolicyContainer(_validControllerName, _validActionName, policyAppender)
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
			return new PolicyContainer(_expectedControllerName, _expectedActionName, _expectedPolicyAppender);
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
		public void Should_invoke_the_isautheticated_and_roles_functions()
		{
			// Arrange
			var context = MockRepository.GenerateMock<ISecurityContext>();
			context.Expect(x => x.CurrenUserAuthenticated()).Return(true).Repeat.Once();
			context.Expect(x => x.CurrenUserRoles()).Return(new List<object> { UserRole.Owner }.ToArray()).Repeat.Once();
			context.Replay();
			
			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(new TestPolicy());

			// Act
			policyContainer.EnforcePolicies(context);

			// Assert
			context.VerifyAllExpectations();
		}

		[Test]
		public void Should_enforce_policies_with_context()
		{
			// Arrange
			var roles = new List<object> { UserRole.Owner }.ToArray();
			const bool isAuthenticated = true;

			var context = new Mock<ISecurityContext>();
			context.Setup(x => x.CurrenUserAuthenticated()).Returns(isAuthenticated);
			context.Setup(x => x.CurrenUserRoles()).Returns(roles);

			var policy = new Mock<ISecurityPolicy>();
			policy.Setup(x => x.Enforce(It.Is<ISecurityContext>(c => c.CurrenUserAuthenticated() == isAuthenticated && c.CurrenUserRoles() == roles))).Returns(PolicyResult.CreateSuccessResult(policy.Object));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(policy.Object);

			// Act
			policyContainer.EnforcePolicies(context.Object);

			// Assert
			policy.VerifyAll();
		}

		[Test]
		public void Should_return_results()
		{
			// Arrange
			var roles = new List<object> { UserRole.Owner }.ToArray();
			const bool isAuthenticated = true;
			const string failureOccured = "Failure occured";
			var context = TestDataFactory.CreateSecurityContext(isAuthenticated, roles);

			var policy = new Mock<ISecurityPolicy>();
			policy.Setup(x => x.Enforce(It.IsAny<ISecurityContext>())).Returns(PolicyResult.CreateFailureResult(policy.Object, failureOccured));

			var policyContainer = new PolicyContainer(TestDataFactory.ValidControllerName, TestDataFactory.ValidActionName, TestDataFactory.CreateValidPolicyAppender());
			policyContainer.AddPolicy(policy.Object);

			// Act
			var results = policyContainer.EnforcePolicies(context);

			// Assert
			Assert.That(results.Count(), Is.EqualTo(1));
			Assert.That(results.Single().ViolationOccured, Is.True);
			Assert.That(results.Single().Message, Is.EqualTo(failureOccured));
		}

		[Test]
		public void Should_throw_ConfigurationErrorsException_when_a_container_has_no_policies()
		{
			// Arrange
			ExceptionFactory.RequestDescriptionProvider = () => TestDataFactory.CreateRequestDescription();

			var context = TestDataFactory.CreateSecurityContext(false);
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(() => policyContainer.EnforcePolicies(context));
		}

		private class TestPolicy : ISecurityPolicy
		{
			public PolicyResult Enforce(ISecurityContext context)
			{
				var authenticated = context.CurrenUserAuthenticated();
				var roles = context.CurrenUserRoles();
				return PolicyResult.CreateSuccessResult(this);
			}
		}
	}
}