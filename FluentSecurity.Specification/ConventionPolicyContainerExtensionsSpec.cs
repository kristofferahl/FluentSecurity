using System;
using System.Collections.Generic;
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
	public class When_adding_a_DelegatePolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_complex_DelegatePolicy()
		{
			// Arrange
			const string policyName = "Name1";
			Func<DelegateSecurityContext, PolicyResult> policyDelegate = context => PolicyResult.CreateSuccessResult(context.Policy);
			Func<PolicyViolationException, ActionResult> violationHandlerDelegate = exception => new EmptyResult();

			// Act
			_conventionPolicyContainer.DelegatePolicy(policyName, policyDelegate, violationHandlerDelegate);

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(DelegatePolicy))).Single() as DelegatePolicy;
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
			var securityContext = new DelegateSecurityContext(new Mock<ISecurityPolicy>().Object, new Mock<ISecurityContext>().Object);

			// Act
			_conventionPolicyContainer.DelegatePolicy(policyName, policyDelegate, violationHandlerDelegate, failureMessage);

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(DelegatePolicy))).Single() as DelegatePolicy;
			Assert.That(securityPolicy, Is.Not.Null);
			Assert.That(securityPolicy.Name, Is.EqualTo(policyName));

			var policyResult = securityPolicy.Policy.Invoke(securityContext);
			Assert.That(policyResult.Message, Is.EqualTo(failureMessage));
			Assert.That(policyResult.ViolationOccured, Is.True);

			Assert.That(securityPolicy.ViolationHandler, Is.EqualTo(violationHandlerDelegate));
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerExtensionsSpec")]
	public class When_adding_a_DenyAnonymousAccessPolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_DenyAnonymousAccessPolicy()
		{
			// Act
			_conventionPolicyContainer.DenyAnonymousAccess();

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(DenyAnonymousAccessPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerExtensionsSpec")]
	public class When_adding_a_DenyAuthenticatedAccessPolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_DenyAuthenticatedAccessPolicy()
		{
			// Act
			_conventionPolicyContainer.DenyAuthenticatedAccess();

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(DenyAuthenticatedAccessPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerExtensionsSpec")]
	public class When_adding_a_RequireRolePolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_RequireRolePolicy()
		{
			// Arrange

			// Act
			_conventionPolicyContainer.RequireRole(UserRole.Owner);

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(RequireRolePolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerExtensionsSpec")]
	public class When_adding_a_RequireAllRolesPolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_RequireAllRolesPolicy()
		{
			// Act
			_conventionPolicyContainer.RequireAllRoles(UserRole.Owner);

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(RequireAllRolesPolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	[TestFixture]
	[Category("ConventionPolicyContainerExtensionsSpec")]
	public class When_adding_a_IgnorePolicy_to_a_conventionpolicycontainer : with_ConventionPolicyContainer
	{
		[Test]
		public void Should_have_a_IgnorePolicy()
		{
			// Arrange

			// Act
			_conventionPolicyContainer.Ignore();

			// Assert
			var securityPolicy = _policyContainers[0].GetPolicies().Where(x => x.GetType().Equals(typeof(IgnorePolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}

	public abstract class with_ConventionPolicyContainer
	{
		protected ConventionPolicyContainer _conventionPolicyContainer;
		protected IList<IPolicyContainer> _policyContainers;

		[SetUp]
		public void SetUp()
		{
			// Arrange
			_policyContainers = new List<IPolicyContainer>
			{
				TestDataFactory.CreateValidPolicyContainer()
			};
			_conventionPolicyContainer = new ConventionPolicyContainer(_policyContainers);
		}
	}
}