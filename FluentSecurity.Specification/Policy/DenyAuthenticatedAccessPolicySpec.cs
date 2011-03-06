using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("DenyAuthenticatedAccessPolicySpec")]
	public class When_enforcing_security_for_a_DenyAuthenticatedAccessPolicy
	{
		[Test]
		public void Should_throw_when_the_user_is_authenticated()
		{
			// Arrange
			var policy = new DenyAuthenticatedAccessPolicy();
			const bool authenticated = true;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act & Assert
			Assert.Throws<PolicyViolationException<DenyAuthenticatedAccessPolicy>>(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_the_user_is_anonymous()
		{
			// Arrange
			var policy = new DenyAuthenticatedAccessPolicy();
			const bool authenticated = false;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act & Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}
	}
}