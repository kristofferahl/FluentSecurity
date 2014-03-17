using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using FluentSecurity.Configuration;
using FluentSecurity.Specification.Helpers;
using FluentSecurity.Specification.TestData;
using NUnit.Framework;

namespace FluentSecurity.Specification
{
	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_creating_a_new_SecurityConfiguration
	{
		private static ISecurityConfiguration _securityConfiguration;

		private static void Because()
		{
			_securityConfiguration = new SecurityConfiguration<MvcConfiguration>(policy => { });
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
		public void Should_not_have_ServiceLocator()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.Runtime.ExternalServiceLocator, Is.Null);
		}

		[Test]
		public void Should_have_advanced_configuration()
		{
			// Act
			Because();

			// Assert
			Assert.That(_securityConfiguration.Runtime, Is.Not.Null);
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
			var exception = Assert.Throws<ArgumentNullException>(() => new SecurityConfiguration<MvcConfiguration>(null));

			// Assert
			Assert.That(exception.ParamName, Is.EqualTo("configurationExpression"));
		}
	}

	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_I_check_what_I_have
	{
		[Test]
		public void Should_return_the_current_configuration()
		{
			const string expectedOutput = @"Ignore missing configuration: True

------------------------------------------------------------------------------------

FluentSecurity.Specification.TestData.BlogController > DeletePost
	FluentSecurity.Policy.RequireAnyRolePolicy (Owner or Publisher)

FluentSecurity.Specification.TestData.BlogController > Index
	FluentSecurity.Policy.DenyAnonymousAccessPolicy

------------------------------------------------------------------------------------";

			var securityConfiguration = new SecurityConfiguration<MvcConfiguration>(configuration =>
			{
				configuration.GetAuthenticationStatusFrom(StaticHelper.IsAuthenticatedReturnsFalse);
				configuration.Advanced.IgnoreMissingConfiguration();
				configuration.For<BlogController>(x => x.DeletePost(0)).DenyAnonymousAccess().RequireAnyRole(UserRole.Owner, UserRole.Publisher);
				configuration.For<BlogController>(x => x.Index()).DenyAnonymousAccess();
			});

			SecurityConfigurator.SetConfiguration<MvcConfiguration>(securityConfiguration);

			// Act
			var whatIHave = securityConfiguration.WhatDoIHave();

			// Assert
			Assert.That(whatIHave, Is.EqualTo(expectedOutput));
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
			var setConfigurationMethod = typeof(SecurityConfiguration).GetMethod("SetConfiguration", BindingFlags.Static | BindingFlags.NonPublic);
			var parameters = new System.Collections.Generic.List<object> { null }.ToArray();

			// Act
			var exception = Assert.Catch(() => setConfigurationMethod.Invoke(null, parameters));

			// Assert
			Assert.That(exception.InnerException, Is.TypeOf(typeof(ArgumentNullException)));
		}
	}
	
	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_assert_all_actions_on_configuration
	{
		[Test]
		public void Should_throw_exception_when_all_actions_are_not_configured()
		{
			//Arrange & Act
			var securityConfiguration = new SecurityConfiguration<MvcConfiguration>(policy => {});

			//Assert
			Assert.Throws<ConfigurationErrorsException>(() => securityConfiguration.AssertAllActionsAreConfigured());
		}

		[Test]
		public void Should_not_throw_exception_when_all_actions_are_configured()
		{
			//Arrange & Act
			var assemblies = (
				from Assembly assembly in AppDomain.CurrentDomain.GetAssemblies()
				where !(assembly is System.Reflection.Emit.AssemblyBuilder) &&
				assembly.GetType().FullName != "System.Reflection.Emit.InternalAssemblyBuilder" &&
				!assembly.GlobalAssemblyCache 
				select assembly
				).ToArray();

			var securityConfiguration = new SecurityConfiguration<MvcConfiguration>(policy => policy
				.ForAllControllersInAssembly(assemblies)
				.DenyAuthenticatedAccess()
				);
				
			//Assert
			Assert.DoesNotThrow(() => securityConfiguration.AssertAllActionsAreConfigured());
		}
	}
	
	[TestFixture]
	[Category("SecurityConfigurationSpec")]
	public class When_calling_assert_all_actions_on_configuration_passing_assemblies_as_argument
	{
		[Test]
		public void Should_throw_exception_when_all_actions_are_not_configured()
		{
			//Arrange & Act
			var securityConfiguration = new SecurityConfiguration<MvcConfiguration>(policy => { });

			//Assert
			Assert.Throws<ConfigurationErrorsException>(() => securityConfiguration.AssertAllActionsAreConfigured(new [] { GetType().Assembly }));
		}

		[Test]
		public void Should_not_throw_exception_when_all_actions_are_configured()
		{
			//Arrange & Act
			var securityConfiguration = new SecurityConfiguration<MvcConfiguration>(policy => policy
				.ForAllControllers()
				.DenyAuthenticatedAccess()
				);

			//Assert
			Assert.DoesNotThrow(() => securityConfiguration.AssertAllActionsAreConfigured(new [] { GetType().Assembly }));
		}
	}
}