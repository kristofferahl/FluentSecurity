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
	[Category("RequireAllRolesPolicySpec")]
	public class When_passing_null_to_the_constructor_of_RequireAllRolesPolicy
	{
		[Test]
		public void Should_throw_ArgumentNullException()
		{
			// Assert
			Assert.Throws<ArgumentNullException>(() => new RequireAllRolesPolicy(null));
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_passing_an_empty_array_to_the_constructor_of_RequireAllRolesPolicy
	{
		[Test]
		public void Should_throw_ArgumentException()
		{
			// Assert
			Assert.Throws<ArgumentException>(() => new RequireAllRolesPolicy(new object[0]));
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_passing_an_array_with_one_required_role_to_the_constructor_of_RequireAllRolesPolicy
	{
		[Test]
		public void Should_not_throw()
		{
			// Assert
			Assert.DoesNotThrow(() => new RequireAllRolesPolicy(new object[1]));
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_getting_the_required_roles_for_a_RequireAllRolesPolicy
	{
		[Test]
		public void Should_return_expected_roles()
		{
			// Arrange
			var expectedRoles = new List<object> { "Administrator", "Editor" }.ToArray();
			var policy = new RequireAllRolesPolicy(expectedRoles);

			// Act
			var rolesRequired = policy.RolesRequired;

			// Assert
			Assert.That(rolesRequired, Is.EqualTo(expectedRoles));
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_enforcing_security_for_a_RequireAllRolesPolicy
	{
		[Test]
		public void Should_resolve_authentication_status_and_roles_exactly_once()
		{
			// Arrange
			var roles = new object[1];
			var policy = new RequireAllRolesPolicy(roles);
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
			var policy = new RequireAllRolesPolicy(new object[1]);
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
			var policy = new RequireAllRolesPolicy(new object[1]);
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
			var policy = new RequireAllRolesPolicy("Role1", "Role2");
			const bool authenticated = true;
			var roles = new List<object> { "Role3", "Role4" }.ToArray();
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles);

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.True);
			Assert.That(result.Message, Is.EqualTo("Access requires all of the following roles: Role1 and Role2."));
		}

		[Test]
		public void Should_be_successful_when_isAuthenticated_is_true_and_user_has_all_required_roles()
		{
			// Arrange
			var requiredRoles = new List<object> {
				UserRole.Writer,
				UserRole.Publisher
			};

			var policy = new RequireAllRolesPolicy(requiredRoles.ToArray());

			const bool authenticated = true;
			var roles = new List<object> {
				UserRole.Writer,
				UserRole.Publisher
			};
			var context = TestDataFactory.CreateSecurityContext(authenticated, roles.ToArray());

			// Act
			var result = policy.Enforce(context);

			// Assert
			Assert.That(result.ViolationOccured, Is.False);
		}
	}

	[Category("RequireAllRolesPolicySpec")]
	public class When_doing_tostring_for_a_RequireAllRolesPolicy
	{
		[Test]
		public void Should_return_name_and_role()
		{
			// Arrange
			var roles = new List<object> { "Administrator" }.ToArray();
			var policy = new RequireAllRolesPolicy(roles);

			// Act
			var result = policy.ToString();

			// Assert
			Assert.That(result, Is.EqualTo("FluentSecurity.Policy.RequireAllRolesPolicy (Administrator)"));
		}

		[Test]
		public void Should_return_name_and_roles()
		{
			// Arrange
			var roles = new List<object> { "Writer", "Editor", "Administrator" }.ToArray();
			var policy = new RequireAllRolesPolicy(roles);

			// Act
			var result = policy.ToString();

			// Assert
			Assert.That(result, Is.EqualTo("FluentSecurity.Policy.RequireAllRolesPolicy (Writer and Editor and Administrator)"));
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_comparing_RequireAllRolesPolicy
	{
		[Test]
		public void Should_be_equal()
		{
			var instance1 = new RequireAllRolesPolicy("Editor");
			var instance2 = new RequireAllRolesPolicy("Editor");
			Assert.That(instance1.Equals(instance2), Is.True);
			Assert.That(instance1 == instance2, Is.True);
			Assert.That(instance1 != instance2, Is.False);

			var instance3 = new RequireAllRolesPolicy(UserRole.Writer);
			var instance4 = new RequireAllRolesPolicy(UserRole.Writer);
			Assert.That(instance3.Equals(instance4), Is.True);
			Assert.That(instance3 == instance4, Is.True);
			Assert.That(instance3 != instance4, Is.False);
		}

		[Test]
		public void Should_not_be_equal_when_comparing_to_null()
		{
			var instance = new RequireAllRolesPolicy("Editor");
			Assert.That(instance.Equals(null), Is.False);
		}

		[Test]
		public void Should_not_be_equal_when_roles_differ()
		{
			var instance1 = new RequireAllRolesPolicy("Editor");
			var instance2 = new RequireAllRolesPolicy("Writer");
			Assert.That(instance1.Equals(instance2), Is.False);
			Assert.That(instance1 == instance2, Is.False);
			Assert.That(instance1 != instance2, Is.True);

			var instance3 = new RequireAllRolesPolicy(UserRole.Publisher);
			var instance4 = new RequireAllRolesPolicy(UserRole.Owner);
			Assert.That(instance3.Equals(instance4), Is.False);
			Assert.That(instance3 == instance4, Is.False);
			Assert.That(instance3 != instance4, Is.True);
		}

		[Test]
		public void Should_not_be_equal_when_roles_count_differ()
		{
			var instance1 = new RequireAllRolesPolicy("Editor", "Writer");
			var instance2 = new RequireAllRolesPolicy("Writer");
			Assert.That(instance1.Equals(instance2), Is.False);
			Assert.That(instance1 == instance2, Is.False);
			Assert.That(instance1 != instance2, Is.True);

			var instance3 = new RequireAllRolesPolicy(UserRole.Owner, UserRole.Writer, UserRole.Publisher);
			var instance4 = new RequireAllRolesPolicy(UserRole.Owner);
			Assert.That(instance3.Equals(instance4), Is.False);
			Assert.That(instance3 == instance4, Is.False);
			Assert.That(instance3 != instance4, Is.True);
		}
	}

	[TestFixture]
	[Category("RequireAllRolesPolicySpec")]
	public class When_getting_the_hash_code_for_RequireAllRolesPolicy
	{
		[Test]
		public void Should_be_the_same()
		{
			var instance1 = new RequireAllRolesPolicy("Editor");
			var instance2 = new RequireAllRolesPolicy("Editor");
			Assert.That(instance1.GetHashCode(), Is.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireAllRolesPolicy(UserRole.Writer);
			var instance4 = new RequireAllRolesPolicy(UserRole.Writer);
			Assert.That(instance3.GetHashCode(), Is.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_roles_differ()
		{
			var instance1 = new RequireAllRolesPolicy("Editor");
			var instance2 = new RequireAllRolesPolicy("Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireAllRolesPolicy(UserRole.Publisher);
			var instance4 = new RequireAllRolesPolicy(UserRole.Owner);
			Assert.That(instance3.GetHashCode(), Is.Not.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_roles_count_differ()
		{
			var instance1 = new RequireAllRolesPolicy("Editor", "Writer");
			var instance2 = new RequireAllRolesPolicy("Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));

			var instance3 = new RequireAllRolesPolicy(UserRole.Owner, UserRole.Writer, UserRole.Publisher);
			var instance4 = new RequireAllRolesPolicy(UserRole.Owner);
			Assert.That(instance3.GetHashCode(), Is.Not.EqualTo(instance4.GetHashCode()));
		}

		[Test]
		public void Should_not_be_the_same_when_types_differ()
		{
			var instance1 = new RequireAnyRolePolicy("Editor", "Writer");
			var instance2 = new RequireAllRolesPolicy("Editor", "Writer");
			Assert.That(instance1.GetHashCode(), Is.Not.EqualTo(instance2.GetHashCode()));
		}
	}
}