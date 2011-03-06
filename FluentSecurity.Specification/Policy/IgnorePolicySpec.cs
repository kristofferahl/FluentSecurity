using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("IgnorePolicySpec")]
	public class When_enforcing_security_for_a_IgnorePolicy
	{
		[Test]
		public void Should_not_throw_when_isAuthenticated_is_false()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = false;
			const object[] roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_isAuthenticated_is_true()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const object[] roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_roles_is_null()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const object[] roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_roles_is_empty()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			var roles = new object[0];
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}

		[Test]
		public void Should_not_throw_when_roles_are_passed()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			var roles = new List<object> { "Administrator", "Editor", "Reader" }.ToArray();
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(context));
		}
	}
}