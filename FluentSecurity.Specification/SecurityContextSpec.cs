using System;
using System.Collections.Generic;
using System.Dynamic;
using FluentSecurity.Configuration;
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
			var exception = Assert.Throws<InvalidOperationException>(() => { var x = SecurityContext.Current; });

			// Assert
			Assert.That(exception.Message, Is.EqualTo("Security has not been configured!"));
		}

		[Test]
		public void Should_create_security_context_from_configuration()
		{
			// Arrange
			const bool status = true;
			var roles = new object[3];
			
			SecurityConfigurator.Configure<MvcConfiguration>(c =>
			{
				c.GetAuthenticationStatusFrom(() => status);
				c.GetRolesFrom(() => roles);
			});

			// Act
			var context = SecurityContext.Current;

			// Assert
			Assert.That(context.Id, Is.Not.EqualTo(Guid.Empty));
			Assert.That(context.Data, Is.TypeOf(typeof(ExpandoObject)));
			Assert.That(context.CurrentUserIsAuthenticated(), Is.EqualTo(status));
			Assert.That(context.CurrentUserRoles(), Is.EqualTo(roles));
			Assert.That(context.Runtime, Is.EqualTo(SecurityConfiguration.Get<MvcConfiguration>().Runtime));
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

			SecurityConfigurator.Configure<MvcConfiguration>(c =>
			{
				c.ResolveServicesUsing(FakeIoC.GetAllInstances);
			});

			// Act
			var context = SecurityContext.Current;

			// Assert
			Assert.That(context.Data, Is.TypeOf(typeof(ExpandoObject)));
			Assert.That(context.CurrentUserIsAuthenticated(), Is.EqualTo(status));
			Assert.That(context.CurrentUserRoles(), Is.EqualTo(roles));
			Assert.That(context.Runtime, Is.Not.Null);
			Assert.AreSame(context, iocContext);
		}
	}
}