using System.Collections.Generic;
using FluentSecurity.Policy;
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

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles));
		}

		[Test]
		public void Should_not_throw_when_isAuthenticated_is_true()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const object[] roles = null;

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles));
		}

		[Test]
		public void Should_not_throw_when_roles_is_null()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const object[] roles = null;

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles));
		}

		[Test]
		public void Should_not_throw_when_roles_is_empty()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			var roles = new object[0];

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles));
		}

		[Test]
		public void Should_not_throw_when_roles_are_passed()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			object[] roles = new List<object> { "Administrator", "Editor", "Reader" }.ToArray();

			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles));
		}
	}

	[TestFixture]
	[Category("IgnorePolicySpec")]
	public class When_getting_the_required_roles_for_an_IgnorePolicy
	{
		[Test]
		public void Should_return_null()
		{
			// Arrange
			var policy = new IgnorePolicy();

			// Act
			object[] rolesRequired = policy.RolesRequired;

			// Assert
			Assert.That(rolesRequired, Is.Null);
		}
	}
}