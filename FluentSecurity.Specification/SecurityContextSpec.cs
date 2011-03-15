using System;
using System.Collections.Generic;
using FluentSecurity.Specification.Helpers;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityContextSpec")]
	public class When_creating_a_security_context
	{
		[Test]
		public void Should_throw_when_security_has_not_been_configured()
		{
			// Arrange
			SecurityConfigurator.Reset();

			// Act
			var exception = Assert.Throws<InvalidOperationException>(() => SecurityContext.Current());

			// Assert
			Assert.That(exception.Message, Is.EqualTo("Security has not been configured!"));
		}

		[Test]
		public void Should_create_security_context_from_configuration()
		{
			// Arrange
			const bool status = true;
			var roles = new object[3];
			
			SecurityConfigurator.Configure(c =>
			{
				c.GetAuthenticationStatusFrom(() => status);
				c.GetRolesFrom(() => roles);
			});

			// Act
			var context = SecurityContext.Current();

			// Assert
			Assert.That(context.CurrenUserAuthenticated(), Is.EqualTo(status));
			Assert.That(context.CurrenUserRoles(), Is.EqualTo(roles));
		}

		[Test]
		public void Should_create_security_context_from_external_ioc()
		{
			// Arrange
			const bool status = true;
			var roles = new object[4];

			var iocContext = TestDataFactory.CreateSecurityContext(status, roles);
			FakeIoC.GetAllInstancesProvider = () => new List<ISecurityContext>
			{
			    iocContext
			};

			SecurityConfigurator.Configure(c =>
			{
				c.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});

			// Act
			var context = SecurityContext.Current();

			// Assert
			Assert.That(context.CurrenUserAuthenticated(), Is.EqualTo(status));
			Assert.That(context.CurrenUserRoles(), Is.EqualTo(roles));
			Assert.AreSame(context, iocContext);
		}
	}
}