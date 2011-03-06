using System;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityContextSpec")]
	public class When_creating_a_security_context
	{
		[Test]
		public void Should_throw_when_IsAuthenticated_is_null()
		{
			// Arrange
			Func<bool> isAuthenticated = null;
			Func<object[]> roles = null;

			// Act & assert
			Assert.Throws<ArgumentNullException>(() => new SecurityContext(isAuthenticated, roles));
		}

		[Test]
		public void Should_have_status_and_roles()
		{
			// Arrange
			Func<bool> isAuthenticated = () => true;
			Func<object[]> roles = StaticHelper.GetRolesExcludingOwner;
			
			// Act
			var context = new SecurityContext(isAuthenticated, roles);

			// Assert
			Assert.That(context.CurrenUserAuthenticated(), Is.EqualTo(isAuthenticated()));
			Assert.That(context.CurrenUserRoles(), Is.EqualTo(roles()));
		}

		[Test]
		public void Should_have_status_and_no_roles()
		{
			// Arrange
			Func<bool> isAuthenticated = () => false;
			Func<object[]> roles = null;

			// Act
			var context = new SecurityContext(isAuthenticated, roles);

			// Assert
			Assert.That(context.CurrenUserAuthenticated(), Is.False);
			Assert.That(context.CurrenUserRoles(), Is.Null);
		}
	}
}