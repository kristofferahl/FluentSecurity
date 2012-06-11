using System;
using System.Collections.Generic;
using FluentSecurity.Policy;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using Moq;
using NUnit.Framework;

namespace FluentSecurity.Specification.Policy
{
	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_passing_null_to_the_constructor_of_RequireRolePolicy
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Assert
			Assert.Throws<ArgumentNullException>(() => new RequireRolePolicy(null));
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
	public class When_getting_the_required_roles_for_a_RequireRolePolicy
	{
		[Test]
		public void Should_return_expected_roles()
		{
			// Arrange
			var expectedRoles = new List<object> { "Administrator", "Editor" }.ToArray();
			var policy = new RequireRolePolicy(expectedRoles);

			// Act
			var rolesRequired = policy.RolesRequired;

			// Assert
			Assert.That(rolesRequired, Is.EqualTo(expectedRoles));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_enforcing_security_for_a_RequireRolePolicy
	{
		[Test]
		public void Should_resolve_authentication_status_and_roles_exactly_once()
		{
			// Arrange
			var roles = new object[1];
			var policy = new RequireRolePolicy(roles);
			var context = new Mock<ISecurityContext>();
			context.Setup(x => x.CurrentUserIsAuthenticated()).Returns(true);
			context.Setup(x => x.CurrentUserRoles()).Returns(roles);

			// Act
			var result = policy.Enforce(context.Object);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
			context.Verify(x => x.CurrentUserIsAuthenticated(), Times.Exactly(1), "The authentication status should be resolved at most once.");
			context.Verify(x => x.CurrentUserRoles(), Times.Exactly(1), "The roles should be resolved at most once.");
		}

		[Test]
		public void Should_not_be_successful_when_isAuthenticated_is_false()
		{
			// Arrange
			var policy = new RequireRolePolicy(new object[1]);
			const bool authenticated = false;
			var context = TestDataFactory.CreateSecurityContext(authenticated);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Anonymous access denied"));
		}

		[Test]
		public void Should_not_be_successful_when_isAuthenticated_is_true_and_roles_are_null()
		{
			// Arrange
			var policy = new RequireRolePolicy(new object[1]);
			const bool authenticated = true;
			IEnumerable<object> roles = null;
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Access denied"));
		}

		[Test]
		public void Should_not_be_successful_when_isAuthenticated_is_true_and_roles_does_not_match()
		{
			// Arrange
			var policy = new RequireRolePolicy("Role1", "Role2");
			const bool authenticated = true;
			var roles = new List<object> { "Role3", "Role4" }.ToArray();
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Access requires one of the following roles: Role1 or Role2."));
		}

		[Test]
		public void Should_be_successful_when_isAuthenticated_is_true_and_user_has_at_least_one_matching_role()
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
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles.ToArray());

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}
	}

	[Category("RequireAllRolesPolicySpec")]
	public class When_doing_tostring_for_a_RequireRolePolicy
	{
		[Test]
		public void Should_return_name_and_role()
		{
			// Arrange
			var roles = new List<object> { "Administrator" }.ToArray();
			var policy = new RequireRolePolicy(roles);

			// Act
			var result = policy.ToString();

			// Assert
			Assert.That(result, Is.EqualTo("FluentSecurity.Policy.RequireRolePolicy (Administrator)"));
		}

		[Test]
		public void Should_return_name_and_roles()
		{
			// Arrange
			var roles = new List<object> { "Writer", "Editor", "Administrator" }.ToArray();
			var policy = new RequireRolePolicy(roles);

			// Act
			var result = policy.ToString();

			// Assert
			Assert.That(result, Is.EqualTo("FluentSecurity.Policy.RequireRolePolicy (Writer or Editor or Administrator)"));
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_comparing_RequireRolePolicy
	{
		[Test]
		public void Should_be_equal()
		{
			var instance1 = new RequireRolePolicy("Editor");
			var instance2 = new RequireRolePolicy("Editor");
			Assert.That(instance1.Equals(instance2), Is.True);
			Assert.That(instance1 == instance2, Is.True);
			Assert.That(instance1 != instance2, Is.False);

			var instance3 = new RequireRolePolicy(UserRole.Writer);
			var instance4 = new RequireRolePolicy(UserRole.Writer);
			Assert.That(instance3.Equals(instance4), Is.True);
			Assert.That(instance3 == instance4, Is.True);
			Assert.That(instance3 != instance4, Is.False);
		}

		[Test]
		public void Should_not_be_equal_when_comparing_to_null()
		{
			var instance = new RequireRolePolicy("Editor");
			Assert.That(instance.Equals(null), Is.False);
		}

		[Test]
		public void Should_not_be_equal_when_roles_differ()
		{
			var instance1 = new RequireRolePolicy("Editor");
			var instance2 = new RequireRolePolicy("Writer");
			Assert.That(instance1.Equals(instance2), Is.False);
			Assert.That(instance1 == instance2, Is.False);
			Assert.That(instance1 != instance2, Is.True);

			var instance3 = new RequireRolePolicy(UserRole.Publisher);
			var instance4 = new RequireRolePolicy(UserRole.Owner);
			Assert.That(instance3.Equals(instance4), Is.False);
			Assert.That(instance3 == instance4, Is.False);
			Assert.That(instance3 != instance4, Is.True);
		}

		[Test]
		public void Should_not_be_equal_when_roles_count_differ()
		{
			var instance1 = new RequireRolePolicy("Editor", "Writer");
			var instance2 = new RequireRolePolicy("Writer");
			Assert.That(instance1.Equals(instance2), Is.False);
			Assert.That(instance1 == instance2, Is.False);
			Assert.That(instance1 != instance2, Is.True);

			var instance3 = new RequireRolePolicy(UserRole.Owner, UserRole.Writer, UserRole.Publisher);
			var instance4 = new RequireRolePolicy(UserRole.Owner);
			Assert.That(instance3.Equals(instance4), Is.False);
			Assert.That(instance3 == instance4, Is.False);
			Assert.That(instance3 != instance4, Is.True);
		}
	}

	[TestFixture]
	[Category("RequireRolePolicySpec")]
	public class When_getting_the_hash_code_for_RequireRolePolicy
	{
		[Test]
		public void Should_be_the_same()
		{
			var instance1 = new RequireRolePolicy("Editor");
			var instance2 = new RequireRolePolicy("Editor");
			Assert.That(instance1.GetHashCode(), Is.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireRolePolicy(UserRole.Writer);
			var instance4 = new RequireRolePolicy(UserRole.Writer);
			Assert.That(instance3.GetHashCode(), Is.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_roles_differ()
		{
			var instance1 = new RequireRolePolicy("Editor");
			var instance2 = new RequireRolePolicy("Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireRolePolicy(UserRole.Publisher);
			var instance4 = new RequireRolePolicy(UserRole.Owner);
			Assert.That(instance3.GetHashCode(), Is.Not.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_roles_count_differ()
		{
			var instance1 = new RequireRolePolicy("Editor", "Writer");
			var instance2 = new RequireRolePolicy("Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireRolePolicy(UserRole.Owner, UserRole.Writer, UserRole.Publisher);
			var instance4 = new RequireRolePolicy(UserRole.Owner);
			Assert.That(instance3.GetHashCode(), Is.Not.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_types_differ()
		{
			var instance1 = new RequireAllRolesPolicy("Editor", "Writer");
			var instance2 = new RequireRolePolicy("Editor", "Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));
		}
	}
}