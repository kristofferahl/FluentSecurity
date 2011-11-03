using System.Collections.Generic;
using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
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