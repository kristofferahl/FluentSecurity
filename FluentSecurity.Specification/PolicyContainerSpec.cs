using System;
using System.Collections.Generic;
using System.Configuration;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Fakes;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerSpec")]
	public class When_I_create_a_new_policycontainer
	{
		[Test]
		public void Should_throw_ArgumentException_when_the_controllername_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(null, "X", StaticHelper.IsAuthenticatedReturnsFalse, null)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_controllername_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer(string.Empty, "X", StaticHelper.IsAuthenticatedReturnsFalse, null)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_null()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer("X", null, StaticHelper.IsAuthenticatedReturnsFalse, null)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_actionname_is_empty()
		{
			Assert.Throws<ArgumentException>(() =>
				new PolicyContainer("X", string.Empty, StaticHelper.IsAuthenticatedReturnsFalse, null)
			);
		}

		[Test]
		public void Should_throw_ArgumentException_when_isauthenticatedfunction_is_null()
		{
			// Arrange
			const Func<bool> isAuthenticatedFunction = null;
			
			// Act & Assert
			Assert.Throws<ArgumentNullException>(() =>
				new PolicyContainer("X", "X", isAuthenticatedFunction, null)
			);
		}
	}

	[TestFixture]
	[Category("PolicContainerExtensionsSpec")]
	public class When_adding_two_policies_of_the_same_type_to_a_policycontainer
	{
		[Test]
		public void Should_throw_InvalidOperationException()
		{
			// Arrange
			var policyContainer = new PolicyContainer("X", "X", StaticHelper.IsAuthenticatedReturnsFalse, null);
			
			// Act & Assert
			Assert.Throws<InvalidOperationException>(() =>
				policyContainer
					.AddPolicy(new DenyAnonymousAccessPolicy())
					.AddPolicy(new DenyAnonymousAccessPolicy())
			);
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
			var helper = MockRepository.GenerateMock<Helper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(true).Repeat.Once();
			helper.Replay();

			var policyContainer = new PolicyContainer("X", "X", helper.IsAuthenticatedReturnsTrue, null);
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
			var helper = MockRepository.GenerateMock<Helper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(true).Repeat.Once();
			helper.Expect(x => x.GetRoles()).Return(new List<object> { UserRole.Owner }.ToArray()).Repeat.Once();
			helper.Replay();

			var policyContainer = new PolicyContainer("X", "X", helper.IsAuthenticatedReturnsTrue, helper.GetRoles);
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

			var helper = MockRepository.GenerateMock<Helper>();
			helper.Expect(x => x.IsAuthenticatedReturnsTrue()).Return(isAuthenticated).Repeat.Once();
			helper.Expect(x => x.GetRoles()).Return(roles).Repeat.Once();
			helper.Replay();

			var mockRepository = new MockRepository();
			var policy =  mockRepository.StrictMock<FakePolicy>();
			policy.Expect(x => x.Enforce(isAuthenticated, roles)).Repeat.Once();
			policy.Replay();

			var policyContainer = new PolicyContainer("X", "X", helper.IsAuthenticatedReturnsTrue, helper.GetRoles);
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
			var policyContainer = new PolicyContainer("X", "X", StaticHelper.IsAuthenticatedReturnsTrue, StaticHelper.GetRolesExcludingOwner);

			// Act & Assert
			Assert.Throws<ConfigurationErrorsException>(policyContainer.EnforcePolicies);
		}
	}
}