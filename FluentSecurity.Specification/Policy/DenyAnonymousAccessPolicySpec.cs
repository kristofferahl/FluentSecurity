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
		public void Should_not_be_successful_when_the_user_is_anonymous()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = false;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Anonymous access denied"));
		}

		[Test]
		public void Should_be_successful_when_the_user_is_authenticated()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = true;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}
	}
}