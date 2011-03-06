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
		public void Should_not_be_successful_when_the_user_is_authenticated()
		{
			// Arrange
			var policy = new DenyAuthenticatedAccessPolicy();
			const bool authenticated = true;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Authenticated access denied"));
		}

		[Test]
		public void Should_be_successful_when_the_user_is_anonymous()
		{
			// Arrange
			var policy = new DenyAuthenticatedAccessPolicy();
			const bool authenticated = false;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}
	}
}