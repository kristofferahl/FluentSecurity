using System.Linq;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Fakes;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("PolicyContainerExtensionsSpec")]
	public class When_adding_a_DenyAnonymousAccessPolicy_to_a_policycontainer
	{
		[Test]
		public void Should_have_a_DenyAnonymousAccessPolicy()
		{
			// Arrange
			var policyContainer = new PolicyContainer("C", "A", StaticHelper.IsAuthenticatedReturnsFalse, null);

			// Act
			policyContainer.DenyAnonymousAccess();

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(DenyAnonymousAccessPolicy))).Single();
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
			var policyContainer = new PolicyContainer("C", "A", StaticHelper.IsAuthenticatedReturnsFalse, null);

			// Act
			policyContainer.RequireRole(UserRole.Writer);

			// Assert
			var securityPolicy = policyContainer.GetPolicies().Where(x => x.GetType().Equals(typeof(RequireRolePolicy))).Single();
			Assert.That(securityPolicy, Is.Not.Null);
		}
	}
}