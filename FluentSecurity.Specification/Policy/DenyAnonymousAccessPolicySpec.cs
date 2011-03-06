using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
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
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act & Assert
			Assert.Throws<PolicyViolationException<DenyAnonymousAccessPolicy>>(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_the_user_is_authenticated()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = true;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act & Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}
	}
}