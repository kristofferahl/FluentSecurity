using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("DenyAnonymousAccessPolicySpec")]
	public class When_enforcing_security_for_a_DenyAnonymousAccessPolicy
	{
		[Test]
		public void Should_throw_when_the_user_is_anonymous()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = false;

			// Act & Assert
			Assert.Throws<PolicyViolationException<DenyAnonymousAccessPolicy>>(() => policy.Enforce(authenticated, null));
		}

		[Test]
		public void Should_not_throw_when_the_user_is_authenticated()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = true;

			// Act & Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, null));
		}
	}

	[TestFixture]
	[Category("DenyAnonymousAccessPolicySpec")]
	public class When_getting_the_required_roles_for_a_DenyAnonymousAccessPolicy
	{
		[Test]
		public void Should_return_null()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();

			// Assert
			Assert.That(policy.RolesRequired, Is.Null);
		}
	}
}