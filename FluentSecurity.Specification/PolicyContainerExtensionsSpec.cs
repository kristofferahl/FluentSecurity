using System;
using System.Linq;
using System.Web.Mvc;
using FluentSecurity.Policy;
using FluentSecurity.Policy.Contexts;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_DelegatePolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_complex_DelegatePolicy()
		{
			// Arrange
			const string policyName = "Name1";
			Func<DelegateSecurityContext, PolicyResult> policyDelegate = context => PolicyResult.CreateSuccessResult(context.Policy);
			Func<PolicyViolationException, ActionResult> violationHandlerDelegate = exception => new EmptyResult();
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.DelegatePolicy(policyName, policyDelegate, violationHandlerDelegate);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Single(x => x.GetType() == typeof(DelegatePolicy)) as DelegatePolicy;
			Assert.That(securityPolicy, Is.Not.Null);
			Assert.That(securityPolicy.Name, Is.EqualTo(policyName));
			Assert.That(securityPolicy.Policy, Is.EqualTo(policyDelegate));
			Assert.That(securityPolicy.ViolationHandler, Is.EqualTo(violationHandlerDelegate));
		}

		[Test]
		public void Should_have_a_simple_DelegatePolicy()
		{
			// Arrange
			const string policyName = "Name1";
			const string failureMessage = "Some error message";
			Func<DelegateSecurityContext, bool> policyDelegate = context => false;
			Func<PolicyViolationException, ActionResult> violationHandlerDelegate = exception => new EmptyResult();
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();
			var securityContext = new DelegateSecurityContext(new Mock<ISecurityPolicy>().Object, new Mock<ISecurityContext>().Object);

			// Act
			policyContainer.DelegatePolicy(policyName, policyDelegate, violationHandlerDelegate, failureMessage);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(DelegatePolicy))).Single() as DelegatePolicy;
			Assert.That(securityPolicy, Is.Not.Null);
			Assert.That(securityPolicy.Name, Is.EqualTo(policyName));
			
			var policyResult = securityPolicy.Policy.Invoke(securityContext);
			Assert.That(policyResult.Message, Is.EqualTo(failureMessage));
			Assert.That(policyResult.ViolationOccured, Is.True);
			
			Assert.That(securityPolicy.ViolationHandler, Is.EqualTo(violationHandlerDelegate));
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_DenyAnonymousAccessPolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.DenyAnonymousAccess();

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(DenyAnonymousAccessPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_DenyAuthenticatedAccessPolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_DenyAuthenticatedAccessPolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.DenyAuthenticatedAccess();

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(DenyAuthenticatedAccessPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_RequireRolePolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_RequireRolePolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.RequireRole(UserRole.Writer);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Single(x => x.GetType() == typeof(RequireRolePolicy));
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_RequireAnyRolePolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_RequireAnyRolePolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.RequireAnyRole(UserRole.Writer);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Single(x => x.GetType() == typeof(RequireAnyRolePolicy));
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_RequireAllRolesPolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_RequireAllRolesPolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.RequireAllRoles(UserRole.Writer);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(RequireAllRolesPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_IgnorePolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_IgnorePolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.Ignore();

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(IgnorePolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_IgnorePolicy_to_a_policycontainer_using_AllowAny
	{
		[Test]
		public void Should_have_a_IgnorePolicy()
		{
			// Arrange
			var policyContainer = TestDataFactory.CreateValidPolicyContainer();

			// Act
			policyContainer.AllowAny();

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(IgnorePolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}
}