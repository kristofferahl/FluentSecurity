using System.Security;
using FluentSecurity.Policy;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("DenyAnonymousAccessPolicySpec")]
	public class When_enforcing_security_for_an_DenyAnonymousAccessPolicy
	{
		[Test]
		public void Should_throw_SecurityException_when_isAuthenticated_is_false()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = false;

			// Assert
			Assert.Throws<SecurityException>(() => policy.Enforce(authenticated, null));
		}

		[Test]
		public void Should_not_throw_when_isAuthenticated_is_true()
		{
			// Arrange
			var policy = new DenyAnonymousAccessPolicy();
			const bool authenticated = true;

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, null));
		}
	}
}