using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Fakes;
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
	public class When_passing_an__array_with_one_item_to_the_constructor_of_RequireRolePolicy
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.DoesNotThrow(() => new RequireRolePolicy(new object[1]));
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
}