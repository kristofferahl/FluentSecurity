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
		public void Should_be_successful_when_isAuthenticated_is_false()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = false;
			const IEnumerable<object> roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_be_successful_when_isAuthenticated_is_true()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const IEnumerable<object> roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_be_successful_when_roles_is_null()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			const IEnumerable<object> roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_be_successful_when_roles_is_empty()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			var roles = new object[0];
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}

		[Test]
		public void Should_be_successful_when_roles_are_passed()
		{
			// Arrange
			var policy = new IgnorePolicy();
			const bool authenticated = true;
			var roles = new List<object> { "Administrator", "Editor", "Reader" }.ToArray();
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}
	}
}