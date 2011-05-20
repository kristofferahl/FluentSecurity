using System;
using System.Linq;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_creating_a_new_SecurityConfiguration
	{
		private static SecurityConfiguration _securityConfiguration;

		private static void Because()
		{
			_securityConfiguration = new SecurityConfiguration(policy => { });
		}

		[Test]
		public void Should_not_contain_any_policycontainers()
		{
			// Act
			Because();

			// Assert
			var containers = _securityConfiguration.PolicyContainers.Count();
			Assert.That(containers, Is.EqualTo(0));
		}

		[Test]
		public void Should_not_ignore_missing_configurations()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.IgnoreMissingConfiguration, Is.False);
		}
		
		[Test]
		public void Should_not_have_ServiceLocator()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.ExternalServiceLocator, Is.Null);
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_creating_a_new_SecurityConfiguration_passing_null_as_the_argument
	{
		[Test]
		public void Should_throw()
		{
			// Act & assert
			var exception = Assert.Throws<ArgumentNullException>(() => new SecurityConfiguration(null));

			// Assert
			Assert.That(exception.ParamName, Is.EqualTo("configurationExpression"));
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_configure_with_ignore_missing_configuration
	{
		private static SecurityConfiguration _securityConfiguration;

		private static void Because()
		{
			_securityConfiguration = new SecurityConfiguration(policy => policy.IgnoreMissingConfiguration());
		}

		[Test]
		public void Should_ignore_missing_configurations()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.IgnoreMissingConfiguration, Is.True);
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_I_check_what_I_have
	{
		[Test]
		public void Should_return_the_current_configuration()
		{
			var securityConfiguration = new SecurityConfiguration(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				configuration.IgnoreMissingConfiguration();
				configuration.For<BlogController>(x => x.DeletePost(0)).DenyAnonymousAccess().RequireRole(UserRole.Owner, UserRole.Publisher);
				configuration.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			// Act
			var whatIHave = securityConfiguration.WhatDoIHave();

			// Assert
			Assert.That(whatIHave.Replace("\r\n", "|").Replace("\t", "%"), Is.EqualTo("Ignore missing configuration: True||------------------------------------------------------------------------------------|BlogController > DeletePost|%FluentSecurity.Policy.RequireRolePolicy (Owner or Publisher)|BlogController > Index|%FluentSecurity.Policy.DenyAnonymousAccessPolicy|------------------------------------------------------------------------------------"));
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_setting_the_configuration_to_null
	{
		[Test]
		public void Should_throw()
		{
			// Arrange
			var setConfigurationMethod = typeof(SecurityConfiguration).GetMethod("SetConfiguration", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
			var parameters = new System.Collections.Generic.List<object> { null }.ToArray();

			// Act
			var exception = Assert.Catch(() => setConfigurationMethod.Invoke(null, parameters));

			// Assert
			Assert.That(exception.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
		}
	}
}