using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using FluentSecurity.Policy;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_passing_null_to_the_constructor_of_RequireRolePolicy
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new RequireRolePolicy(null));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_passing_an_empty_array_to_the_constructor_of_RequireRolePolicy
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new RequireRolePolicy(new object[0]));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_passing_an_array_with_one_required_role_to_the_constructor_of_RequireRolePolicy
	{
		[Test]
		public void Should_not_throw()
		{
			// Assert
			Assert.DoesNotThrow(() => new RequireRolePolicy(new object[1]));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_getting_the_required_roles_for_an_RequiredRolePolicy
	{
		[Test]
		public void Should_return_null()
		{
			// Arrange
			var expectedRoles = new List<object> { "Administrator", "Editor" }.ToArray();
			var policy = new RequireRolePolicy(expectedRoles);

			// Act
			object[] rolesRequired = policy.RolesRequired;

			// Assert
			Assert.That(rolesRequired, Is.EqualTo(expectedRoles));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_enforcing_security_for_a_RequireRolePolicy
	{
		[Test]
		public void Should_throw_SecurityException_when_isAuthenticated_is_false()
		{
			// Arrange
			var policy = new RequireRolePolicy(new object[1]);
			const bool authenticated = false;

			// Assert
			Assert.Throws<SecurityException>(() => policy.Enforce(authenticated, null));
		}

		[Test]
		public void Should_throw_SecurityException_when_isAuthenticated_is_true_and_roles_are_null()
		{
			// Arrange
			var policy = new RequireRolePolicy(new object[1]);
			const bool authenticated = true;
			object[] roles = null;

			// Assert
			Assert.Throws<SecurityException>(() => policy.Enforce(authenticated, roles));
		}

		[Test]
		public void Should_not_throw_when_isAuthenticated_is_true_and_user_has_at_least_one_matching_role()
		{
			// Arrange
			var requiredRoles = new List<object> {
				UserRole.Writer,
				UserRole.Publisher
			};

			var policy = new RequireRolePolicy(requiredRoles.ToArray());

			const bool authenticated = true;
			var roles = new List<object> {
				UserRole.Writer
			};


			// Assert
			Assert.DoesNotThrow(() => policy.Enforce(authenticated, roles.ToArray()));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_comparing_RequireRolePolicy
	{
		[Test]
		public void Should_be_equal()
		{
			var instance1 = new RequireRolePolicy(new List<object> { "Editor" }.ToArray());
			var instance2 = new RequireRolePolicy(new List<object> { "Editor" }.ToArray());
			Assert.That(instance1.Equals(instance2), Is.True);

			var instance3 = new RequireRolePolicy(new List<object> { UserRole.Writer }.ToArray());
			var instance4 = new RequireRolePolicy(new List<object> { UserRole.Writer }.ToArray());
			Assert.That(instance3.Equals(instance4), Is.True);
		}

		[Test]
		public void Should_not_be_equal()
		{
			var instance1 = new RequireRolePolicy(new List<object> { "Editor" }.ToArray());
			var instance2 = new RequireRolePolicy(new List<object> { "Writer" }.ToArray());
			Assert.That(instance1.Equals(instance2), Is.False);

			var instance3 = new RequireRolePolicy(new List<object> { UserRole.Publisher }.ToArray());
			var instance4 = new RequireRolePolicy(new List<object> { UserRole.Owner }.ToArray());
			Assert.That(instance3.Equals(instance4), Is.False);
		}
	}
}